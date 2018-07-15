USE [msdb]
GO

/****** Object:  Job [CFIGLFiles]    Script Date: 2/8/2018 3:21:25 PM ******/
IF EXISTS (SELECT name FROM msdb.dbo.sysjobs_view WHERE name=N'CFIGLFILES')
EXEC msdb.dbo.sp_delete_job @job_name=N'CFIGLFILES', @delete_unused_schedule=1
GO

/****** Object:  Job [CFIGLFiles]    Script Date: 2/8/2018 3:21:25 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]]    Script Date: 2/8/2018 3:21:25 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'CFIGLFiles', 
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
/****** Object:  Step [LoadSQLables]    Script Date: 2/8/2018 3:21:25 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'LoadSQLables', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'SSIS', 
		@command=N'/ISSERVER "\"\SSISDB\CFIGLFiles\CFIGLFiles\CFI_LoadSQLTables.dtsx\"" /SERVER BIETL01 /Par "\"$Project::ArchiveDirectory\"";"\"\\MANSAN02\BATCH$\CFI\CFIGLFILES\HISTORY\"" /Par "\"$Project::CFI_CaseReserveFile\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFICaseReserves.txt\"" /Par "\"$Project::CFI_DistributionList\"";"\"James_gabel@trg.com\"" /Par "\"$Project::CFI_FileSummary\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIFileSummary.txt\"" /Par "\"$Project::CFI_Paids\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIPaids.txt\"" /Par "\"$Project::CFI_QuarterSummaryFile\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIQuarterSummary.txt\"" /Par "\"$Project::HomeDirectory\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\"" /Par "\"$Project::RS_ReportingCon\"";"\"Data Source=DWProd;Initial Catalog=RS_Reporting;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;\"" /Par "\"$Project::SMTPRelayCon\"";"\"manrelay01.trg.com\"" /Par "\"$Project::ValuationDate\"";"\"9999-12-31\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CFI_CaseReserveFileLoad]    Script Date: 2/8/2018 3:21:25 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CFI_CaseReserveFileLoad', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'SSIS', 
		@command=N'/ISSERVER "\"\SSISDB\CFIGLFiles\CFIGLFiles\CFI_CaseReserveFileLoad.dtsx\"" /SERVER BIETL01 /Par "\"$Project::ArchiveDirectory\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\HISTORY\"" /Par "\"$Project::CFI_CaseReserveFile\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFICaseReserves.txt\"" /Par "\"$Project::CFI_DistributionList\"";"\"James_gabel@trg.com\"" /Par "\"$Project::CFI_FileSummary\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIFileSummary.txt\"" /Par "\"$Project::CFI_Paids\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIPaids.txt\"" /Par "\"$Project::CFI_QuarterSummaryFile\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIQuarterSummary.txt\"" /Par "\"$Project::HomeDirectory\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\"" /Par "\"$Project::RS_ReportingCon\"";"\"Data Source=DWProd;Initial Catalog=RS_Reporting;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;\"" /Par "\"$Project::SMTPRelayCon\"";"\"manrelay01.trg.com\"" /Par "\"$Project::ValuationDate\"";"\"9999-12-31\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CFI_PaidsFileLoad]    Script Date: 2/8/2018 3:21:25 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CFI_PaidsFileLoad', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'SSIS', 
		@command=N'/ISSERVER "\"\SSISDB\CFIGLFiles\CFIGLFiles\CFI_PaidsFileLoad.dtsx\"" /SERVER BIETL01 /Par "\"$Project::ArchiveDirectory\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\HISTORY\"" /Par "\"$Project::CFI_CaseReserveFile\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFICaseReserves.txt\"" /Par "\"$Project::CFI_DistributionList\"";"\"James_gabel@trg.com\"" /Par "\"$Project::CFI_FileSummary\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIFileSummary.txt\"" /Par "\"$Project::CFI_Paids\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIPaids.txt\"" /Par "\"$Project::CFI_QuarterSummaryFile\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIQuarterSummary.txt\"" /Par "\"$Project::HomeDirectory\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\"" /Par "\"$Project::RS_ReportingCon\"";"\"Data Source=DWProd;Initial Catalog=RS_Reporting;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;\"" /Par "\"$Project::SMTPRelayCon\"";"\"manrelay01.trg.com\"" /Par "\"$Project::ValuationDate\"";"\"9999-12-31\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CFI_FilesSummaryFileLoad]    Script Date: 2/8/2018 3:21:25 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CFI_FilesSummaryFileLoad', 
		@step_id=4, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'SSIS', 
		@command=N'/ISSERVER "\"\SSISDB\CFIGLFiles\CFIGLFiles\CFI_FilesSummaryFileLoad.dtsx\"" /SERVER BIETL01 /Par "\"$Project::ArchiveDirectory\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\HISTORY\"" /Par "\"$Project::CFI_CaseReserveFile\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFICaseReserves.txt\"" /Par "\"$Project::CFI_DistributionList\"";"\"James_gabel@trg.com\"" /Par "\"$Project::CFI_FileSummary\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIFileSummary.txt\"" /Par "\"$Project::CFI_Paids\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIPaids.txt\"" /Par "\"$Project::CFI_QuarterSummaryFile\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIQuarterSummary.txt\"" /Par "\"$Project::HomeDirectory\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\"" /Par "\"$Project::RS_ReportingCon\"";"\"Data Source=DWProd;Initial Catalog=RS_Reporting;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;\"" /Par "\"$Project::SMTPRelayCon\"";"\"manrelay01.trg.com\"" /Par "\"$Project::ValuationDate\"";"\"9999-12-31\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CFI_QuarterSummaryFileLoad]    Script Date: 2/8/2018 3:21:25 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CFI_QuarterSummaryFileLoad', 
		@step_id=5, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'SSIS', 
		@command=N'/ISSERVER "\"\SSISDB\CFIGLFiles\CFIGLFiles\CFI_QuarterSummaryFileLoad.dtsx\"" /SERVER BIETL01 /Par "\"$Project::ArchiveDirectory\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\HISTORY\"" /Par "\"$Project::CFI_CaseReserveFile\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFICaseReserves.txt\"" /Par "\"$Project::CFI_DistributionList\"";"\"James_gabel@trg.com\"" /Par "\"$Project::CFI_FileSummary\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIFileSummary.txt\"" /Par "\"$Project::CFI_Paids\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIPaids.txt\"" /Par "\"$Project::CFI_QuarterSummaryFile\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIQuarterSummary.txt\"" /Par "\"$Project::HomeDirectory\"";"\"\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\"" /Par "\"$Project::RS_ReportingCon\"";"\"Data Source=DWProd;Initial Catalog=RS_Reporting;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;\"" /Par "\"$Project::SMTPRelayCon\"";"\"manrelay01.trg.com\"" /Par "\"$Project::ValuationDate\"";"\"9999-12-31\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E', 
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


