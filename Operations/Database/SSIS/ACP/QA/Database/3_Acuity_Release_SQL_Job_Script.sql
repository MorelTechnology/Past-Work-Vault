USE [msdb]
GO

--Check Existence of Job & Delete
IF EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'Acuity_Data_Load')
EXEC msdb.dbo.sp_delete_job @job_name=N'Acuity_Data_Load', @delete_unused_schedule=1
GO


BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0


--Get Environment Reference
DECLARE @REFERENCE_ID nvarchar(25),@PName nvarchar(200)
DECLARE @command1 nvarchar(1000)

SELECT  @PName=p.name, @REFERENCE_ID = cast(reference_id as nvarchar(25))
  FROM  SSISDB.[catalog].environment_references er
        JOIN SSISDB.[catalog].projects p ON p.project_id = er.project_id
 WHERE  er.environment_name = 'QA_Env_Acuity'
   AND  p.name LIKE 'SSIS_Acuity_Data_Load'



IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
END


DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'Acuity_Data_Load', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

Set @command1=N'/ISSERVER "\"\SSISDB\SSIS_Acuity_Data_Load\SSIS_Acuity_Data_Load\SSIS_Acuity_STG.dtsx\"" /SERVER '+ @@SERVERNAME + '/ENVREFERENCE '+@REFERENCE_ID +' /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E'
	--Select @command1

EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Run SSIS_Acuity_STG', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'SSIS', 
		@command=@command1,
--		@command=N'/ISSERVER "\"\SSISDB\SSIS_Acuity_Data_Load\SSIS_Acuity_Data_Load\SSIS_Acuity_STG.dtsx\"" /SERVER BIDEVETL01 /ENVREFERENCE 143 /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E', 
		@database_name=N'master', 
		@flags=0, 
		@proxy_name=N'QA_SSIS_PROXY'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

Set @command1=N'/ISSERVER "\"\SSISDB\SSIS_Acuity_Data_Load\SSIS_Acuity_Data_Load\SSIS_Work_AcuityStg.dtsx\"" /SERVER '+ @@SERVERNAME + '/ENVREFERENCE '+@REFERENCE_ID +' /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E'
	--Select @command1

EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Run Work_AcuityStg', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'SSIS', 
		@command=@command1,
--		@command=N'/ISSERVER "\"\SSISDB\SSIS_Acuity_Data_Load\SSIS_Acuity_Data_Load\SSIS_Work_AcuityStg.dtsx\"" /SERVER BIDEVETL01 /ENVREFERENCE 143 /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E', 
		@database_name=N'master', 
		@flags=0, 
		@proxy_name=N'QA_SSIS_PROXY'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO


