USE [msdb]
GO

/****** Object:  Job [CFIGLFiles]    Script Date: 2/8/2018 3:21:25 PM ******/
IF EXISTS (SELECT name FROM msdb.dbo.sysjobs_view WHERE name=N'OPSDW AG Table Refresh')
EXEC msdb.dbo.sp_delete_job @job_name=N'OPSDW AG Table Refresh', @delete_unused_schedule=1
GO

/****** Object:  Job [OPSDW AG Table Refresh]    Script Date: 2/23/2018 1:30:17 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 2/23/2018 1:30:17 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'OPSDW AG Table Refresh', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'TRG\jgabe', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [RUN SSIS Table Refresh]    Script Date: 2/23/2018 1:30:17 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'RUN SSIS Table Refresh', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'SSIS', 
		@command=N'/ISSERVER "\"\SSISDB\OPSDW_AG_Refresh\OPSDW_AG_Refresh\OPSDW_ag_Refresh.dtsx\"" /SERVER BIQAETL01\BIUATETL01 /Par "\"$Project::ConAG_CS\"";"\"Data Source=SQLTEST2012R2;Initial Catalog=AuthorityGuidelines;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;\"" /Par "\"$Project::conOPSDWPS_CS\"";"\"Data Source=OPSDW;Initial Catalog=ProductionSupport;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E', 
		@database_name=N'master', 
		@flags=0
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


