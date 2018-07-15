USE [msdb]
GO

/****** Object:  Job [MEF Transfer To ODS]    Script Date: 4/25/2016 2:51:16 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 4/25/2016 2:51:16 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'MEF Transfer To ODS', 
		@enabled=1, 
		@notify_level_eventlog=3, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@notify_email_operator_name=N'IT Production Support Group', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Check to see if monthend is currently processing]    Script Date: 4/25/2016 2:51:16 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Check to see if monthend is currently processing', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=1, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
DECLARE @dayStart AS DATE, @dayToday AS DATE;
DECLARE @bit Bit;

SET @dayToday = GETDATE();
SET @dayStart = DATEADD(d,1,EOMONTH(@dayToday, -1));


WITH cteDays AS
(   /*
	   List the days for the current month including weekday number
    */
    SELECT 1 as Rownum,
    @dayStart as MoDay,
    DATEPART(weekday, @dayStart) as WkDay
    UNION ALL
    SELECT Rownum + 1,
    DATEADD(d,1, MoDay) as MoDay,
    DATEPART(weekday,DATEADD(d,1,MoDay)) as WkDay
    FROM cteDays
    WHERE MoDay <= EOMONTH(@dayStart,0)
),
cteFridays AS
(   /*
	   Select all the Fridays and assign the count within the month
    */
    SELECT *, 
    COUNT(WkDay) OVER (partition by WkDay order by Rownum) countInMonth,
    DATEDIFF(d,@dayToday,MoDay) dayDiff
    FROM cteDays
    WHERE WkDay = 6
)
/*
    If today''s date (the execute date) is between the 2nd and 3rd Friday
    do not execute further
*/
SELECT @bit = IIF(Count(*)= 0, 1, 0)
FROM (
	   SELECT Max(ISNULL(A.StartDate, CAST(''1/1/1900'' AS DATE))) StartDt,
			Max(ISNULL(A.EndDate, CAST(''1/1/1900'' AS DATE))) EndDt
	   FROM (
			 SELECT MoDay as StartDate, NULL as EndDate
			 FROM cteFridays f
			 WHERE countInMonth = 2
			 UNION ALL
			 SELECT NULL as StartDate, MoDay as EndDate
			 FROM cteFridays f
			 WHERE countInMonth = 3 ) A
    ) B
WHERE @dayToday BETWEEN B.StartDt AND B.EndDt;

IF @bit = 0
    RAISERROR(''Automated processing cannot occur while monthend close is in process.'', 16,1);', 
		@database_name=N'master', 
		@flags=4
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [ETL MEFtoODS_Transfer]    Script Date: 4/25/2016 2:51:16 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'ETL MEFtoODS_Transfer', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'SSIS', 
		@command=N'/ISSERVER "\"\SSISDB\MEFtoODSTransfer\ManualEntryFacilityETL\MEFtoODS_Transfer.dtsx\"" /SERVER BIETL01 /X86 /ENVREFERENCE 1 /Par pSendEmailsTo;"\"BIU_MEF@trg.com\"" /Par "\"pValuationPeriodOverride(DateTime)\"";"\"12/31/1899 12:00:00 AM\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E', 
		@database_name=N'master', 
		@flags=32
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Weekdays at 11:00 PM', 
		@enabled=1, 
		@freq_type=8, 
		@freq_interval=63, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=1, 
		@active_start_date=20160121, 
		@active_end_date=99991231, 
		@active_start_time=230000, 
		@active_end_time=235959, 
		@schedule_uid=N'568bcb56-aaa1-44f6-8427-fbd3a064e71a'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO


