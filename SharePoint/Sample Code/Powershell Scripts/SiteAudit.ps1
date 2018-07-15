# CRE 11/1/2016 - Jeremy Morel
# Produces an audit report showing which users and groups have access to a given site and all subwebs.

#Syntax: SiteAudit.ps1 | out-file c:\permissions.txt

#Add SharePoint PowerShell SnapIn if not already added
if ((Get-PSSnapin "Microsoft.SharePoint.PowerShell" -ErrorAction SilentlyContinue) -eq $null) {
Add-PSSnapin "Microsoft.SharePoint.PowerShell"
}

#Define variables
$site = Get-SPSite "https://rivernet2n.trg.com/sites/General Counsel"

#Get all subsites for site collection
$web = $site.AllWebs

#Loop through each subsite and write permissions

foreach ($web in $web)
{
if (($web.permissions -ne $null) -and ($web.hasuniqueroleassignments -eq "True"))
{
Write-Output "****************************************"
Write-Output "Displaying site permissions for: $web"
$web.permissions | fl member, basepermissions
}
elseif ($web.hasuniqueroleassignments -ne "True")
{
Write-Output "****************************************"
Write-Output "Displaying site permissions for: $web"
"$web inherits permissions from $site"
}

#Loop through each list in each subsite and get permissions

foreach ($list in $web.lists)
{
$unique = $list.hasuniqueroleassignments
if (($list.permissions -ne $null) -and ($unique -eq "True"))
{
Write-Output "****************************************"
Write-Output "Displaying Lists permissions for: $web \ $list"
$list.permissions | fl member, basepermissions
}
elseif ($unique -ne "True") {
Write-Output "$web \ $list inherits permissions from $web"
}
}
}
Write-Host "Finished."
$site.dispose()
$web.dispose()
