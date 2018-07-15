

<# 
This powershell will update all existing Linked Matter sites, to conform to the
new provisioning standard.  Using the current matter list as the best source of
truth for which sites currently exist, the following actions will be performed
for each site:

1. Get the Web

2. Populate the following properties with the values found in the list:
     Legacy_Site_ID (The site ID number from the list for reference)
     Matter_Number (i.e. LM0000xxxxx)
     Case_Caption
     Matter_Name
     Account_Name
     Litigation_Manager (name)
     LMUserID (managers login id)
     Matter_Status
     Docket_Number
     Litigation_Type
     State_Filed
     Venue
     Country
     Work_Matter_Type
     Last_Synchronized (Current Date/Time)
     isMatterActive 
     isLinkedMatter (always true)
     Site_Created (item.created)

3. Change the site url to the LM Number

4. Add a security group to contain only the litmanager, and populate it (LMNumber - Site Manager)

5. Add a security group for read only users (LMNumber - Read Only Users)

6. Add a security group for additional contributors (LMNumber - Additional Contributors)
Now future updates from Claim center will keep these sites upated.

#>

Add-PSSnapin Microsoft.SharePoint.PowerShell

#region Configuration
$ErrorActionPreference = "Stop"
$logFile = "LitMan-Update.log"
$rootweb = Get-SPWeb https://rivernetqa.trg.com/sites/litman
$list = $web.lists["Matters"]
$contentType = "Litigation Matter"
$items = $list.Items | where {$_.ContentType.Name -eq $contentType}
#endregion

#region Functions (at the top because powershell is dumb.)
function getProperty
{
    try
    {
      $property = $args[0].ToString()
      $internalName = $item.Fields[$property].InternalName.toString()
      $internalName = "ows_$internalName"
      $properties.$internalName.toString()
    }
    catch 
    {
      # Property wasn't found, return nothing
    }
  }

function Log 
{ 
  Param ([string]$message)
  $stamp = Get-Date -format G
  Add-content $LogFile -value "$stamp - $message" 
  Write-Host $message
}

function New-SPGroup ($web, $groupName, $owner, $member, $description)
{ 
    if ($web.SiteGroups[$groupName] -ne $null)
    {
        throw "Group $groupName already exists!"   
    }
    else 
    {
        $web.SiteGroups.Add($groupName, $owner, $member, $description)
        $SPGroup = $web.SiteGroups[$groupName]
        $web.RoleAssignments.Add($SPGroup)
    }
}

function AddGroupToSite ($web, $groupName, $permLevel)
{
    $account = $web.SiteGroups[$groupName]
    $assignment = New-Object Microsoft.SharePoint.SPRoleAssignment($account)
    $role = $web.RoleDefinitions[$permLevel]
    $assignment.RoleDefinitionBindings.Add($role);
    $web.RoleAssignments.Add($assignment)
}

#endregion

#region Main Routine
Log "[PROCESS BEGIN]"
Log "Identified $($items.Count) items to process."

$processedCounter = 0
$skippedCounter=0
foreach($item in $items)
{
  # Linked matters use an external content type, so we have to
  # Use the field table to be sure we're getting the right internal
  # name, and then get it from the item's Xml property.

  [xml]$itemxml = $item.Xml
  $properties = $itemxml.row

  # try to get the site
  Try
  {
    $web = (getProperty "Matter Site").Split(",")[0]
  
      Log "Processing $web" 

      # Add the site's properties from the legacy list

      $web.AllProperties["Legacy_Site_ID"] = (getProperty "ID")
      $web.AllProperties["Matter_Number"] = (getProperty "Matter Number: Matter Number")
      $web.AllProperties["Affiliate"] = (getProperty "Matter Number: Affiliate")
      $web.AllProperties["Case_Caption"] = (getProperty "Matter Number: Case Caption")
      $web.AllProperties["Matter_Name"] = (getProperty "Matter Number: Matter Name")
      $web.AllProperties["Account_Name"] = (getProperty "Matter Number: Account Name")
      $web.AllProperties["Litigation_Manager"] = (getProperty "Matter Number: Litigation Manager")
      $web.AllProperties["LMUserID"] = (getProperty "Matter Number: User Name")
      $web.AllProperties["Matter_Status"] = (getProperty "Matter Number: Matter Status")
      $web.AllProperties["Docket_Number"] = (getProperty "Matter Number: Docket Number")
      $web.AllProperties["Litigation_Type"] = (getProperty "Matter Number: Litigation Type")
      $web.AllProperties["State_Filed"] = (getProperty "Matter Number: State Filed")
      $web.AllProperties["Venue"] = (getProperty "Matter Number: Venue")
      $web.AllProperties["Country"] = (getProperty "Matter Number: Country")
      $web.AllProperties["Work_Matter_Type"] = (getProperty "Matter Number: Work/Matter Type")
      $web.AllProperties["Last_Synchronized"] = Get-Date -format G
      $web.AllProperties["isMatterActive"] = (getProperty "Matter Number: IsMatterActive")
      $web.AllProperties["isLinkedMatter"] = "true"
      $web.AllProperties["Site_Created"] = Get-Date($web.Created) -format G
      $web.Update()
      Log "Properties have been set on $web"

      # Create new security groups...

      try
      {
        #  make sure the user is found
        $member = $web|Get-SPUser "TRG\$($web.AllProperties['LMUserID'])"
      }
      catch
      {
        Log "Could not retrieve a user with the ID $($web.AllProperties['LMUserID']).  Will assign the system account for now."
        $member = $web|Get-SPUser $web.site.SystemAccount
        $owner = $web|Get-SPUser $web.site.SystemAccount
      }

      # Add Site Manager Group
      Log "Creating the Site Manager Group"
      $groupName = "$($web.AllProperties['Matter_Number']) - Site Manager"
      $description = "This group should contain ONLY ONE user who is designated as the manager of $($web.AllProperties['Matter_Number'])."
      New-SPGroup -web $web -groupName $groupName -owner $owner -member $member
      Log "Adding Group to Site"
      AddGroupToSite -web $web -groupName $groupName -permLevel "Contribute"

      # Add Read Only Users Group
      Log "Creating Read Only Users Group"
      $groupName = "$($web.AllProperties['Matter_Number']) - Read Only Users"
      $description = "This group contains users with Read-Only access to $($web.AllProperties['Matter_Number'])."
      New-SPGroup -web $web -groupName $groupName -owner $owner -member $member
      Log "Adding Group to Site"
      AddGroupToSite -web $web -groupName $groupName -permLevel "Read"

      # Add Additional Participants Group
      Log "Creating Additional Participants Group"
      $groupName = "$($web.AllProperties['Matter_Number']) - Additional Contributors"
      $description = "This group contains users, who, in addition to the Site Manager, can contribute to $($web.AllProperties['Matter_Number']). Use of this group should be limited to rare exceptions where more than one person requires control of the site."
      New-SPGroup -web $web -groupName $groupName -owner $owner -member $member
      Log "Adding Group to Site"
      AddGroupToSite -web $web -groupName $groupName -permLevel "Contribute"

      # Set the URL to use the new format.
      Log "Modifying URL from /$($web.Url.Split('/')[$_.length-1]) to /$($web.AllProperties['Matter_Number'])"
      $web|Set-SPWeb -RelativeUrl $($web.AllProperties['Matter_Number'])

      $processedCounter++

  }
  Catch
  {
    Log "Issue while attempting to retrieve a site at $($web)."
    Log  $_.Exception.Message
    write-host "[Continuing...]"
    $skippedCounter++
  }

  Log "Operation Completed.  Items Processed: $($processedCounter).  Items Skipped: $($skippedCounter)."

#endregion











