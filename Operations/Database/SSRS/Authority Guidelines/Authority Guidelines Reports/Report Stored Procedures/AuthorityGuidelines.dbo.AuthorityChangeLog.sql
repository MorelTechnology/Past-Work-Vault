USE [AuthorityGuidelines]
GO

/****** Object:  StoredProcedure [dbo].[rpt_AuthorityChangeLog]    Script Date: 02/04/2016 08:00:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[rpt_AuthorityChangeLog] 
	-- Add the parameters for the stored procedure here
	
@StartDate DATE,
@EndDate DATE,
@InputDate DATETIME = NULL

AS

BEGIN


select
 CAST(AL.StartDate AS DATE) AS StartDate
,CAST(AL.EndDate AS DATE) AS ChangeDate
,AL.StartUser AS ChangedBy
,LastName
,[A].[LastName] + ', ' + [A].[FirstName] AS Associate
,D.Description as Department
,L.Description AS LimitType
,G.Description AS [Group]
,CASE
WHEN AL.Limit IN(-1,-2,-3)
THEN 0
ELSE AL.Limit 
END AS Amount
,CASE
WHEN AL.Limit = -1
THEN 'Unlimited'
WHEN AL.Limit = -2
THEN 'Policy Limits'
WHEN AL.Limit = -3
THEN 'DH_Approver'
ELSE '0'
END AS Amount2
,AL.TicketNumber
,R.Requester AS TicketRequester
,R.Subject AS TicketSubject
FROM [AG].[Associate] [A]
JOIN [AG].[Associate_Limit] [AL]
ON [AL].AssociateID = [A].AssociateID
LEFT JOIN [AG].[Department] [D]
ON [D].[DepartmentID] = [A].[DepartmentID]
LEFT JOIN [AG].Limit L
ON
L.LimitID = [AL].LimitID
LEFT JOIN ServDesk.ServDesk.dbo.Requests R
ON R.WorkorderID = AL.TicketNumber
LEFT JOIN [AG].[GROUP]G
ON A.GroupID = G.GroupID
--WHERE (CAST(AL.EndDate AS DATE) = @ChangeDate
--OR A.LastName = @LastName
--OR @InputDate = @InputDate)
WHERE (AL.StartUser <> 'InitialLoad'
OR (AL.StartUser = 'InitialLoad' And AL.EndDate IS NOT NULL))
AND AL.Limit <> 0
AND @InputDate Between @StartDate and @EndDate

Group by 
AL.StartDate
,AL.EndDate
,AL.StartUser
,A.LastName
,A.FirstName
,D.Description
,L.Description
,AL.Limit
,AL.TicketNumber
,R.Requester
,R.Subject
,G.Description
ORDER BY A.LastName


		
END


GO


