Use SSISDB
Go

/*
Script Name: C:\ProdTeamWork\Operations\SSIS\Fairmont_WINS\Database\Fairmont_WINS\1_BIUATETL01_SSISDB_Fairmont_WINS.folder.sql
Generated From Catalog Instance: BIUATETL01
Catalog Name: SSISDB
Folder Name: Fairmont_WINS
Generated By: TRG\rkasu
Generated Date: 11/15/2017 11:49:21 AM
Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP
*/

print 'Script Name: C:\ProdTeamWork\Operations\SSIS\Fairmont_WINS\Database\Fairmont_WINS\1_BIUATETL01_SSISDB_Fairmont_WINS.folder.sql
Generated From Catalog Instance: BIUATETL01
Catalog Name: SSISDB
Folder Name: Fairmont_WINS
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
 -- SSISDB\Fairmont_WINS

print 'Folder SSISDB\Fairmont_WINS'
If Not Exists(Select * 
              From SSISDB.[catalog].folders 
              Where name = N'Fairmont_WINS') 
 begin 
  print ' - Creating Fairmont_WINS folder' 
  declare @folder_id bigint 
  Exec SSISDB.[catalog].create_folder 
       @folder_name = N'Fairmont_WINS' 
     , @folder_id = @folder_id OUTPUT 
  print ' - Fairmont_WINS folder created' 
 end 
else 
 begin 
  print ' - Fairmont_WINS folder already exists.' 
 end 

 print ' - Setting Fairmont_WINS folder description to ""' 
  Exec SSISDB.[catalog].set_folder_description 
       @folder_name = N'Fairmont_WINS' 
     , @folder_description=N'Fairmont_WINS' 
 print ' - Fairmont_WINS folder description set to "Fairmont_WINS"' 
go 