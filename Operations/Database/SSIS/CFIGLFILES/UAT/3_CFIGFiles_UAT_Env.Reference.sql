 Use [SSISDB]
 Go

/*
Script Name: C:\ProdTeamWork\Operations\SSIS\CFIGLFiles\Database\CFIGLFiles
Generated From Catalog Instance: BIUAETL01
Catalog Name: SSISDB
Folder Name: CFIGLFiles
Project Name: CFIGLFiles
Reference Name: CFIGLFiles/CFIGLFiles_Env
Environment Name: CFIGLFiles_Env
Generated By: TRG\rkasu
Generated Date: 11/15/2017 11:49:21 AM
Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP
*/


 -- SSISDB\CFIGLFiles\CFIGLFiles\[.|CFIGLFiles/CFIGLFiles_Env]


--print 'Script Name: C:\ProdTeamWork\Operations\SSIS\CFIGLFiles\Database\CFIGLFiles
--Generated From Catalog Instance: BIUAETL01
--Catalog Name: SSISDB
--Folder Name: CFIGLFiles
--Project Name: CFIGLFiles
--Reference Name: CFIGLFiles/CFIGLFiles_Env
--Environment Name: CFIGLFiles_Env
--Generated By: TRG\rkasu
--Generated Date: 11/15/2017 11:49:21 AM
--Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP'


print ''
print '------------------------------------------------------------'
print 'Deployed to Instance: ' + @@servername
print 'Deploy Date: ' + Convert(varchar,GetDate(), 101) + ' ' + Convert(varchar,GetDate(), 108)
print 'Deployed By: ' + original_login()
print '------------------------------------------------------------'
print ''
print 'Reference SSISDB\CFIGLFiles\CFIGLFiles\[.|CFIGLFiles/CFIGLFiles_Env]'


If Not Exists(Select * 
              From SSISDB.[catalog].environment_references er 
              Join SSISDB.[catalog].projects cp 
                On cp.project_id = er.project_id 
              Join SSISDB.[catalog].folders cf 
                On cf.folder_id = cp.folder_id 
              Where cf.name = N'CFIGLFiles'
                And cp.name = N'CFIGLFiles'
                And er.environment_name = N'CFIGLFiles_Env'
                And er.environment_folder_name Is NULL)
 begin
  print ' - Creating Reference SSISDB\CFIGLFiles\CFIGLFiles\[.|CFIGLFiles/CFIGLFiles_Env]'
  Declare @reference_id bigint 
  Exec [SSISDB].[catalog].[create_environment_reference] 
       @environment_name = N'CFIGLFiles_Env' 
     , @reference_id = @reference_id OUTPUT 
     , @project_name = N'CFIGLFiles' 
     , @folder_name = N'CFIGLFiles' 
     , @environment_folder_name = NULL
     , @reference_type = R 
  print ' - Reference SSISDB\CFIGLFiles\CFIGLFiles\[.|CFIGLFiles/CFIGLFiles_Env] created'
 end
else print ' - Reference SSISDB\CFIGLFiles\CFIGLFiles\[.|CFIGLFiles/CFIGLFiles_Env] already exists.'
go


print ''