 Use SSISDB
 Go
 


-- Environment Variable Fairmont_WINS\Fairmont_WINS_Env\FFCS_TIGIERecon
Declare @DBFileName sql_variant = N'\\mansan02\batchdev$\Fairfax\History\Jan 2017 FM_DIRECT_and_CEDED_LOSSES_MONTHLY.mdb'

-- Environment Variable Fairmont_WINS\Fairmont_WINS_Env\FFCS_TIGPosRecon
Declare @Email_Distribution_List sql_variant = N'roja_kasula@trg.com'


-- Fairmont_WINS_Env ENVIRONMENT VARIABLE VALUES --

 -- Environment Variable Fairmont_WINS\Fairmont_WINS_Env\CS_CMS_Reporting
Declare @CS_Reporting sql_variant = N'Data Source= MandevData02;Initial Catalog=reporting;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;'


 -- Environment Variable Fairmont_WINS\Fairmont_WINS_Env\SMTPRelay
Declare @SMTPRelay sql_variant = N'SmtpServer=manrelay01.trg.com;UseWindowsAuthentication=False;EnableSsl=False;Timeout=2000;'


-----------------------------

/*
Script Name: C:\Users\rkasu\Desktop\Fairmont_WINSEnvironmentScript\Fairmont_WINS\4_BIDEVETL01_SSISDB_Fairmont_WINS_Fairmont_WINS_Env.environment.sql
Generated From Catalog Instance: BIDEVETL01
Catalog Name: SSISDB
Folder Name: Fairmont_WINS
Environment Name: Fairmont_WINS_Env
Generated By: TRG\rkasu
Generated Date: 10/2/2017 12:59:48 PM
Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP
*/

print 'Script Name: C:\Users\rkasu\Desktop\Fairmont_WINSEnvironmentScript\Fairmont_WINS\4_BIDEVETL01_SSISDB_Fairmont_WINS_Fairmont_WINS_Env.environment.sql
Generated From Catalog Instance: BIDEVETL01
Catalog Name: SSISDB
Folder Name: Fairmont_WINS
Environment Name: Fairmont_WINS_Env
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

 -- Environment SSISDB\Fairmont_WINS\Fairmont_WINS_Env
print 'Environment SSISDB\Fairmont_WINS\Fairmont_WINS_Env'
If Not Exists(Select * 
           From SSISDB.[catalog].environments e 
           Join SSISDB.[catalog].folders f 
             On f.folder_id = e.folder_id 
           Where e.name = N'Fairmont_WINS_Env'
             And f.name = N'Fairmont_WINS')

 begin
  print ' - Creating Environment SSISDB\Fairmont_WINS\Fairmont_WINS_Env'

  Exec SSISDB.[catalog].create_environment 
      @environment_name=N'Fairmont_WINS_Env'
    , @folder_name=N'Fairmont_WINS'
  print ' - Environment SSISDB\Fairmont_WINS\Fairmont_WINS_Env created'
 end
else
 print ' - Environment SSISDB\Fairmont_WINS\Fairmont_WINS_Env already exists.'
print ''



-- Environment Variable Fairmont_WINS\Fairmont_WINS_Env\DBFileName
print 'Environment Variable Fairmont_WINS\Fairmont_WINS_Env\DBFileName'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'Fairmont_WINS_Env' 
			And ev.name = N'DBFileName' 
			And f.name = N'Fairmont_WINS') 
 begin
  print ' - Dropping Environment Variable Fairmont_WINS\Fairmont_WINS_Env\DBFileName'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'DBFileName' 
  , @environment_name=N'Fairmont_WINS_Env' 
  , @folder_name=N'Fairmont_WINS' 
  print ' - Environment Variable Fairmont_WINS\Fairmont_WINS_Env\DBFileName dropped'
 end
print ' - Creating Environment Variable Fairmont_WINS\Fairmont_WINS_Env\DBFileName'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'DBFileName'
    , @sensitive = 0
    , @environment_name = N'Fairmont_WINS_Env'
    , @folder_name = N'Fairmont_WINS'
    , @value = @DBFileName
    , @data_type = N'String'

print ' - Environment Variable Fairmont_WINS\Fairmont_WINS_Env\DBFileName created'


-- Environment Variable Fairmont_WINS\Fairmont_WINS_Env\CS_Reporting
print 'Environment Variable Fairmont_WINS\Fairmont_WINS_Env\CS_Reporting'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'Fairmont_WINS_Env' 
			And ev.name = N'CS_Reporting' 
			And f.name = N'Fairmont_WINS') 
 begin
  print ' - Dropping Environment Variable Fairmont_WINS\Fairmont_WINS_Env\CS_Reporting'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'CS_Reporting' 
  , @environment_name=N'Fairmont_WINS_Env' 
  , @folder_name=N'Fairmont_WINS' 
  print ' - Environment Variable Fairmont_WINS\Fairmont_WINS_Env\CS_Reporting dropped'
 end
print ' - Creating Environment Variable Fairmont_WINS\Fairmont_WINS_Env\CS_Reporting'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'CS_Reporting'
    , @sensitive = 0
    , @environment_name = N'Fairmont_WINS_Env'
    , @folder_name = N'Fairmont_WINS'
    , @value = @CS_Reporting
    , @data_type = N'String'

print ' - Environment Variable Fairmont_WINS\Fairmont_WINS_Env\CS_Reporting created'



-- Environment Variable Fairmont_WINS\Fairmont_WINS_Env\SMTPRelay
print 'Environment Variable Fairmont_WINS\Fairmont_WINS_Env\SMTPRelay'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'Fairmont_WINS_Env' 
			And ev.name = N'SMTPRelay' 
			And f.name = N'Fairmont_WINS') 
 begin
  print ' - Dropping Environment Variable Fairmont_WINS\Fairmont_WINS_Env\SMTPRelay'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'SMTPRelay' 
  , @environment_name=N'Fairmont_WINS_Env' 
  , @folder_name=N'Fairmont_WINS' 
  print ' - Environment Variable Fairmont_WINS\Fairmont_WINS_Env\SMTPRelay dropped'
 end
print ' - Creating Environment Variable Fairmont_WINS\Fairmont_WINS_Env\SMTPRelay'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'SMTPRelay'
    , @sensitive = 0
    , @environment_name = N'Fairmont_WINS_Env'
    , @folder_name = N'Fairmont_WINS'
    , @value = @SMTPRelay
    , @data_type = N'String'

print ' - Environment Variable Fairmont_WINS\Fairmont_WINS_Env\SMTPRelay created'


-- Environment Variable Fairmont_WINS\Fairmont_WINS_Env\Email_Distribution_List
print 'Environment Variable Fairmont_WINS\Fairmont_WINS_Env\Email_Distribution_List'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'Fairmont_WINS_Env' 
			And ev.name = N'Email_Distribution_List' 
			And f.name = N'Fairmont_WINS') 
 begin
  print ' - Dropping Environment Variable Fairmont_WINS\Fairmont_WINS_Env\Email_Distribution_List'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'Email_Distribution_List' 
  , @environment_name=N'Fairmont_WINS_Env' 
  , @folder_name=N'Fairmont_WINS' 
  print ' - Environment Variable Fairmont_WINS\Fairmont_WINS_Env\Email_Distribution_List dropped'
 end
print ' - Creating Environment Variable Fairmont_WINS\Fairmont_WINS_Env\Email_Distribution_List'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'Email_Distribution_List'
    , @sensitive = 0
    , @environment_name = N'Fairmont_WINS_Env'
    , @folder_name = N'Fairmont_WINS'
    , @value = @Email_Distribution_List
    , @data_type = N'String'

print ' - Environment Variable Fairmont_WINS\Fairmont_WINS_Env\Email_Distribution_List created'

