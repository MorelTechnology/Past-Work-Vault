USE SSISDB
GO

DECLARE 
    @folder_id bigint,
    @environment_id bigint,
	@ServerName nvarchar(25),
	@Server nvarchar(25)

--Set Server Names
Select @Server = @@Servername

If @@Servername = 'BIDEVETL01' Select @Servername = 'BIDEVDM01'
If @@Servername = 'BIQAETL01' Select @Servername = 'BIQADM01'
If @@Servername = 'BIETL01' Select @Servername = 'BIDM01'
--Select @@Servername , @Servername


----Create Folder to add SSIS Project
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[folders] WHERE name = N'SSIS_Acuity_Data_Load')
    EXEC [SSISDB].[catalog].[create_folder] @folder_name=N'SSIS_Acuity_Data_Load', @folder_id=@folder_id OUTPUT
ELSE
    SET @folder_id = (SELECT folder_id FROM [SSISDB].[catalog].[folders] WHERE name = N'SSIS_Acuity_Data_Load')


--Create SSIS Environment
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environments] WHERE folder_id = @folder_id AND name = N'QA_Env_Acuity')
    EXEC [SSISDB].[catalog].[create_environment] @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load'
ELSE
	SET @environment_id = (SELECT environment_id FROM [SSISDB].[catalog].[environments] WHERE folder_id = @folder_id and name = N'QA_Env_Acuity')



    SET @folder_id = (SELECT folder_id FROM [SSISDB].[catalog].[folders] WHERE name = N'SSIS_Acuity_Data_Load')
	SET @environment_id = (SELECT environment_id FROM [SSISDB].[catalog].[environments] WHERE folder_id = @folder_id and name = N'QA_Env_Acuity')

--Select @folder_id, @environment_id


DECLARE @var sql_variant



--Env Variable 'SMTP Connection'
SET @var = N'SmtpServer=manrelay01.trg.com;UseWindowsAuthentication=False;EnableSsl=False;Timeout=2000;'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'SMTP Connection')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'SMTP Connection', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'


--Env Variable 'SQL Log Connection'
SET @var = N'Data Source='+@Servername+';Initial Catalog=RSETLLog;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'SQL Log Connection')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'SQL Log Connection', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'


--Env Variable 'SQL Src Connection'
SET @var = N'Data Source='+@Servername+';Initial Catalog=ACP_STG;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'SQL Src Connection')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'SQL Src Connection', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'


--Env Variable 'SQL Trg Connection'
SET @var = N'Data Source='+@Servername+';Initial Catalog=Work_AcuityStg;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'SQL Trg Connection')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'SQL Trg Connection', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'



------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--Environment Variable 'ServerName'
SET @var = N'ServerName'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'ServerName')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'ServerName', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@Servername, @data_type=N'String'



--Environment Variable 'UserName'
SET @var = N'TRG\BIDPQA_svc'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'UserName')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'UserName', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'


--Environment Variable 'Password'
SET @var = N'BID@taPr0jQa'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Password')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Password', @sensitive=1, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'


--Environment Variable 'Email_List'
SET @var = N'AcuityProductionSupport@trg.com'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Email_List')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Email_List', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'



--Environment Variable 'SourceDirectory'
SET @var = N'\\mansan02\batchqa$\Acuity\DataDumpIn\'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'SourceDirectory')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'SourceDirectory', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'


--Environment Variable 'ArchiveDirectory'
SET @var = N'\\mansan02\batchqa$\Acuity\DataDumpIn\Archive'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'ArchiveDirectory')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'ArchiveDirectory', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'


--Environment Variable 'ErrorDirectory'
SET @var = N'\\mansan02\batchqa$\Acuity\DataDumpIn\DataDumpError\'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'ErrorDirectory')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'ErrorDirectory', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'


--Environment Variable 'FileName'
SET @var = N'Claims_Budget.out'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'FileName')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'FileName', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'


--Environment Variable 'Masstort_Budgets'
SET @var = N'Masstort_Budgets.out'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Masstort_Budgets')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Masstort_Budgets', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'


--Environment Variable 'Masstort_Invoice_Details'
SET @var = N'Masstort_Invoice_Details.out'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Masstort_Invoice_Details')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Masstort_Invoice_Details', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'




--Environment Variable 'Masstort_Invoices'
SET @var = N'Masstort_Invoices.out'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Masstort_Invoices')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Masstort_Invoices', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'



--Environment Variable 'Masstort_Matters'
SET @var = N'Masstort_Matters.out'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Masstort_Matters')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Masstort_Matters', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'



--Environment Variable 'Masstort_Utilities'
SET @var = N'Masstort_Utilities.out'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Masstort_Utilities')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Masstort_Utilities', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'



--Environment Variable 'Claims_Budgets'
SET @var = N'Claims_Budget.out'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Claims_Budget')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Claims_Budget', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'


--Environment Variable 'Claims_Invoice_Details'
SET @var = N'Claims_Invoice_Details.out'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Claims_Invoice_Details')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Claims_Invoice_Details', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'




--Environment Variable 'Claims_Invoices'
SET @var = N'Claims_Invoices.out'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Claims_Invoices')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Claims_Invoices', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'



--Environment Variable 'Claims_Matters'
SET @var = N'Claims_Matter.out'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Claims_Matter')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Claims_Matter', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'



--Environment Variable 'Claims_Utilities'
SET @var = N'Claims_Utilities.out'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Claims_Utilities')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Claims_Utilities', @sensitive=0, @description=N'', @environment_name=N'QA_Env_Acuity', @folder_name=N'SSIS_Acuity_Data_Load', @value=@var, @data_type=N'String'



------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

