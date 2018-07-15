

DECLARE @ReturnCode INT=0
		, @folder_id bigint
		,@ref_id bigint

/*****************************************************
    Variable declarations, DBA  changes here per environment
*****************************************************/
DECLARE   @project sysname = 'Fairmont_WINS' --Deployed project
		, @folder sysname = 'Fairmont_WINS' --Deployed folder
        , @env sysname = 'ENV_Fairmont_WINS' --Environment to create
		--Environment variables to deploy and map to project parameters
        , @EV_Email_Distribution_List sysname= N'jroch@trg.com'
        , @EV_SMTP_Connection sysname= N'SmtpServer=manrelay01.trg.com;UseWindowsAuthentication=False;EnableSsl=False;Timeout=2000;'
        , @EV_WINS_Destination_ServerName sysname= N'MANDEVDATA02'
        , @EV_WINS_Source_dBFileName sysname= N'"\\mansan02\batchdev$\Fairfax\FM_DIRECT_and_CEDED_LOSSES_MONTHLY.mdb"'
;
/* Start the transaction */
BEGIN TRANSACTION

--Make sure the project is deployed
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[projects] WHERE name = @project)
    BEGIN
        RAISERROR('Please deploy %s to an identically named folder before running this script', 10, 1, @folder) WITH NOWAIT;
         GOTO QuitWithRollback;
    END


--Create folder if necessary
    IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[folders] WHERE name = @folder)
    BEGIN
        RAISERROR('Creating folder: %s ...', 10, 1, @folder) WITH NOWAIT;
        EXEC @ReturnCode = [SSISDB].[catalog].[create_folder] @folder_name=@folder, @folder_id=@folder_id OUTPUT
        IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;
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

--Create the variables
--EV_Email_Distribution_List
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @env_id AND name = N'EV_Email_Distribution_List')
    BEGIN
	RAISERROR('Creating variable: EV_Email_Distribution_List ...', 10, 1) WITH NOWAIT;
    EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_variable]
        @variable_name=N'EV_Email_Distribution_List'
        , @sensitive=0
        , @description=N''
        , @environment_name=@env
        , @folder_name=@folder
        , @value=@EV_Email_Distribution_List
        , @data_type=N'String'
	END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

--EV_SMTP_Connection
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @env_id AND name = N'EV_SMTP_Connection')
     BEGIN
		 RAISERROR('Creating variable: EV_SMTP_Connection ...', 10, 1) WITH NOWAIT;
		 EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_variable]
			@variable_name=N'EV_SMTP_Connection'
			, @sensitive=0
			, @description=N''
			, @environment_name=@env
			, @folder_name=@folder
			, @value=@EV_SMTP_Connection
			, @data_type=N'String'
		END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

--EV_WINS_Destination_ServerName
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @env_id AND name = N'EV_WINS_Destination_ServerName')
	BEGIN
		RAISERROR('Creating variable: EV_WINS_Destination_ServerName ...', 10, 1) WITH NOWAIT;
		EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_variable]
			@variable_name=N'EV_WINS_Destination_ServerName'
			, @sensitive=0
			, @description=N''
			, @environment_name=@env
			, @folder_name=@folder
			, @value=@EV_WINS_Destination_ServerName
			, @data_type=N'String'
	END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;
--EV_WINS_Source_dBFileName
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environment_variables] WHERE environment_id = @env_id AND name = N'EV_WINS_Source_dBFileName')
	BEGIN
		RAISERROR('Creating variable: EV_WINS_Source_dBFileName ...', 10, 1) WITH NOWAIT;
		EXEC @ReturnCode = [SSISDB].[catalog].[create_environment_variable]
			@variable_name=N'EV_WINS_Source_dBFileName'
			, @sensitive=0
			, @description=N'Path to Access dB File'
			, @environment_name=@env
			, @folder_name=@folder
			, @value=@EV_WINS_Source_dBFileName
			, @data_type=N'String'
	END
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

--Create environment reference
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

----Create environment variable reference(s)
----All vars should be  project level(object_type=20)

----EV_Email_Distribution_List

RAISERROR('An environment variable reference to EV_Email_Distribution_List is being created.' , 0, 1) WITH NOWAIT;

 EXEC @ReturnCode = [SSISDB].[catalog].[set_object_parameter_value]
		@object_type = 20,
		@parameter_name = N'Email_Distribution_List',
		@folder_name = @folder,
		@project_name = @project,
		@parameter_value = 'EV_Email_Distribution_List',
		@value_type = 'R'

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

----EV_WINS_Destination_ServerName

RAISERROR('An environment variable reference to EV_WINS_Destination_ServerName is being created.' , 0, 1) WITH NOWAIT;

 EXEC @ReturnCode = [SSISDB].[catalog].[set_object_parameter_value]
		@object_type = 20,
		@parameter_name = N'WINS_Destination_ServerName',
		@folder_name = @folder,
		@project_name = @project,
		@parameter_value = 'EV_WINS_Destination_ServerName',
		@value_type = 'R'

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

----EV_SMTP_Connection

RAISERROR('An environment variable reference to EV_SMTP_Connection is being created.' , 0, 1) WITH NOWAIT;

 EXEC @ReturnCode = [SSISDB].[catalog].[set_object_parameter_value]
		@object_type = 20,
		@parameter_name = N'SMTP_Connection',
		@folder_name = @folder,
		@project_name = @project,
		@parameter_value = 'EV_SMTP_Connection',
		@value_type = 'R'

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

----EV_WINS_Source_dBFileName

RAISERROR('An environment variable reference to EV_WINS_Source_dBFileName is being created.' , 0, 1) WITH NOWAIT;

 EXEC @ReturnCode = [SSISDB].[catalog].[set_object_parameter_value]
		@object_type = 20,
		@parameter_name = N'WINS_Source_dBFileName',
		@folder_name = @folder,
		@project_name = @project,
		@parameter_value = 'EV_WINS_Source_dBFileName',
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


