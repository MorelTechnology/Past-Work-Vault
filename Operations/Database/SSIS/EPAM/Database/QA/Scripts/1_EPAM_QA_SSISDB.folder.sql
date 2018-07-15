Use SSISDB
Go

/*
Script Name: C:\ProdTeamWork\Operations\SSIS\ePAM\Database\ePAM\1_BIQAETL01_SSISDB_ePAM.folder.sql
Generated From Catalog Instance: BIQAETL01
Catalog Name: SSISDB
Folder Name: ePAM
Generated By: TRG\rkasu
Generated Date: 11/15/2017 11:49:21 AM
Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP
*/

print 'Script Name: C:\ProdTeamWork\Operations\SSIS\ePAM\Database\ePAM\1_BIQAETL01_SSISDB_ePAM.folder.sql
Generated From Catalog Instance: BIQAETL01
Catalog Name: SSISDB
Folder Name: ePAM
Generated By: TRG\rkasu
Generated Date: 12/11/2017 11:49:21 AM
Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP'
print ''
print '------------------------------------------------------------'
print 'Deployed to Instance: ' + @@servername
print 'Deploy Date: ' + Convert(varchar,GetDate(), 101) + ' ' + Convert(varchar,GetDate(), 108)
print 'Deployed By: ' + original_login()
print '------------------------------------------------------------'
print ''
 -- SSISDB\ePAM

print 'Folder SSISDB\ePAM'
If Not Exists(Select * 
              From SSISDB.[catalog].folders 
              Where name = N'ePAM') 
 begin 
  print ' - Creating ePAM folder' 
  declare @folder_id bigint 
  Exec SSISDB.[catalog].create_folder 
       @folder_name = N'ePAM' 
     , @folder_id = @folder_id OUTPUT 
  print ' - ePAM folder created' 
 end 
else 
 begin 
  print ' - ePAM folder already exists.' 
 end 

 print ' - Setting ePAM folder description to ""' 
  Exec SSISDB.[catalog].set_folder_description 
       @folder_name = N'ePAM' 
     , @folder_description=N'' 
 print ' - ePAM folder description set to ""' 
go 