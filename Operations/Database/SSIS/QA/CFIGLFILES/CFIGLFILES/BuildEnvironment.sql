

DECLARE @ReturnCode INT=0
		, @folder_id bigint
		,@ref_id bigint

/*****************************************************
    Variable declarations, DBA  changes here per environment
*****************************************************/
DECLARE   @project sysname = 'CFIGLFiles' --Deployed project
		,@folder sysname = 'CFIGLFiles' --Deployed folder
        ,@env sysname = 'ENV_CFIGLFiles' --Environment to create
		--Environment variables to deploy and map to project parameters
        ,@EV_CFI_DistributionList sysname= N'jroch@trg.com'--1
        ,@EV_SMTPRelay_ServerName sysname= N'manrelay01.trg.com'--2
		,@EV_Excel_Destination_FilePath sysname = '\\mansan02\batchdev$\CFI\CFIGLFiles\CFIExport.xls'--3
		,@EV_CFI_ValuationDate sysname = N'08/31/2017'--4
		,@EV_RS_Reporting_ServerName sysname = 'DWDEV'--5
		,@EV_OutputFlag_CFICaseReserves bit = 'TRUE'--6
		,@EV_OutputFlag_CFIFilesSummary bit = 'TRUE'--7
		,@EV_OutputFlag_CFIPaids bit = 'TRUE'--8
		,@EV_OutputFlag_CFIQuarterSummary bit = 'TRUE'--9
		
		;
/* Start the transaction */
BEGIN TRANSACTION



--Make sure the project is deployed
RAISERROR('Checking project %s  is deployed', 10, 1, @project) WITH NOWAIT;
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[projects] WHERE name = @project)
    BEGIN
        RAISERROR('Please deploy %s  before running this script', 10, 1, @folder) WITH NOWAIT;
         GOTO QuitWithRollback;
    END


----Make sure the folder name specified above is deployed
	RAISERROR('Checking folder %s  is deployed', 10, 1, @folder) WITH NOWAIT;
    IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[folders] WHERE name = @folder)
   BEGIN
       RAISERROR('Nonexistent folder specified in script: %s ...', 10, 1, @folder) WITH NOWAIT;
       GOTO QuitWithRollback;
   END

--Create environment if necessary
    IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environments] WHERE name = @env)
	BEGIN
	RAISERROR('Creating Environment: %s', 10, 1, @env) WITH NOWAIT;
    EXEC @ReturnCode = [SSISDB].[catalog].[create_environment] @folder_name=@folder, @environment_name=@env
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;
	END
--Get the environment ID
DECLARE @env_id bigint
SET @env_id=(SELECT environment_id FROM [SSISDB].[catalog].[environments] WHERE name = @env)
IF (@@ERROR <> 0) GOTO QuitWithRollback;

--Create the Environmant variables if necessary
--EV_CFI_DistributionList 1
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @env_id AND name = 'EV_CFI_DistributionList')
    BEGIN
	RAISERROR('Creating variable: EV_CFI_DistributionList ...', 10, 1) WITH NOWAIT;
    EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_variable]
        @variable_name=N'EV_CFI_DistributionList'
        , @sensitive=0
        , @description=N''
        , @environment_name=@env
        , @folder_name=@folder
        , @value=@EV_CFI_DistributionList
        , @data_type=N'String'
	END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

--EV_SMTPRelay_ServerName 2
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @env_id AND name = N'EV_SMTPRelay_ServerName')
     BEGIN
		 RAISERROR('Creating variable: EV_SMTPRelay_ServerName ...', 10, 1) WITH NOWAIT;
		 EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_variable]
			@variable_name=N'EV_SMTPRelay_ServerName'
			, @sensitive=0
			, @description=N''
			, @environment_name=@env
			, @folder_name=@folder
			, @value=@EV_SMTPRelay_ServerName
			, @data_type=N'String'
		END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

--EV_Excel_Destination_FilePath 3
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @env_id AND name = N'EV_Excel_Destination_FilePath')
	BEGIN
		RAISERROR('Creating variable: EV_WINS_Destination_ServerName ...', 10, 1) WITH NOWAIT;
		EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_variable]
			@variable_name=N'EV_Excel_Destination_FilePath'
			, @sensitive=0
			, @description=N''
			, @environment_name=@env
			, @folder_name=@folder
			, @value=@EV_Excel_Destination_FilePath
			, @data_type=N'String'
	END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;
--EV_CFI_ValuationDate 4
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @env_id AND name = N'EV_CFI_ValuationDate')
	BEGIN
		RAISERROR('Creating variable: EV_WINS_Source_dBFileName ...', 10, 1) WITH NOWAIT;
		EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_variable]
			@variable_name=N'EV_CFI_ValuationDate'
			, @sensitive=0
			, @description=N'Valuation Date'
			, @environment_name=@env
			, @folder_name=@folder
			, @value=@EV_CFI_ValuationDate
			, @data_type=N'DateTime'
	END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

	--EV_RS_Reporting_ServerName 5
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @env_id AND name = N'EV_RS_Reporting_ServerName')
	BEGIN
		RAISERROR('Creating variable: EV_RS_Reporting_ServerName ...', 10, 1) WITH NOWAIT;
		EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_variable]
			@variable_name=N'EV_RS_Reporting_ServerName'
			, @sensitive=0
			, @description=N'SQL Source Server Name'
			, @environment_name=@env
			, @folder_name=@folder
			, @value=@EV_RS_Reporting_ServerName
			, @data_type=N'String'
	END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

	--EV_OutputFlag_CFICaseReserves 6
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @env_id AND name = N'EV_OutputFlag_CFICaseReserves')
	BEGIN
		RAISERROR('Creating variable: EV_OutputFlag_CFICaseReserves ...', 10, 1) WITH NOWAIT;
		EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_variable]
			@variable_name=N'EV_OutputFlag_CFICaseReserves'
			, @sensitive=0
			, @description=N'include or exclude this tab from the excel workbook - default is include'
			, @environment_name=@env
			, @folder_name=@folder
			, @value=@EV_OutputFlag_CFICaseReserves
			, @data_type=N'Boolean'
	END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

	--EV_OutputFlag_CFIFilesSummary 7
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @env_id AND name = N'EV_OutputFlag_CFIFilesSummary')
	BEGIN
		RAISERROR('Creating variable: EV_OutputFlag_CFIFilesSummary ...', 10, 1) WITH NOWAIT;
		EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_variable]
			@variable_name=N'EV_OutputFlag_CFIFilesSummary'
			, @sensitive=0
			, @description=N'include or exclude this tab from the excel workbook - default is include'
			, @environment_name=@env
			, @folder_name=@folder
			, @value=@EV_OutputFlag_CFIFilesSummary
			, @data_type=N'Boolean'
	END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

		--EV_OutputFlag_CFIPaids 8
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @env_id AND name = N'EV_OutputFlag_CFIPaids')
	BEGIN
		RAISERROR('Creating variable: EV_OutputFlag_CFIPaids ...', 10, 1) WITH NOWAIT;
		EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_variable]
			@variable_name=N'EV_OutputFlag_CFIPaids'
			, @sensitive=0
			, @description=N'include or exclude this tab from the excel workbook - default is include'
			, @environment_name=@env
			, @folder_name=@folder
			, @value=@EV_OutputFlag_CFIPaids
			, @data_type=N'Boolean'
	END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

			--EV_OutputFlag_CFIQuarterSummary 9
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @env_id AND name = N'EV_OutputFlag_CFIQuarterSummary')
	BEGIN
		RAISERROR('Creating variable: EV_OutputFlag_CFIQuarterSummary ...', 10, 1) WITH NOWAIT;
		EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_variable]
			@variable_name=N'EV_OutputFlag_CFIQuarterSummary'
			, @sensitive=0
			, @description=N'include or exclude this tab from the excel workbook - default is include'
			, @environment_name=@env
			, @folder_name=@folder
			, @value=@EV_OutputFlag_CFIQuarterSummary
			, @data_type=N'Boolean'
	END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

--Create environment references

   IF NOT EXISTS( SELECT 1
        FROM [SSISDB].[catalog].[folders] f
        JOIN [SSISDB].[catalog].[environments] e ON e.folder_id = f.folder_id
        JOIN [SSISDB].[catalog].[projects] p ON p.folder_id = f.folder_id
        JOIN [SSISDB].[catalog].[environment_references] r ON r.environment_name = e.name
																AND p.project_id = r.project_id
        WHERE f.name = @folder AND e.name = @env AND p.name = @project
        )
    BEGIN
        RAISERROR('An environment reference for project %s is being created.' , 0, 1,@project) WITH NOWAIT;

        EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_reference]
            @environment_name=@env, @reference_id=@ref_id OUTPUT,
            @project_name=@project, @folder_name=@folder, @reference_type='R';
        
    END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

--Create environment variable reference(s)
--If a reference exists, it will be overwritten 
--All vars should be  project level(object_type=20)

----EV_CFI_DistributionList 1

RAISERROR('An environment variable reference to EV_CFI_DistributionList is being created or updated.' , 0, 1) WITH NOWAIT;

 EXEC @ReturnCode = [SSISDB].[catalog].[set_object_parameter_value]
		@object_type = 20,
		@parameter_name = N'CFI_DistributionList',
		@folder_name = @folder,
		@project_name = @project,
		@parameter_value = 'EV_CFI_DistributionList',
		@value_type = 'R'

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

----EV_SMTPRelay_ServerName 2

RAISERROR('An environment variable reference to EV_SMTPRelay_ServerName is being created or updated.' , 0, 1) WITH NOWAIT;

 EXEC @ReturnCode = [SSISDB].[catalog].[set_object_parameter_value]
		@object_type = 20,
		@parameter_name = N'SMTPRelay_ServerName',
		@folder_name = @folder,
		@project_name = @project,
		@parameter_value = 'EV_SMTPRelay_ServerName',
		@value_type = 'R'

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

----EV_Excel_Destination_FilePath 3

RAISERROR('An environment variable reference to EV_Excel_Destination_FilePath is being created or updated.' , 0, 1) WITH NOWAIT;

 EXEC @ReturnCode = [SSISDB].[catalog].[set_object_parameter_value]
		@object_type = 20,
		@parameter_name = N'Excel_Destination_FilePath',
		@folder_name = @folder,
		@project_name = @project,
		@parameter_value = 'EV_Excel_Destination_FilePath',
		@value_type = 'R'

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

----EV_CFI_ValuationDate 4

RAISERROR('An environment variable reference to EV_CFI_ValuationDate is being created or updated.' , 0, 1) WITH NOWAIT;

 EXEC @ReturnCode = [SSISDB].[catalog].[set_object_parameter_value]
		@object_type = 20,
		@parameter_name = N'CFI_ValuationDate',
		@folder_name = @folder,
		@project_name = @project,
		@parameter_value = 'EV_CFI_ValuationDate',
		@value_type = 'R'

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

----EV_RS_Reporting_ServerName 5

RAISERROR('An environment variable reference to EV_RS_Reporting_ServerName is being created or updated.' , 0, 1) WITH NOWAIT;

 EXEC @ReturnCode = [SSISDB].[catalog].[set_object_parameter_value]
		@object_type = 20,
		@parameter_name = N'RS_Reporting_ServerName',
		@folder_name = @folder,
		@project_name = @project,
		@parameter_value = 'EV_RS_Reporting_ServerName',
		@value_type = 'R'

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

----EV_OutputFlag_CFICaseReserves 6

RAISERROR('An environment variable reference to EV_OutputFlag_CFICaseReserves is being created or updated.' , 0, 1) WITH NOWAIT;

 EXEC @ReturnCode = [SSISDB].[catalog].[set_object_parameter_value]
		@object_type = 20,
		@parameter_name = N'OutputFlag_CFICaseReserves',
		@folder_name = @folder,
		@project_name = @project,
		@parameter_value = 'EV_OutputFlag_CFICaseReserves',
		@value_type = 'R'

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

----EV_OutputFlag_CFIFilesSummary 7

RAISERROR('An environment variable reference to EV_OutputFlag_CFIFilesSummary is being created or updated.' , 0, 1) WITH NOWAIT;

 EXEC @ReturnCode = [SSISDB].[catalog].[set_object_parameter_value]
		@object_type = 20,
		@parameter_name = N'OutputFlag_CFIFilesSummary',
		@folder_name = @folder,
		@project_name = @project,
		@parameter_value = 'EV_OutputFlag_CFIFilesSummary',
		@value_type = 'R'

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

----EV_OutputFlag_CFIPaids 8

RAISERROR('An environment variable reference to EV_OutputFlag_CFIPaids is being created or updated.' , 0, 1) WITH NOWAIT;

 EXEC @ReturnCode = [SSISDB].[catalog].[set_object_parameter_value]
		@object_type = 20,
		@parameter_name = N'OutputFlag_CFIPaids',
		@folder_name = @folder,
		@project_name = @project,
		@parameter_value = 'EV_OutputFlag_CFIPaids',
		@value_type = 'R'

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

----EV_OutputFlag_CFIQuarterSummary 9

RAISERROR('An environment variable reference to EV_OutputFlag_CFIQuarterSummary is being created or updated.' , 0, 1) WITH NOWAIT;

 EXEC @ReturnCode = [SSISDB].[catalog].[set_object_parameter_value]
		@object_type = 20,
		@parameter_name = N'OutputFlag_CFIQuarterSummary',
		@folder_name = @folder,
		@project_name = @project,
		@parameter_value = 'EV_OutputFlag_CFIQuarterSummary',
		@value_type = 'R'

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;




COMMIT TRANSACTION
RAISERROR(N'Script Complete', 10, 1) WITH NOWAIT;
GOTO EndSave

QuitWithRollback:
IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
RAISERROR(N'Fatal Script Error', 16,1) WITH NOWAIT;

EndSave:
GO


