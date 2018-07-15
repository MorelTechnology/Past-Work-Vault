USE [ServiceDesk]
GO

/****** Object:  StoredProcedure [dbo].[rpt_GetReportStartDate]    Script Date: 8/4/2015 5:52:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--date procedures--

CREATE PROCEDURE [dbo].[rpt_GetReportStartDate]
	@Period as varchar(max),
	@StartDate as Date

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	    
	IF (@Period = 'M')
	  SELECT(DATEFROMPARTS(YEAR(@StartDate),MONTH(@StartDate),1)) as ReportStartDate
	ELSE
	  SELECT(@StartDate) AS ReportStartDate

END


GO

CREATE PROCEDURE [dbo].[rpt_GetDefaultStartDate]
	@Period as varchar(1),
	@UserEndDate as Date

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF (@Period = 'M')
	  SELECT(DATEADD(MONTH,-14,DATEFROMPARTS(YEAR(@UserEndDate),MONTH(@UserEndDate),1))) as UserStartDate
	ELSE
	  SELECT(DATEADD(MONTH,-1,@UserEndDate)) AS UserStartDate

END

GO


CREATE PROCEDURE [dbo].[rpt_GetReportEndDate]
	@Period as varchar(1),
	@UserEndDate as Date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF (@Period = 'M')
	  SELECT(EOMONTH(@UserEndDate)) as ReportEndDate
	ELSE
	  SELECT(@UserEndDate) AS ReportEndDate

END

GO


CREATE PROCEDURE [dbo].[rpt_GetDefaultEndDate]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT(DATEADD(DAY,-1,DATEFROMPARTS(YEAR(GETDATE()),MONTH(GETDATE()),1))) as UserEndDate
	

END

GO

-- rpt_PriorityCounts --

ALTER PROCEDURE [dbo].[rpt_PriorityCounts]
	@StartDate as DATE,
	@EndDate as DATE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

     WITH PrioritySortTime AS
 (
 SELECT *,ROW_NUMBER() OVER (PARTITION BY Ticket_No ORDER BY Request_Ex.Ticket_No,Field_Ex.PriorityUpdateTime ASC) as RN FROM

 (SELECT R.WorkOrderID as Ticket_No,CreatedTime as RCreatedTime,
 (CASE R.[Priority] WHEN '-' THEN '0. Not Assigned' ELSE R.[Priority] END) as RPriority,
 R.Requester as Requestor,R.Subject as Subject,O.OperationID as R_OpID
 FROM Request R
 JOIN Operation O ON
 O.WorkOrderID = R.WorkOrderID 
 WHERE CONVERT(Date,R.[CreatedTime]) BETWEEN @StartDate and @EndDate) Request_Ex
 
 LEFT JOIN
 
 (SELECT D.OperationID as F_OpID,D.OperationTime as PriorityUpdateTime,F.Field,F.OldValue,
 (CASE F.Value WHEN '-' THEN '0. Not Assigned' ELSE F.Value END) AS FPriority
  FROM Detail D
  JOIN Field F ON
  F.DetailID = D.DetailID
  WHERE D.Operation = 'UPDATE' and
       (F.[Field] = 'Priority' or F.[Field] IS NULL)) Field_Ex

 ON Request_Ex.R_OpID = Field_Ex.F_OpID 
 )
 SELECT *,CONVERT(Date,RCreatedTime) as CreatedDate,DATEADD(MONTH, DATEDIFF(MONTH, 0, RCreatedTime), 0) as CreatedMonth,
 (CASE ISNULL(F_OpID,'')
  WHEN '' THEN RPriority
  ELSE FPriority END) AS FirstPriority
 
 FROM PrioritySortTime
 WHERE RN = 1
 OPTION (RECOMPILE)
END
GO

-- rpt_PriorityCountsDetails --

CREATE PROCEDURE [dbo].[rpt_PriorityCountsDetails]
	@StartDate as DATE,
	@EndDate as DATE,
	@Priority as varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
     WITH PrioritySortTime AS
 (
 SELECT *,ROW_NUMBER() OVER (PARTITION BY Ticket_No ORDER BY Request_Ex.Ticket_No,Field_Ex.PriorityUpdateTime ASC) as RN FROM

 (SELECT R.WorkOrderID as Ticket_No,CreatedTime as RCreatedTime,
 (CASE R.[Priority] WHEN '-' THEN '0. Not Assigned' ELSE R.[Priority] END) as RPriority,
 R.Requester as Requestor,R.Subject as Subject,OperationID as R_OpID
 FROM Request R
 JOIN Operation O ON
 O.WorkOrderID = R.WorkOrderID 
 WHERE CONVERT(Date,R.[CreatedTime]) BETWEEN @StartDate and @EndDate) Request_Ex
 
 LEFT JOIN
 
 (SELECT D.OperationID as F_OpID,D.OperationTime as PriorityUpdateTime,F.Field,F.OldValue,
 (CASE F.Value WHEN '-' THEN '0. Not Assigned' ELSE F.Value END) AS FPriority
  FROM Detail D
  JOIN Field F ON
  F.DetailID = D.DetailID
  WHERE D.Operation = 'UPDATE' and
       (F.[Field] = 'Priority' or F.[Field] IS NULL)) Field_Ex

 ON Request_Ex.R_OpID = Field_Ex.F_OpID 
 ),
 GetFirstPriority AS
 (SELECT *,CONVERT(Date,RCreatedTime) as CreatedDate,DATEADD(MONTH, DATEDIFF(MONTH, 0, RCreatedTime), 0) as CreatedMonth,
 (CASE ISNULL(F_OpID,'')
  WHEN '' THEN RPriority
  ELSE FPriority END) AS FirstPriority
 FROM PrioritySortTime
 WHERE RN = 1)
 
 SELECT GetFirstPriority.*,rpt.fnStripFormatting(ShortDescription) as ShortDescription
 FROM GetFirstPriority
 JOIN Request ON
 Request.WorkOrderID = GetFirstPriority.Ticket_No
 WHERE GetFirstPriority.FirstPriority = @Priority
 OPTION (RECOMPILE)
END

GO




