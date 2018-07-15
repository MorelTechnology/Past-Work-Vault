-------------------------------------------------------------------------------------------------------
--Script to create SSIS environment and configure it to SSIS Solution for ClaimsCenterStagingLoad DB
--This scripts needs to run on the ETL Server BIETL01
-------------------------------------------------------------------------------------------------------
USE SSISDB
GO


DECLARE	@Folder_Id bigint,
		@Environment_Id bigint,
		@DBServerName nvarchar(25),
		@UserName sql_variant


--Set Server Name
Select @DBServerName = CASE	WHEN @@ServerName = 'BIDEVETL01' THEN 'DEVSQL01'
							WHEN @@ServerName = 'BIETL01'    THEN 'SQLPRODGW'
					   END

--Select @@Servername , @DBServerName



--Create Folder to add SSIS Project
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[folders] WHERE name = N'SSIS_ClaimCenterStaging')
    EXEC [SSISDB].[catalog].[create_folder] @folder_name=N'SSIS_ClaimCenterStaging', @folder_id=@folder_id OUTPUT
ELSE
    SET @folder_id = (SELECT folder_id FROM [SSISDB].[catalog].[folders] WHERE name = N'SSIS_ClaimCenterStaging')


--Create SSIS Environment
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environments] WHERE folder_id = @folder_id AND name = N'Env_ClaimCenterStaging')
    EXEC [SSISDB].[catalog].[create_environment] @environment_name=N'Env_ClaimCenterStaging', @folder_name=N'SSIS_ClaimCenterStaging'
ELSE
	SET @environment_id = (SELECT environment_id FROM [SSISDB].[catalog].[environments] WHERE folder_id = @folder_id and name = N'Env_ClaimCenterStaging')


--Declare variable for setting SSIS variable values
DECLARE @var sql_variant


-----------------------------------------------------------------------------------------------------------------------------------------------------
--DB Connections
-----------------------------------------------------------------------------------------------------------------------------------------------------


--Env Variable 'SQLClaimCenterStaging_Con'
SET @var = N'Data Source='+@DBServerName+';Initial Catalog=ClaimCenterStaging;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;'
--SELECT @var
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'SQLClaimCenterStaging_Con')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'SQLClaimCenterStaging_Con', @sensitive=0, @description=N'DB connection for ClaimCenterStaging', @environment_name=N'Env_ClaimCenterStaging', @folder_name=N'SSIS_ClaimCenterStaging', @value=@var, @data_type=N'String'



-----------------------------------------------------------------------------------------------------------------------------------------------------
--SMTP & Email Connection
-----------------------------------------------------------------------------------------------------------------------------------------------------


--Env Variable 'SMTP Connection'
SET @var = N'SmtpServer=manrelay01.trg.com;UseWindowsAuthentication=False;EnableSsl=False;Timeout=2000;'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'SMTPConnection')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'SMTPConnection', @sensitive=0, @description=N'SMTP Connection to send emails', @environment_name=N'Env_ClaimCenterStaging', @folder_name=N'SSIS_ClaimCenterStaging', @value=@var, @data_type=N'String'



--Environment Variable 'Email_Distribution_List'
SET @var = CASE	WHEN @@ServerName = 'BIDEVETL01' THEN N'Puneet_Kumar@trg.com; Patricia_Williams@trg.com'
				WHEN @@ServerName = 'BIETL01'    THEN N'ITProductionSupport@trg.com'
		   END
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'Email_List')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'Email_List', @sensitive=0, @description=N'Production Support Email list to send Notifications', @environment_name=N'Env_ClaimCenterStaging', @folder_name=N'SSIS_ClaimCenterStaging', @value=@var, @data_type=N'String'


-----------------------------------------------------------------------------------------------------------------------------------------------------
--ServerName
-----------------------------------------------------------------------------------------------------------------------------------------------------
--Environment Variable 'ServerName'
SET @var = N'ServerName'
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @environment_id AND name = N'ServerName')
	EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'ServerName', @sensitive=0, @description=N'ClaimCenterStaging Database Server', @environment_name=N'Env_ClaimCenterStaging', @folder_name=N'SSIS_ClaimCenterStaging', @value=@DBServerName, @data_type=N'String'
















---------------------------------------------------------------------------------------------------------------------------------------------
--Script to map SSIS environment
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
WHERE er.environment_name = 'Env_ClaimCenterStaging'
AND p.name LIKE 'ClaimCenterStaging_Load'


--Check Environment if exists then delete
If EXISTS (
			SELECT Cast(Reference_ID as NVarchar(25)) 
			FROM SSISDB.[catalog].environment_references er
			JOIN SSISDB.[catalog].projects p 
				ON p.project_id = er.project_id
			WHERE er.environment_name = 'Env_ClaimCenterStaging'
			AND p.name LIKE 'ClaimCenterStaging_Load'
			)
	--Select 'Delete Environment Reference'
	EXECUTE [catalog].[delete_environment_reference] @Ref_ID


--Select 'Create Environment Reference'
	EXEC  [SSISDB].[catalog].[create_environment_reference]
	@project_name=N'ClaimCenterStaging_Load'
,   @environment_name=N'Env_ClaimCenterStaging'
,   @reference_id=@Ref_ID OUTPUT
,   @environment_folder_name=N'SSIS_ClaimCenterStaging'
,   @folder_name=N'SSIS_ClaimCenterStaging'
,   @reference_type=A



--Get reference values to point variables

DECLARE @Reference_ID01 nvarchar(25),@PName nvarchar(200)
DECLARE @ReturnCode INT,@Command1 nvarchar(500),@ObjectName nvarchar(100),@ObjectType int,@FolderName nvarchar(50)

Set @FolderName = 'SSIS_ClaimCenterStaging'

SELECT  @pname=p.name, @REFERENCE_ID01 = cast(reference_id as nvarchar(25)) 
FROM SSISDB.[catalog].environment_references er
JOIN SSISDB.[catalog].projects p ON p.project_id = er.project_id
WHERE er.environment_name = 'Env_ClaimCenterStaging'
AND p.name LIKE 'ClaimCenterStaging_Load'


SELECT distinct @objectName=[object_name],@objectType=object_type 
FROM [SSISDB].[internal].[object_parameters] O
Inner Join [SSISDB].[internal].[environment_references] E
	On O.project_id=E.project_id
Inner Join  SSISDB.[catalog].projects p
	On p.project_id = e.project_id
Where E.[reference_id]=@REFERENCE_ID01 and [object_name] = 'ClaimCenterStaging_Load'

Select @REFERENCE_ID01 as ReferenceID, @ObjectName as ObjectName, @ObjectType as ObjectType, @PName AS ProjectName




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
--SQL ClaimCenterStaging Connection
------------------------------------------------------------------------------------------------


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL_ClaimCenterStaging_Con.ConnectionString'
,   @parameter_value=N'SQLClaimCenterStaging_Con'
,   @object_name=@objectName
,   @value_type=R


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL_ClaimCenterStaging_Con.ServerName'
,   @parameter_value=N'ServerName'
,   @object_name=@objectName
,   @value_type=R



