USE IAFramework

IF EXISTS (SELECT OBJECT_ID(N'JAMS_Executions', 'U'))
BEGIN
	DROP TABLE utility.JAMS_Executions
END

CREATE TABLE utility.JAMS_Executions
(
	ID																BIGINT										NOT NULL IDENTITY (1, 1) PRIMARY KEY NONCLUSTERED
	, Job_Id														INT												NOT NULL INDEX idx_job_id CLUSTERED
	, Job_Name											NVARCHAR(64)
	, Setup_Id												INT												NOT NULL
	, Final_Status											NVARCHAR(MAX)
	, Submitted_By										NVARCHAR(64)
	, Sched_Time											DATETIME 
	, Start_Time											DATETIME
	, Completion_Time								DATETIME
	, TotalRunTime										AS DATEDIFF(ss, ISNULL(Start_Time, Completion_Time), Completion_Time)
	, Job_Folder											NVARCHAR(512)
	, Nodename											NVARCHAR(256)
	, Step_No												INT
	, Setup_Name										NVARCHAR(64)
	, Setup_Folder										NVARCHAR(512)
	, Server_Name										NVARCHAR(64)
)
