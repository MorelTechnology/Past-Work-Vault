/*
Script Name: H:\My SQL Code\Deployment Scripts\BIETL01\OPSDW_AG_Refresh\\1_BIETL01_SSISDB_OPSDW_AG_Refresh.folder.sql
Generated From Catalog Instance: BIETL01
Catalog Name: SSISDB
Folder Name: OPSDW_AG_Refresh
Generated By: TRG\jgabe
Generated Date: 2/23/2018 1:38:05 PM
Generated From: CatalogBase v2.0.2.0 executing on: JGABEL-LAP1
*/

Use SSISDB 
go 

print 'Script Name: H:\My SQL Code\Deployment Scripts\BIETL01\OPSDW_AG_Refresh\\1_BIETL01_SSISDB_OPSDW_AG_Refresh.folder.sql
Generated From Catalog Instance: BIETL01
Catalog Name: SSISDB
Folder Name: OPSDW_AG_Refresh
Generated By: TRG\jgabe
Generated Date: 2/23/2018 1:38:05 PM
Generated From: CatalogBase v2.0.2.0 executing on: JGABEL-LAP1'
print ''
print '------------------------------------------------------------'
print 'Deployed to Instance: ' + @@servername
print 'Deploy Date: ' + Convert(varchar,GetDate(), 101) + ' ' + Convert(varchar,GetDate(), 108)
print 'Deployed By: ' + original_login()
print '------------------------------------------------------------'
print ''
 -- SSISDB\OPSDW_AG_Refresh

print 'Folder SSISDB\OPSDW_AG_Refresh'
If Not Exists(Select * 
              From SSISDB.[catalog].folders 
              Where name = N'OPSDW_AG_Refresh') 
 begin 
  print ' - Creating OPSDW_AG_Refresh folder' 
  declare @folder_id bigint 
  Exec SSISDB.[catalog].create_folder 
       @folder_name = N'OPSDW_AG_Refresh' 
     , @folder_id = @folder_id OUTPUT 
  print ' - OPSDW_AG_Refresh folder created' 
 end 
else 
 begin 
  print ' - OPSDW_AG_Refresh folder already exists.' 
 end 

 print ' - Setting OPSDW_AG_Refresh folder description to ""' 
  Exec SSISDB.[catalog].set_folder_description 
       @folder_name = N'OPSDW_AG_Refresh' 
     , @folder_description=N'' 
 print ' - OPSDW_AG_Refresh folder description set to ""' 
go 