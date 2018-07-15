 USE [SSISDB]
 GO
 
 
 -- BAI_Prod ENVIRONMENT VARIABLE VALUES --


-- Environment Variable BAI\BAI_Prod\BAIError_Cntr
Declare @BAIError_Cntr int = 0


-- Environment Variable BAI\BAI_Prod\JPMI_DirectoryName
Declare @JPMI_DirectoryName sql_variant = N'\\mansan02\batch$\CashManagement\JPMorgan\IntraDay\'
-- Environment Variable BAI\BAI_Prod\JPMI_FileName
Declare @JPMI_FileName sql_variant = N'TINCO.IRIS.CBAI.txt'
-- Environment Variable BAI\BAI_Prod\JPMI_FullPath
Declare @JPMI_FullPath sql_variant = N'\\mansan02\batch$\CashManagement\JPMorgan\IntraDay\TINCO.IRIS.CBAI.txt'


-- Environment Variable BAI\BAI_Prod\JPMP_DirectoryName
Declare @JPMP_DirectoryName sql_variant = N'\\mansan02\batch$\CashManagement\JPMorgan\Prior\'
-- Environment Variable BAI\BAI_Prod\JPMP_FileName
Declare @JPMP_FileName sql_variant = N'TINCO.IRIS.PBAI.txt'
-- Environment Variable BAI\BAI_Prod\JPMP_FullPath
Declare @JPMP_FullPath sql_variant = N'\\mansan02\batch$\CashManagement\JPMorgan\Prior\TINCO.IRIS.PBAI.txt'


-- Environment Variable BAI\BAI_Prod\KEYI_DirectoryName
Declare @KEYI_DirectoryName sql_variant = N'\\mansan02\batch$\CashManagement\KeyBank\IntraDay\'
-- Environment Variable BAI\BAI_Prod\KEYI_FileName
Declare @KEYI_FileName sql_variant = N'riverstoneID.txt'
-- Environment Variable BAI\BAI_Prod\KEYI_FullPath
Declare @KEYI_FullPath sql_variant = N'\\mansan02\batch$\CashManagement\KeyBank\IntraDay\riverstoneID.txt'


-- Environment Variable BAI\BAI_Prod\KEYP_DirectoryName
Declare @KEYP_DirectoryName sql_variant = N'\\mansan02\batch$\CashManagement\KeyBank\Prior\'
-- Environment Variable BAI\BAI_Prod\KEYP_FileName
Declare @KEYP_FileName sql_variant = N'riverstonePD.txt'
-- Environment Variable BAI\BAI_Prod\KEYP_FullPath
Declare @KEYP_FullPath sql_variant = N'\\mansan02\batch$\CashManagement\KeyBank\Prior\riverstonePD.txt'


-- Environment Variable BAI\BAI_Prod\Email_Distribution_List
Declare @Email_Distribution_List sql_variant = N'ITProductionSupport@trg.com'

-- Environment Variable BAI\BAI_Prod\CS_BAIStaging
Declare @CS_BAIStaging sql_variant = N'Data Source=MANSQL01;Initial Catalog=BAI_Staging;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;'

-- Environment Variable BAI\BAI_Prod\CS_BAIStaging\ServerName
Declare @ServerName sql_variant = N'MANSQL01'


---- Environment Variable 'SMTP Connection'
--Declare @SendMailConnection sql_variant = N'SmtpServer=manrelay01.trg.com;UseWindowsAuthentication=False;EnableSsl=False;Timeout=2000;'


----Env Variable 'SMTP Connection'
--SET @var = N'SmtpServer=manrelay01.trg.com;UseWindowsAuthentication=False;EnableSsl=False;Timeout=2000;'
--IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'SMTP Connection')
--	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'SMTP Connection', @sensitive=0, @description=N'', @environment_name=N'Env_Asbestos', @folder_name=N'ENTAsbestos', @value=@var, @data_type=N'String'

 ---------------------------------

/*
Script Name: C:\ProdTeamWorkRequirementDocs\BAI_Prod\BAI\4_BIETL01_SSISDB_BAI_BAI_Prod.environment.sql
Generated From Catalog Instance: BIETL01
Catalog Name: SSISDB
Folder Name: BAI
Environment Name: BAI_Prod
Generated By: TRG\rkasu
Generated Date: 2/22/2017 2:24:51 PM
Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP
*/

print 'Script Name: C:\ProdTeamWork\Operations\SSIS\BAI2\Database\1_BIETL01_SSISDB_BAI_BAI_Prod.environment.sql
Generated From Catalog Instance: BIETL01
Catalog Name: SSISDB
Folder Name: BAI
Environment Name: BAI_Prod
Generated By: TRG\rkasu
Generated Date: 2/22/2017 2:24:51 PM
Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP'
print ''
print '------------------------------------------------------------'
print 'Deployed to Instance: ' + @@servername
print 'Deploy Date: ' + Convert(varchar,GetDate(), 101) + ' ' + Convert(varchar,GetDate(), 108)
print 'Deployed By: ' + original_login()
print '------------------------------------------------------------'
print ''

 -- Environment SSISDB\BAI\BAI_Prod
print 'Environment SSISDB\BAI\BAI_Prod'
If Not Exists(Select * 
           From SSISDB.[catalog].environments e 
           Join SSISDB.[catalog].folders f 
             On f.folder_id = e.folder_id 
           Where e.name = N'BAI_Prod'
             And f.name = N'BAI')

 begin
  print ' - Creating Environment SSISDB\BAI\BAI_Prod'

  Exec SSISDB.[catalog].create_environment 
      @environment_name=N'BAI_Prod'
    , @folder_name=N'BAI'
  print ' - Environment SSISDB\BAI\BAI_Prod created'
 end
else
 print ' - Environment SSISDB\BAI\BAI_Prod already exists.'
print ''



-- -- Environment Variable BAI\BAI_Prod\BAIError_Cntr
print '-- Environment Variable BAI\BAI_Prod\BAIError_Cntr'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'BAIError_Cntr' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping -- Environment Variable BAI\BAI_Prod\BAIError_Cntr'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'BAIError_Cntr' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - -- Environment Variable BAI\BAI_Prod\BAIError_Cntr dropped'
 end
print ' - Creating -- Environment Variable BAI\BAI_Prod\BAIError_Cntr'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'BAIError_Cntr'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @BAIError_Cntr
    , @data_type = N'Int32'

print ' - -- Environment Variable BAI\BAI_Prod\BAIError_Cntr created'

-- Set Environment Variable BAI\BAI_Prod\BAIError_Cntr
print ' - Set -- Environment Variable BAI\BAI_Prod\BAIError_Cntr value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'BAIError_Cntr' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @BAIError_Cntr
print ' - -- Environment Variable BAI\BAI_Prod\BAIError_Cntr value set'
print ''

-- Environment Variable BAI\BAI_Prod\JPMI_DirectoryName
print 'Environment Variable BAI\BAI_Prod\JPMI_DirectoryName'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'JPMI_DirectoryName' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\JPMI_DirectoryName'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'JPMI_DirectoryName' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\JPMI_DirectoryName dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\JPMI_DirectoryName'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'JPMI_DirectoryName'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @JPMI_DirectoryName
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\JPMI_DirectoryName created'

-- Set Environment Variable BAI\BAI_Prod\JPMI_DirectoryName
print ' - Set Environment Variable BAI\BAI_Prod\JPMI_DirectoryName value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'JPMI_DirectoryName' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @JPMI_DirectoryName
print ' - Environment Variable BAI\BAI_Prod\JPMI_DirectoryName value set'
print ''

-- Environment Variable BAI\BAI_Prod\JPMI_FileName
print 'Environment Variable BAI\BAI_Prod\JPMI_FileName'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'JPMI_FileName' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\JPMI_FileName'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'JPMI_FileName' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\JPMI_FileName dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\JPMI_FileName'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'JPMI_FileName'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @JPMI_FileName
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\JPMI_FileName created'

-- Set Environment Variable BAI\BAI_Prod\JPMI_FileName
print ' - Set Environment Variable BAI\BAI_Prod\JPMI_FileName value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'JPMI_FileName' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @JPMI_FileName
print ' - Environment Variable BAI\BAI_Prod\JPMI_FileName value set'
print ''




-- Environment Variable BAI\BAI_Prod\JPMI_FullPath
print 'Environment Variable BAI\BAI_Prod\JPMI_FullPath'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'JPMI_FullPath' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\JPMI_FullPath'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'JPMI_FullPath' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\JPMI_FullPath dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\JPMI_FullPath'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'JPMI_FullPath'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @JPMI_FullPath
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\JPMI_FullPath created'

-- Set Environment Variable BAI\BAI_Prod\JPMI_FullPath
print ' - Set Environment Variable BAI\BAI_Prod\JPMI_FullPath value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'JPMI_FullPath' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @JPMI_FullPath
print ' - Environment Variable BAI\BAI_Prod\JPMI_FullPath value set'
print ''
 

-- Environment Variable BAI\BAI_Prod\JPMP_DirectoryName
print 'Environment Variable BAI\BAI_Prod\JPMP_DirectoryName'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'JPMP_DirectoryName' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\JPMP_DirectoryName'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'JPMP_DirectoryName' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\JPMP_DirectoryName dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\JPMP_DirectoryName'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'JPMP_DirectoryName'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @JPMP_DirectoryName
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\JPMP_DirectoryName created'

-- Set Environment Variable BAI\BAI_Prod\JPMP_DirectoryName
print ' - Set Environment Variable BAI\BAI_Prod\JPMP_DirectoryName value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'JPMP_DirectoryName' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @JPMP_DirectoryName
print ' - Environment Variable BAI\BAI_Prod\JPMP_DirectoryName value set'
print ''

-- Environment Variable BAI\BAI_Prod\JPMP_FileName
print 'Environment Variable BAI\BAI_Prod\JPMP_FileName'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'JPMP_FileName' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\JPMP_FileName'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'JPMP_FileName' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\JPMP_FileName dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\JPMP_FileName'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'JPMP_FileName'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @JPMP_FileName
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\JPMP_FileName created'

-- Set Environment Variable BAI\BAI_Prod\JPMP_FileName
print ' - Set Environment Variable BAI\BAI_Prod\JPMP_FileName value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'JPMP_FileName' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @JPMP_FileName
print ' - Environment Variable BAI\BAI_Prod\JPMP_FileName value set'
print ''


-- Environment Variable BAI\BAI_Prod\JPMP_FullPath
print 'Environment Variable BAI\BAI_Prod\JPMP_FullPath'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'JPMP_FullPath' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\JPMP_FullPath'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'JPMP_FullPath' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\JPMP_FullPath dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\JPMP_FullPath'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'JPMP_FullPath'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @JPMP_FullPath
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\JPMP_FullPath created'

-- Set Environment Variable BAI\BAI_Prod\JPMP_FullPath
print ' - Set Environment Variable BAI\BAI_Prod\JPMP_FullPath value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'JPMP_FullPath' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @JPMP_FullPath
print ' - Environment Variable BAI\BAI_Prod\JPMP_FullPath value set'
print ''
 
 
 
 --- Environment Variable BAI\BAI_Prod\KEYI_DirectoryName
print 'Environment Variable BAI\BAI_Prod\KEYI_DirectoryName'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'KEYI_DirectoryName' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\KEYI_DirectoryName'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'KEYI_DirectoryName' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\KEYI_DirectoryName dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\KEYI_DirectoryName'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'KEYI_DirectoryName'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @KEYI_DirectoryName
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\KEYI_DirectoryName created'

-- Set Environment Variable BAI\BAI_Prod\KEYI_DirectoryName
print ' - Set Environment Variable BAI\BAI_Prod\KEYI_DirectoryName value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'KEYI_DirectoryName' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @KEYI_DirectoryName
print ' - Environment Variable BAI\BAI_Prod\KEYI_DirectoryName value set'
print ''

-- Environment Variable BAI\BAI_Prod\KEYI_FileName
print 'Environment Variable BAI\BAI_Prod\KEYI_FileName'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'KEYI_FileName' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\KEYI_FileName'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'KEYI_FileName' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\KEYI_FileName dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\KEYI_FileName'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'KEYI_FileName'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @KEYI_FileName
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\KEYI_FileName created'

-- Set Environment Variable BAI\BAI_Prod\KEYI_FileName
print ' - Set Environment Variable BAI\BAI_Prod\KEYI_FileName value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'KEYI_FileName' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @KEYI_FileName
print ' - Environment Variable BAI\BAI_Prod\KEYI_FileName value set'
print ''


-- Environment Variable BAI\BAI_Prod\KEYI_FullPath
print 'Environment Variable BAI\BAI_Prod\KEYI_FullPath'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'KEYI_FullPath' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\KEYI_FullPath'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'KEYI_FullPath' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\KEYI_FullPath dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\KEYI_FullPath'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'KEYI_FullPath'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @KEYI_FullPath
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\KEYI_FullPath created'

-- Set Environment Variable BAI\BAI_Prod\KEYI_FullPath
print ' - Set Environment Variable BAI\BAI_Prod\KEYI_FullPath value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'KEYI_FullPath' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @KEYI_FullPath
print ' - Environment Variable BAI\BAI_Prod\KEYI_FullPath value set'
print ''
 
 
 -- Environment Variable BAI\BAI_Prod\KEYP_DirectoryName
print 'Environment Variable BAI\BAI_Prod\KEYP_DirectoryName'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'KEYP_DirectoryName' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\KEYP_DirectoryName'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'KEYP_DirectoryName' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\KEYP_DirectoryName dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\KEYP_DirectoryName'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'KEYP_DirectoryName'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @KEYP_DirectoryName
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\KEYP_DirectoryName created'

-- Set Environment Variable BAI\BAI_Prod\KEYP_DirectoryName
print ' - Set Environment Variable BAI\BAI_Prod\KEYP_DirectoryName value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'KEYP_DirectoryName' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @KEYP_DirectoryName
print ' - Environment Variable BAI\BAI_Prod\KEYP_DirectoryName value set'
print ''

-- Environment Variable BAI\BAI_Prod\KEYP_FileName
print 'Environment Variable BAI\BAI_Prod\KEYP_FileName'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'KEYP_FileName' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\KEYP_FileName'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'KEYP_FileName' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\KEYP_FileName dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\KEYP_FileName'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'KEYP_FileName'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @KEYP_FileName
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\KEYP_FileName created'

-- Set Environment Variable BAI\BAI_Prod\KEYP_FileName
print ' - Set Environment Variable BAI\BAI_Prod\KEYP_FileName value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'KEYP_FileName' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @KEYP_FileName
print ' - Environment Variable BAI\BAI_Prod\KEYP_FileName value set'
print ''

-- Environment Variable BAI\BAI_Prod\KEYP_FullPath
print 'Environment Variable BAI\BAI_Prod\KEYP_FullPath'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'KEYP_FullPath' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\KEYP_FullPath'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'KEYP_FullPath' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\KEYP_FullPath dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\KEYP_FullPath'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'KEYP_FullPath'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @KEYP_FullPath
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\KEYP_FullPath created'

-- Set Environment Variable BAI\BAI_Prod\KEYP_FullPath
print ' - Set Environment Variable BAI\BAI_Prod\KEYP_FullPath value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'KEYP_FullPath' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @KEYP_FullPath
print ' - Environment Variable BAI\BAI_Prod\KEYP_FullPath value set'
print ''
 
 
-- Environment Variable BAI\BAI_Prod\Email_Distribution_List
print 'Environment Variable BAI\BAI_Prod\Email_Distribution_List'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'Email_Distribution_List' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\Email_Distribution_List'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'Email_Distribution_List' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\Email_Distribution_List dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\Email_Distribution_List'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'Email_Distribution_List'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @Email_Distribution_List
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\Email_Distribution_List created'

-- Set Environment Variable BAI\BAI_Prod\Email_Distribution_List
print ' - Set Environment Variable BAI\BAI_Prod\Email_Distribution_List value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'Email_Distribution_List' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @Email_Distribution_List
print ' - Environment Variable BAI\BAI_Prod\Email_Distribution_List value set'
print ''


-- Environment Variable BAI\BAI_Prod\CS_BAIStaging
print 'Environment Variable BAI\BAI_Prod\CS_BAIStaging'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'CS_BAIStaging' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\CS_BAIStaging'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'CS_BAIStaging' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\CS_BAIStaging dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\CS_BAIStaging'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'CS_BAIStaging'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @CS_BAIStaging
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\CS_BAIStaging created'

-- Set Environment Variable BAI\BAI_Prod\CS_BAIStaging
print ' - Set Environment Variable BAI\BAI_Prod\CS_BAIStaging value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'CS_BAIStaging' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @CS_BAIStaging
print ' - Environment Variable BAI\BAI_Prod\CS_BAIStaging value set'
print ''


-- Environment Variable BAI\BAI_Prod\ServerName
print 'Environment Variable BAI\BAI_Prod\ServerName'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'BAI_Prod' 
			And ev.name = N'ServerName' 
			And f.name = N'BAI') 
 begin
  print ' - Dropping Environment Variable BAI\BAI_Prod\ServerName'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'ServerName' 
  , @environment_name=N'BAI_Prod' 
  , @folder_name=N'BAI' 
  print ' - Environment Variable BAI\BAI_Prod\ServerName dropped'
 end
print ' - Creating Environment Variable BAI\BAI_Prod\ServerName'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'ServerName'
    , @sensitive = 0
    , @environment_name = N'BAI_Prod'
    , @folder_name = N'BAI'
    , @value = @ServerName
    , @data_type = N'String'

print ' - Environment Variable BAI\BAI_Prod\ServerName created'

-- Set Environment Variable BAI\BAI_Prod\ServerName
print ' - Set Environment Variable BAI\BAI_Prod\ServerName value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'ServerName' 
    , @environment_name = N'BAI_Prod' 
    , @folder_name = N'BAI' 
    , @value = @ServerName
print ' - Environment Variable BAI\BAI_Prod\ServerName value set'
print ''
