-------------------------------------------------------------------------------------------------------
--Script to create SSIS environment and configure it to SSIS Solution for UpdateCloseCompleteDate DB
--This scripts needs can be run on any of the (DEV/QA/Prod) ETL Server.
-------------------------------------------------------------------------------------------------------
USE SSISDB
GO


DECLARE	@Folder_Id bigint,
		@Environment_Id bigint,
		@DBServerName nvarchar(25),
		@UserName sql_variant


Select @DBServerName = CASE	WHEN @@ServerName = 'BIDEVETL01' THEN 'DWDEV'
							WHEN @@ServerName = 'BIETL01'    THEN 'DWPROD'
						END

--Select @@Servername , @DBServerName



--Create Folder to add SSIS Project
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[folders] WHERE name = N'SSIS_UpdateCloseCompleteDate')
    EXEC [SSISDB].[catalog].[create_folder] @folder_name=N'SSIS_UpdateCloseCompleteDate', @folder_id=@folder_id OUTPUT
ELSE
    SET @folder_id = (SELECT folder_id FROM [SSISDB].[catalog].[folders] WHERE name = N'SSIS_UpdateCloseCompleteDate')


--Create SSIS Environment
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environments] WHERE folder_id = @folder_id AND name = N'Env_UpdateCloseCompleteDate')
    EXEC [SSISDB].[catalog].[create_environment] @environment_name=N'Env_UpdateCloseCompleteDate', @folder_name=N'SSIS_UpdateCloseCompleteDate'
ELSE
	SET @environment_id = (SELECT environment_id FROM [SSISDB].[catalog].[environments] WHERE folder_id = @folder_id and name = N'Env_UpdateCloseCompleteDate')


--Declare variable for setting SSIS variable values
DECLARE @var sql_variant


-----------------------------------------------------------------------------------------------------------------------------------------------------
--DB Connections
--SQL_RS_ODS_Con
-----------------------------------------------------------------------------------------------------------------------------------------------------


--Env Variable 'SQL_RSODS_Con'

SET @var = N'Data Source='+@DBServerName+';Initial Catalog=RS_ODS;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'SQL_RSODS_Con')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'SQL_RSODS_Con', @sensitive=0, @description=N'DB connection for RS_ODS', @environment_name=N'Env_UpdateCloseCompleteDate', @folder_name=N'SSIS_UpdateCloseCompleteDate', @value=@var, @data_type=N'String'


-----------------------------------------------------------------------------------------------------------------------------------------------------
--SMTP & Email Connection
-----------------------------------------------------------------------------------------------------------------------------------------------------


--Env Variable 'SMTP_Connection'
SET @var = N'SmtpServer=manrelay01.trg.com;UseWindowsAuthentication=False;EnableSsl=False;Timeout=2000;'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'SMTPConnection')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'SMTPConnection', @sensitive=0, @description=N'SMTP Connection to send emails', @environment_name=N'Env_UpdateCloseCompleteDate', @folder_name=N'SSIS_UpdateCloseCompleteDate', @value=@var, @data_type=N'String'



--Environment Variable 'Email_Distribution_List'
SET @var = CASE	WHEN @@ServerName = 'BIDEVETL01' THEN N'Puneet_Kumar@trg.com; Patricia_Williams@trg.com'
				WHEN @@ServerName = 'BIETL01'    THEN N'ITProductionSupport@trg.com'
		   END
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Email_List')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Email_List', @sensitive=0, @description=N'Production Support Email list to send Notifications', @environment_name=N'Env_UpdateCloseCompleteDate', @folder_name=N'SSIS_UpdateCloseCompleteDate', @value=@var, @data_type=N'String'


-----------------------------------------------------------------------------------------------------------------------------------------------------
--ServerNames
-----------------------------------------------------------------------------------------------------------------------------------------------------


--Environment Variable 'ServerName'
SET @var = N'ServerName'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'ServerName')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'ServerName', @sensitive=0, @description=N'UpdateCloseCompleteDate Database Server', @environment_name=N'Env_UpdateCloseCompleteDate', @folder_name=N'SSIS_UpdateCloseCompleteDate', @value=@DBServerName, @data_type=N'String'













---------------------------------------------------------------------------------------------------------------------------------------------
--Script to map SSIS environment for DeleteDataHavenDocs
--This scripts needs to run on the ETL Server BIETL01
---------------------------------------------------------------------------------------------------------------------------------------------

USE SSISDB
GO


--Create Reference environment to project
Declare @Ref_ID bigint

--Get Reference_ID if reference exists
SELECT @Ref_ID = Cast(Reference_ID as NVarchar(25)) 
FROM SSISDB.[catalog].environment_references er
JOIN SSISDB.[catalog].projects p 
	ON p.project_id = er.project_id
WHERE er.environment_name = 'Env_UpdateCloseCompleteDate'
AND p.name LIKE 'UpdateCloseCompleteDate'


--Check Environment if exists then delete
If EXISTS (
			SELECT Cast(Reference_ID as NVarchar(25)) 
			FROM SSISDB.[catalog].environment_references er
			JOIN SSISDB.[catalog].projects p 
				ON p.project_id = er.project_id
			WHERE er.environment_name = 'Env_UpdateCloseCompleteDate'
			AND p.name LIKE 'UpdateCloseCompleteDate'
			)
	--Select 'Delete Environment Reference'
	EXECUTE [catalog].[delete_environment_reference] @Ref_ID


--Select 'Create Environment Reference'
	EXEC  [SSISDB].[catalog].[create_environment_reference]
	@project_name=N'UpdateCloseCompleteDate'
,   @environment_name=N'Env_UpdateCloseCompleteDate'
,   @reference_id=@Ref_ID OUTPUT
,   @environment_folder_name=N'SSIS_UpdateCloseCompleteDate'
,   @folder_name=N'SSIS_UpdateCloseCompleteDate'
,   @reference_type=A



--Get reference values to point variables

DECLARE @Reference_ID01 nvarchar(25),@PName nvarchar(200)
DECLARE @ReturnCode INT,@Command1 nvarchar(500),@ObjectName nvarchar(100),@ObjectType int,@FolderName nvarchar(50)

Set @FolderName = 'SSIS_UpdateCloseCompleteDate'

SELECT  @pname=p.name, @REFERENCE_ID01 = cast(reference_id as nvarchar(25)) 
FROM SSISDB.[catalog].environment_references er
JOIN SSISDB.[catalog].projects p ON p.project_id = er.project_id
WHERE er.environment_name = 'Env_UpdateCloseCompleteDate'
AND p.name LIKE 'UpdateCloseCompleteDate'


SELECT DISTINCT @objectName=[object_name],@objectType=object_type 
FROM [SSISDB].[internal].[object_parameters] O
Inner Join [SSISDB].[internal].[environment_references] E
	ON O.project_id=E.project_id
Inner Join  SSISDB.[catalog].projects p
	ON p.project_id = e.project_id
WHERE E.[reference_id]=@REFERENCE_ID01 and [object_name] = 'UpdateCloseCompleteDate'

SELECT @REFERENCE_ID01 as ReferenceID, @ObjectName as ObjectName, @ObjectType as ObjectType, @PName AS ProjectName




------------------------------------------------------------------------------------------------
--SMTP Connection
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SMTP_Connection.ConnectionString'
,   @parameter_value=N'SMTPConnection'
,   @object_name=@objectName
,   @value_type=R


------------------------------------------------------------------------------------------------
--EMail_List parameter
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'Email_Distribution_List'
,   @parameter_value=N'Email_List'
,   @object_name=@objectName
,   @value_type=R



------------------------------------------------------------------------------------------------
--SQL_RSReporting_Con Connection
------------------------------------------------------------------------------------------------


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL_RS_ODS_Con.ConnectionString'
,   @parameter_value=N'SQL_RSODS_Con'
,   @object_name=@objectName
,   @value_type=R


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL_RS_ODS_Con.ServerName'
,   @parameter_value=N'ServerName'
,   @object_name=@objectName
,   @value_type=R




