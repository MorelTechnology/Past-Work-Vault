USE SSISDB
GO


--Create Reference environment to project
Declare @Ref_ID bigint

---Get Reference_ID if reference exists
SELECT @Ref_ID = Cast(Reference_ID as NVarchar(25)) 
FROM SSISDB.[catalog].environment_references er
JOIN SSISDB.[catalog].projects p ON p.project_id = er.project_id
WHERE er.environment_name = 'Prod_Env_Acuity'
AND p.name LIKE 'SSIS_Acuity_Data_Load'


--Check Environment if exists then delete
If EXISTS (
			SELECT Cast(Reference_ID as NVarchar(25)) 
			FROM SSISDB.[catalog].environment_references er
			JOIN SSISDB.[catalog].projects p ON p.project_id = er.project_id
			WHERE er.environment_name = 'Prod_Env_Acuity'
			AND p.name LIKE 'SSIS_Acuity_Data_Load'
			)
	Select 'Delete Environment Reference'
	EXECUTE [catalog].[delete_environment_reference] @Ref_ID


Select 'Create Environment Reference'
	EXEC  [SSISDB].[catalog].[create_environment_reference]
	@project_name=N'SSIS_Acuity_Data_Load'
,   @environment_name=N'Prod_Env_Acuity'
,   @reference_id=@Ref_ID OUTPUT
,   @environment_folder_name=N'SSIS_Acuity_Data_Load'
,   @folder_name=N'SSIS_Acuity_Data_Load'
,   @reference_type=A

----------------------------------remove existing and recreate-----------
---------------------------------------------------------------------------------------------------------------------------



--Get reference values to point variables

DECLARE @Reference_ID nvarchar(25),@PName nvarchar(200)
DECLARE @ReturnCode INT,@Command1 nvarchar(500),@ObjectName nvarchar(100),@ObjectType int,@FolderName nvarchar(50)

Set @FolderName = 'SSIS_Acuity_Data_Load'

SELECT  @pname=p.name, @REFERENCE_ID = cast(reference_id as nvarchar(25)) 
FROM SSISDB.[catalog].environment_references er
JOIN SSISDB.[catalog].projects p ON p.project_id = er.project_id
WHERE er.environment_name = 'Prod_Env_Acuity'
AND p.name LIKE 'SSIS_Acuity_Data_Load'


SELECT distinct @objectName=[object_name],@objectType=object_type 
FROM [SSISDB].[internal].[object_parameters] O
Inner Join [SSISDB].[internal].[environment_references] E
	On O.project_id=E.project_id
Inner Join  SSISDB.[catalog].projects p
	On p.project_id = e.project_id
Where E.[reference_id]=@REFERENCE_ID and [object_name] like '%SSIS_Acuity_Data_Load%'

Select @REFERENCE_ID as ReferenceID,@ObjectName as ObjectName,@ObjectType as ObjectType,@PName



------------------------------------------------------------------------------------------------
--FileName
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'FileName'
,   @parameter_value=N'FileName'
,   @object_name=@objectName
,   @value_type=R


------------------------------------------------------------------------------------------------
--Claims_Budgets 
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'Claims_Budget'
,   @parameter_value=N'Claims_Budget'
,   @object_name=@objectName
,   @value_type=R



------------------------------------------------------------------------------------------------
--Claims_Invoice_Details
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'Claims_Invoice_Details'
,   @parameter_value=N'Claims_Invoice_Details'
,   @object_name=@objectName
,   @value_type=R


------------------------------------------------------------------------------------------------
--Claims_Invoices
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'Claims_Invoices'
,   @parameter_value=N'Claims_Invoices'
,   @object_name=@objectName
,   @value_type=R


------------------------------------------------------------------------------------------------
--Claims_Matters
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'Claims_Matter'
,   @parameter_value=N'Claims_Matter'
,   @object_name=@objectName
,   @value_type=R



------------------------------------------------------------------------------------------------
--Claims_Utilities
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'Claims_Utilities'
,   @parameter_value=N'Claims_Utilities'
,   @object_name=@objectName
,   @value_type=R




------------------------------------------------------------------------------------------------
--Masstort_Budgets 
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'Masstort_Budgets'
,   @parameter_value=N'Masstort_Budgets'
,   @object_name=@objectName
,   @value_type=R

------------------------------------------------------------------------------------------------
--Masstort_Invoice_Details
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'Masstort_Invoice_Details'
,   @parameter_value=N'Masstort_Invoice_Details'
,   @object_name=@objectName
,   @value_type=R


------------------------------------------------------------------------------------------------
--Masstort_Invoices
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'Masstort_Invoices'
,   @parameter_value=N'Masstort_Invoices'
,   @object_name=@objectName
,   @value_type=R


------------------------------------------------------------------------------------------------
--Masstort_Matters
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'Masstort_Matters'
,   @parameter_value=N'Masstort_Matters'
,   @object_name=@objectName
,   @value_type=R



------------------------------------------------------------------------------------------------
--Masstort_Utilities
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'Masstort_Utilities'
,   @parameter_value=N'Masstort_Utilities'
,   @object_name=@objectName
,   @value_type=R



----------------------------------------------------------------------------------------------
---Error Directory
----------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'ErrorDirectory'
,   @parameter_value=N'ErrorDirectory'
,   @object_name=@objectName
,   @value_type=R


------------------------------------------------------------------------------------------------
---Archive Directory
----------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'ArchiveDirectory'
,   @parameter_value=N'ArchiveDirectory'
,   @object_name=@objectName
,   @value_type=R



----------------------------------------------------------------------------------------------
----SourceDirectory
----------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'SourceDirectory'
,   @parameter_value=N'SourceDirectory'
,   @object_name=@objectName
,   @value_type=R



------------------------------------------------------------------------------------------------
--SMTP Connection
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SMTP Send Mail.ConnectionString'
,   @parameter_value=N'SMTP Connection'
,   @object_name=@objectName
,   @value_type=R


------------------------------------------------------------------------------------------------
--EMail _List Connection
------------------------------------------------------------------------------------------------

EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'Email_Distribution_List'
,   @parameter_value=N'Email_List'
,   @object_name=@objectName
,   @value_type=R


----------------------------------------------------------------------------------------------
---SQL Log Connection
----------------------------------------------------------------------------------------------


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL Log Connection.ConnectionString'
,   @parameter_value=N'SQL Log Connection'
,   @object_name=@objectName
,   @value_type=R


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL Log Connection.ServerName'
,   @parameter_value=N'ServerName'
,   @object_name=@objectName
,   @value_type=R


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL Log Connection.UserName'
,   @parameter_value=N'UserName'
,   @object_name=@objectName
,   @value_type=R


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL Log Connection.Password'
,   @parameter_value=N'Password'
,   @object_name=@objectName
,   @value_type=R



----------------------------------------------------------------------------------------------
---SQL Src Connection
----------------------------------------------------------------------------------------------



EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL Src Connection.ConnectionString'
,   @parameter_value=N'SQL Src Connection'
,   @object_name=@objectName
,   @value_type=R


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL Src Connection.ServerName'
,   @parameter_value=N'ServerName'
,   @object_name=@objectName
,   @value_type=R


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL Src Connection.UserName'
,   @parameter_value=N'UserName'
,   @object_name=@objectName
,   @value_type=R


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL Src Connection.Password'
,   @parameter_value=N'Password'
,   @object_name=@objectName
,   @value_type=R



----------------------------------------------------------------------------------------------
---SQL Trg Connection
----------------------------------------------------------------------------------------------



EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL Trg Connection.ConnectionString'
,   @parameter_value=N'SQL Trg Connection'
,   @object_name=@objectName
,   @value_type=R


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL Trg Connection.ServerName'
,   @parameter_value=N'ServerName'
,   @object_name=@objectName
,   @value_type=R


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL Trg Connection.UserName'
,   @parameter_value=N'UserName'
,   @object_name=@objectName
,   @value_type=R


EXEC [SSISDB].[catalog].[set_object_parameter_value]
    @object_type=@objectType
,   @folder_name=@folderName
,   @project_name=@pname
,   @parameter_name=N'CM.SQL Trg Connection.Password'
,   @parameter_value=N'Password'
,   @object_name=@objectName
,   @value_type=R


----------------------------------------------------------------------------------------------

