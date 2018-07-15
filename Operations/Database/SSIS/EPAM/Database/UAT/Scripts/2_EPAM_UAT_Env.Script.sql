 Use SSISDB
 Go
 
 
-- Environment Variable EPAM\EPAM_Env\FFCS_TIGGLRecon 
Declare @FFCS_TIGGLRecon  sql_variant = N'\\mansan02\batchuat$\ePAM\TIGGLRecon.txt'

-- Environment Variable EPAM\EPAM_Env\FFCS_TIGIERecon
Declare @FFCS_TIGIERecon sql_variant = N'\\mansan02\batchuat$\ePAM\TIGIERecon.txt'

-- Environment Variable EPAM\EPAM_Env\FFCS_TIGPosRecon
Declare @FFCS_TIGPosRecon sql_variant = N'\\mansan02\batchuat$\ePAM\TIGPosRecon.txt'


-- EPAM_Env ENVIRONMENT VARIABLE VALUES --

 -- Environment Variable EPAM\EPAM_Env\CS_CMS_Reporting
Declare @CS_CMS_Reporting sql_variant = N'Data Source= sqluat2012r2;Initial Catalog=cms_reporting;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;'


-----------------------------

/*
Script Name: C:\Users\rkasu\Desktop\EPAMEnvironmentScript\EPAM\4_BIUATETL01_SSISDB_EPAM_EPAM_Env.environment.sql
Generated From Catalog Instance: BIUATETL01
Catalog Name: SSISDB
Folder Name: EPAM
Environment Name: EPAM_Env
Generated By: TRG\rkasu
Generated Date: 10/2/2017 12:59:48 PM
Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP
*/

print 'Script Name: C:\Users\rkasu\Desktop\EPAMEnvironmentScript\EPAM\4_BIUATETL01_SSISDB_EPAM_EPAM_Env.environment.sql
Generated From Catalog Instance: BIUATETL01
Catalog Name: SSISDB
Folder Name: EPAM
Environment Name: EPAM_Env
Generated By: TRG\rkasu
Generated Date: 10/2/2017 12:59:48 PM
Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP'
print ''
print '------------------------------------------------------------'
print 'Deployed to Instance: ' + @@servername
print 'Deploy Date: ' + Convert(varchar,GetDate(), 101) + ' ' + Convert(varchar,GetDate(), 108)
print 'Deployed By: ' + original_login()
print '------------------------------------------------------------'
print ''

 -- Environment SSISDB\EPAM\EPAM_Env
print 'Environment SSISDB\EPAM\EPAM_Env'
If Not Exists(Select * 
           From SSISDB.[catalog].environments e 
           Join SSISDB.[catalog].folders f 
             On f.folder_id = e.folder_id 
           Where e.name = N'EPAM_Env'
             And f.name = N'EPAM')

 begin
  print ' - Creating Environment SSISDB\EPAM\EPAM_Env'

  Exec SSISDB.[catalog].create_environment 
      @environment_name=N'EPAM_Env'
    , @folder_name=N'EPAM'
  print ' - Environment SSISDB\EPAM\EPAM_Env created'
 end
else
 print ' - Environment SSISDB\EPAM\EPAM_Env already exists.'
print ''

-- Environment Variable EPAM\EPAM_Env\FFCS_TIGGLRecon
print 'Environment Variable EPAM\EPAM_Env\FFCS_TIGGLRecon'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'EPAM_Env' 
			And ev.name = N'FFCS_TIGGLRecon' 
			And f.name = N'EPAM') 
 begin
  print ' - Dropping Environment Variable EPAM\EPAM_Env\FFCS_TIGGLRecon'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'FFCS_TIGGLRecon' 
  , @environment_name=N'EPAM_Env' 
  , @folder_name=N'EPAM' 
  print ' - Environment Variable EPAM\EPAM_Env\FFCS_TIGGLRecon dropped'
 end
print ' - Creating Environment Variable EPAM\EPAM_Env\FFCS_TIGGLRecon'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'FFCS_TIGGLRecon'
    , @sensitive = 0
    , @environment_name = N'EPAM_Env'
    , @folder_name = N'EPAM'
    , @value = @FFCS_TIGGLRecon
    , @data_type = N'String'

print ' - Environment Variable EPAM\EPAM_Env\FFCS_TIGGLRecon created'


-- Environment Variable EPAM\EPAM_Env\FFCS_TIGIERecon
print 'Environment Variable EPAM\EPAM_Env\FFCS_TIGIERecon'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'EPAM_Env' 
			And ev.name = N'FFCS_TIGIERecon' 
			And f.name = N'EPAM') 
 begin
  print ' - Dropping Environment Variable EPAM\EPAM_Env\FFCS_TIGIERecon'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'FFCS_TIGIERecon' 
  , @environment_name=N'EPAM_Env' 
  , @folder_name=N'EPAM' 
  print ' - Environment Variable EPAM\EPAM_Env\FFCS_TIGIERecon dropped'
 end
print ' - Creating Environment Variable EPAM\EPAM_Env\FFCS_TIGIERecon'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'FFCS_TIGIERecon'
    , @sensitive = 0
    , @environment_name = N'EPAM_Env'
    , @folder_name = N'EPAM'
    , @value = @FFCS_TIGIERecon
    , @data_type = N'String'

print ' - Environment Variable EPAM\EPAM_Env\FFCS_TIGIERecon created'



-- Environment Variable EPAM\EPAM_Env\ FFCS_TIGPosRecon
print 'Environment Variable EPAM\EPAM_Env\ FFCS_TIGPosRecon'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'EPAM_Env' 
			And ev.name = N'FFCS_TIGPosRecon' 
			And f.name = N'EPAM') 
 begin
  print ' - Dropping Environment Variable EPAM\EPAM_Env\ FCS_TIGPosRecon'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'FFCS_TIGPosRecon' 
  , @environment_name=N'EPAM_Env' 
  , @folder_name=N'EPAM' 
  print ' - Environment Variable EPAM\EPAM_Env\FFCS_TIGPosRecondropped'
 end
print ' - Creating Environment Variable EPAM\EPAM_Env\FFCS_TIGPosRecon'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'FFCS_TIGPosRecon'
    , @sensitive = 0
    , @environment_name = N'EPAM_Env'
    , @folder_name = N'EPAM'
    , @value = @FFCS_TIGPosRecon
    , @data_type = N'String'

print ' - Environment Variable EPAM\EPAM_Env\FFCS_TIGPosReconcreated'

-- Environment Variable EPAM\EPAM_Env\ CS_CMS_Reporting
print 'Environment Variable EPAM\EPAM_Env\CS_CMS_Reporting'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'EPAM_Env' 
			And ev.name = N'CS_CMS_Reporting' 
			And f.name = N'EPAM') 
 begin
  print ' - Dropping Environment Variable EPAM\EPAM_Env\CS_CMS_Reporting'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N' CS_CMS_Reporting' 
  , @environment_name=N'EPAM_Env' 
  , @folder_name=N'EPAM' 
  print ' - Environment Variable EPAM\EPAM_Env\CS_CMS_Reportingdropped'
 end
print ' - Creating Environment Variable EPAM\EPAM_Env\CS_CMS_Reporting'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'CS_CMS_Reporting'
    , @sensitive = 0
    , @environment_name = N'EPAM_Env'
    , @folder_name = N'EPAM'
    , @value = @CS_CMS_Reporting
    , @data_type = N'String'

print ' - Environment Variable EPAM\EPAM_Env\CS_CMS_Reportingcreated'




























