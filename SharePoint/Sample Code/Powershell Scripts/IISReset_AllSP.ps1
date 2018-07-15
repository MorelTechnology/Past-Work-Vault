# This script will list all your SP Servers and restarts IIS on all of them.

add-pssnapin microsoft.sharepoint.powershell 
$spserver = get-spserver | ?{$_.role -eq "Application"} 
foreach ($server in $spserver) 
{
    write-host "Performing IIS Reset on Server:"$server.name
    iisreset $server.Name 
}