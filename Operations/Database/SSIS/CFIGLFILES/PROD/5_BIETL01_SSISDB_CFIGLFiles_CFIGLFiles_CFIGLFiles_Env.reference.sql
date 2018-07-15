/*
Script Name: C:\Users\jgabe\Desktop\BIETL01\CFIGLFiles\
Generated From Catalog Instance: BIETL01
Catalog Name: SSISDB
Folder Name: CFIGLFiles
Project Name: CFIGLFiles
Reference Name: CFIGLFiles/Env_CFIGLFiles
Environment Name: Env_CFIGLFiles
Generated By: TRG\jgabe
Generated Date: 2/8/2018 8:05:22 AM
Generated From: CatalogBase v2.0.2.0 executing on: JGABEL-LAP1
*/


 -- SSISDB\CFIGLFiles\CFIGLFiles\[.|CFIGLFiles/Env_CFIGLFiles]
print 'Script Name: C:\Users\jgabe\Desktop\BIETL01\CFIGLFiles\
Generated From Catalog Instance: BIETL01
Catalog Name: SSISDB
Folder Name: CFIGLFiles
Project Name: CFIGLFiles
Reference Name: CFIGLFiles/Env_CFIGLFiles
Environment Name: Env_CFIGLFiles
Generated By: TRG\jgabe
Generated Date: 2/8/2018 8:05:22 AM
Generated From: CatalogBase v2.0.2.0 executing on: JGABEL-LAP1'
print ''
print '------------------------------------------------------------'
print 'Deployed to Instance: ' + @@servername
print 'Deploy Date: ' + Convert(varchar,GetDate(), 101) + ' ' + Convert(varchar,GetDate(), 108)
print 'Deployed By: ' + original_login()
print '------------------------------------------------------------'
print ''
declare @ErrMsg varchar(100) 
print 'Check for folder: CFIGLFiles ' 
If Not Exists(Select name 
              From SSISDB.[catalog].folders 
              Where name = N'CFIGLFiles') 
 begin 
  set @ErrMsg = ' - CFIGLFiles does not exist.' 
  raisError(@ErrMsg, 16, 1) 
  return 
 end 
Else 
 begin 
  print ' - CFIGLFiles folder exists.' 
 end 
print '' 
 
print 'Check for project: CFIGLFiles ' 
If Not Exists(Select name 
              From SSISDB.[catalog].projects 
              Where name = N'CFIGLFiles') 
 begin 
  set @ErrMsg = ' - CFIGLFiles project does not exist.' 
  raisError(@ErrMsg, 16, 1) 
  return 
 end 
Else 
 begin 
  print ' - CFIGLFiles project exists.' 
 end 
print '' 
 
 
print 'Check for environment: Env_CFIGLFiles ' 
If Not Exists(Select name 
              From SSISDB.[catalog].environments 
              Where name = N'Env_CFIGLFiles') 
 begin 
  set @ErrMsg = ' - Env_CFIGLFiles environment does not exist.' 
  raisError(@ErrMsg, 16, 1) 
  return 
 end 
Else 
 begin 
  print ' - Env_CFIGLFiles environment exists.' 
 end 
print '' 
 
print 'Reference SSISDB\CFIGLFiles\CFIGLFiles\[.|CFIGLFiles/Env_CFIGLFiles]'
If Not Exists(Select * 
              From SSISDB.[catalog].environment_references er 
              Join SSISDB.[catalog].projects cp 
                On cp.project_id = er.project_id 
              Join SSISDB.[catalog].folders cf 
                On cf.folder_id = cp.folder_id 
              Where cf.name = N'CFIGLFiles'
                And cp.name = N'CFIGLFiles'
                And er.environment_name = N'Env_CFIGLFiles'
                And er.environment_folder_name Is NULL)
 begin
  print ' - Creating Reference SSISDB\CFIGLFiles\CFIGLFiles\[.|CFIGLFiles/Env_CFIGLFiles]'
  Declare @reference_id bigint 
  Exec [SSISDB].[catalog].[create_environment_reference] 
       @environment_name = N'Env_CFIGLFiles' 
     , @reference_id = @reference_id OUTPUT 
     , @project_name = N'CFIGLFiles' 
     , @folder_name = N'CFIGLFiles' 
     , @environment_folder_name = NULL
     , @reference_type = R 
  print ' - Reference SSISDB\CFIGLFiles\CFIGLFiles\[.|CFIGLFiles/Env_CFIGLFiles] created'
 end
else print ' - Reference SSISDB\CFIGLFiles\CFIGLFiles\[.|CFIGLFiles/Env_CFIGLFiles] already exists.'


print ''