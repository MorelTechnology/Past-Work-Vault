 param ( [Parameter(Mandatory=$true)][string]$Url, [Parameter(Mandatory=$true)][string]$ProjectName )
 
 <#
.SYNOPSIS
  Name:           PostDeployScript.ps1
  Version:        1.1
  Author:         Jeremy M. Morel
  Creation Date:  05/25/2017
  Purpose:		  Used to resolve SharePoint deployment issues in Visual Studio, and add debugging support for multi-server farms.

.DESCRIPTION
  During deploy attemts, an error is typically presented, similar to the following:
   
  "Error occurred in deployment step ‘Activate Features’: feature with ID '{guid}' is not installed in this farm, 
  and cannot be added to this scope." 
  
  The cause is that Visual Studio deploys using the method 'Microsoft.SharePoint.Administration.SPSolution.DeployLocal()', 
  which can only deploy and activate features on the local machine when it's the only server in the farm. 
  More specifically, the process doesn't wait for full-farm deployment before attempting feature activation.  Feature activation
  consequently fails, causing the build to fail and retract, possibly leaving artifacts behind. 
  
  This approach for resolution is to replace Visual Studio feature activation action with this script as a post-deployment 
  step.  The script ensures the feature was installed, and only then activates it.

.USAGE
  1. Add this script at your project's root level.
  2. Edit your Project Properties, select the 'SharePoint' Tab:
  
      A) Set Active Deployment Configuration to 'No Activation' 
      B) Paste the below command in Post-Deployment Command Line box:
       
  %SystemRoot%\sysnative\WindowsPowerShell\v1.0\powershell "start-process powershell.exe -ArgumentList '$(ProjectDir)PostDeployScript.ps1 -Url $(SharePointSiteUrl) -ProjectName $(ProjectName)' "

  Note: You can add '-NoExit' in the -ArgumentList array (before '$(ProjectDir)') to  keep the powershell console visible for troubleshooting.

  3. Save project, build and deploy.

.PARAMETER Url
    The Url to the SharePoint site you are deploying to and/or debugging against.  
    *** Typically you can use $(SharePointSiteUrl) from the deployment command line *** 

.PARAMETER ProjectName
    The name of your SharePoint Project in Visual Studio.
    *** Typically you can use $(ProjectName) from the deployment command line ***

.EXAMPLE
  .\PostDeployScript.ps1 -Url https://sharepoint.host.com/Sites/Site -ProjectName MySharePointProject
#>

# This hack keeps the window on top so you can see status messages... ################################################################
$signature = @' 
[DllImport("user32.dll")] 
public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags); 
'@ 
$type = Add-Type -MemberDefinition $signature -Name SetWindowPosition -Namespace SetWindowPos -Using System.Text -PassThru
$handle = (Get-Process -id $Global:PID).MainWindowHandle 
$alwaysOnTop = New-Object -TypeName System.IntPtr -ArgumentList (-1) 
$type::SetWindowPos($handle, $alwaysOnTop, 0, 0, 0, 0, 0x0003) 
CLS
######################################################################################################################################
write-output "[Post-Deploy Feature Deployment Routine Started.]"

# If possible, reuse thread for memory efficiency
$ver = $host | select version
if ($ver.Version.Major -gt 1) {
       $host.Runspace.ThreadOptions = "ReuseThread"
} 

# Add SharePoint snap-in if needed 
write-output "[Ensuring Microsoft.SharePoint.PowerShell snap-in is loaded...]"
if ((Get-PSSnapin "Microsoft.SharePoint.PowerShell" -ErrorAction SilentlyContinue) -eq $null) {
    Add-PSSnapin "Microsoft.SharePoint.PowerShell"
}

# Get the names of all features found in the project
$AllFeatures = Get-ChildItem Features | Where-Object {$_.PSIsContainer} | Foreach-Object {$_.Name}

foreach($item in $AllFeatures)
{
  $FeatureString = $ProjectName+"_"+$item

# If feature already enabled, disable & retract it

$feature = Get-SPFeature -Site $Url | Where {$_.DisplayName -eq $FeatureString}
if ($feature -ne $null) {
       write-output "[ Feature '$($item)' found in target site $($Url). ]"
       Disable-SPFeature -Identity $FeatureString -Url $Url -confirm:$false
       write-output "[ Feature '$($item)' deactivated. ]"
       Uninstall-SPFeature $FeatureString
       write-output "[ Feature '$($item)' uninstalled. ]"
}
else{
       write-output "[ Feature '$($item)' was not found in target site $($Url). ]"
}

# installing the feature
Install-SPFeature -Path $FeatureString
echo ""
write-output "[ Feature '$($item)' was installed to the hive. ]"

# If feature already enabled then disable it, because the operation is not complete (web parts are not copied
# to web part gallery in site collection)
# Note: feature will get automatically enabled when installed if scope is either 'SiteCollection' or 'Web'
# $FeatureString is in the format: [ ProjectName_FeatureName ] and has nothing to do with feature title

$feature = Get-SPFeature -Site $Url | Where {$_.Displayname -like $FeatureString}

if ($feature -ne $null) {
       write-output "[ Feature '$($item)' was found to be prematurely activated. Deactivating... ]"
       Disable-SPFeature -Identity $FeatureString -Url $Url -confirm:$false
       write-output "[ Feature deactivated. ]"
}
else {
       write-output "[ Feature '$($item)' is ready for activation. ]"
}

# Enable the feature
Enable-SPFeature -Identity $FeatureString -Url $Url
write-output "[ Feature '$($item)' has been activated at target site $($Url). ]"
write-output "[ Feature deployment and activation has completed. ]"

}