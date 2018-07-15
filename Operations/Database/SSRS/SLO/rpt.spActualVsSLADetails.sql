USE [ServiceDesk]
GO

/****** Object:  StoredProcedure [rpt].[spActualVsSLADetails]    Script Date: 3/15/2016 11:17:13 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- Author:		Scott Marcus
-- Create date: July 6, 2015, recreated March 14, 2016
-- Description:	Actuals vs. SLA over 15 months by requester
-- ============================================================
CREATE PROCEDURE [rpt].[spActualVsSLADetails]
    @enddatep date,        -- Last month
	@RequesterADIDS varchar(2000),     -- List of AD ID's, comma separated
	@TechnicianADIDS varchar(2000)     -- List of AD ID's, comma separated
AS
BEGIN
	SET NOCOUNT ON;

Declare @FullEndDate DateTime = EOMONTH(@enddatep)
Declare @FullStartDate DateTime = DATEADD(month, DATEDIFF(month, 0, DATEADD(month, -14, @FullEndDate)), 0)

select r.WorkOrderID, r.Category, r.Subcategory, r.item, r.CreatedTime, r.CompletedTime, r.CompletionTimeInBusinessOpenHours 
from [ServiceDesk].dbo.request r

left join [ServiceCatalog].dbo.Task task on 
		task.ServiceDeskCategory = r.category and
		(task.ServiceDeskSubCategory=r.subcategory or task.ServiceDeskSubCategory='[app name]' or task.ServiceDeskSubCategory='[item]') and
		(task.ServiceDeskItem = r.item or task.ServiceDeskItem='')
left join [ServiceCatalog].dbo.ServiceLevelAgreement sla on sla.ServiceLevelAgreementID = task.ServiceLevelAgreementID

where 
BusinessHours is not null --and BusinessHours != 0
and Category != ''
and Status='Closed'
and r.CreatedTime between @FullStartDate and @FullEndDate
and UPPER(r.Technician) in (select * from rpt.fnNamesUnderManagersIncludingManagers(@FullStartDate, @FullEndDate, @TechnicianADIDS))
and UPPER(r.RequesterEmail) in (select * from rpt.fnEmailUnderManagersIncludingManagers(@FullStartDate,@FullEndDate,@RequesterADIDS))
order by category, Subcategory, item

END

GO


