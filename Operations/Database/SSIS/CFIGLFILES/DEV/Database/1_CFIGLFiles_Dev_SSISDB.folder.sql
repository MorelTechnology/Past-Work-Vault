Use SSISDB
Go

/*
Script Name: C:\ProdTeamWork\Operations\SSIS\CFIGLFiles\Database\CFIGLFiles\1_BIDEVETL01_SSISDB_CFIGLFiles.folder.sql
Generated From Catalog Instance: BIDEVETL01
Catalog Name: SSISDB
Folder Name: CFIGLFiles
Generated By: TRG\rkasu
Generated Date: 11/15/2017 11:49:21 AM
Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP
*/

print 'Script Name: C:\ProdTeamWork\Operations\SSIS\CFIGLFiles\Database\CFIGLFiles\1_BIDEVETL01_SSISDB_CFIGLFiles.folder.sql
Generated From Catalog Instance: BIDEVETL01
Catalog Name: SSISDB
Folder Name: CFIGLFiles
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
 -- SSISDB\CFIGLFiles

print 'Folder SSISDB\CFIGLFiles'
If Not Exists(Select * 
              From SSISDB.[catalog].folders 
              Where name = N'CFIGLFiles') 
 begin 
  print ' - Creating CFIGLFiles folder' 
  declare @folder_id bigint 
  Exec SSISDB.[catalog].create_folder 
       @folder_name = N'CFIGLFiles' 
     , @folder_id = @folder_id OUTPUT 
  print ' - CFIGLFiles folder created' 
 end 
else 
 begin 
  print ' - CFIGLFiles folder already exists.' 
 end 

 print ' - Setting CFIGLFiles folder description to ""' 
  Exec SSISDB.[catalog].set_folder_description 
       @folder_name = N'CFIGLFiles' 
     , @folder_description=N'' 
 print ' - CFIGLFiles folder description set to ""' 
go 