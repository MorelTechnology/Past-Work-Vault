USE [ServiceDesk]
GO
/****** Object:  StoredProcedure [rpt].[spActualVsSLA]    Script Date: 3/15/2016 11:10:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================================
-- Author:		Scott Marcus
-- Create date: July 6, 2015 - recreated March 14, 2016
-- Description:	Actuals vs. SLA over 15 months
-- ============================================================
CREATE PROCEDURE [rpt].[spActualVsSLA]
    @enddatep date,        -- First month
	@RequesterADIDS varchar(2000),     -- List of AD ID's, comma separated
	@TechnicianADIDS varchar(2000)     -- List of AD ID's, comma separated
AS
BEGIN
	SET NOCOUNT ON;

Declare @FullEndDate DateTime = EOMONTH(@enddatep)
Declare @FullStartDate DateTime = DATEADD(month, DATEDIFF(month, 0, DATEADD(month, -14, @FullEndDate)), 0)

select distinct Category, Subcategory, item, avg(sla.BusinessHours) as SLA, 
		cast((cast(count(case when r.CompletionTimeInBusinessOpenHours <= sla.BusinessHours then 1 else null end) as float) / cast(count(*) as float)) * 100 as integer) as WithinSLA,
		count(*) as SLACount,
		max(case when r.CompletionTimeInBusinessOpenHours <= 1000 then r.CompletionTimeInBusinessOpenHours else 0 end) as MaxSLA,
		avg(case when r.CompletionTimeInBusinessOpenHours <= 1000 then r.CompletionTimeInBusinessOpenHours else 0 end) as AvgSLA,
		0 as Month1,
		0 as Month2, 
		0 as Month3,
		0 as Month4,
		0 as Month5,
		0 as Month6,
		0 as Month7,
		0 as Month8,
		0 as Month9,
		0 as Month10,
		0 as Month11,
		0 as Month12,
		0 as Month13,
		0 as Month14,
		0 as Month15,
		0 as ShowMonth1,
		0 as ShowMonth2, 
		0 as ShowMonth3,
		0 as ShowMonth4,
		0 as ShowMonth5,
		0 as ShowMonth6,
		0 as ShowMonth7,
		0 as ShowMonth8,
		0 as ShowMonth9,
		0 as ShowMonth10,
		0 as ShowMonth11,
		0 as ShowMonth12,
		0 as ShowMonth13,
		0 as ShowMonth14,
		0 as ShowMonth15

into #aggregate
from [ServiceDesk].dbo.request r

left join [ServiceCatalog].dbo.Task task on 
		task.ServiceDeskCategory = r.category and
		(task.ServiceDeskSubCategory=r.subcategory or task.ServiceDeskSubCategory='[app name]' or task.ServiceDeskSubCategory='[item]') and
		(task.ServiceDeskItem = r.item or task.ServiceDeskItem='')
left join [ServiceCatalog].dbo.ServiceLevelAgreement sla on sla.ServiceLevelAgreementID = task.ServiceLevelAgreementID

where 
BusinessHours is not null --and BusinessHours != 0
and Category != ''
and r.CreatedTime between @FullStartDate and @FullEndDate
and UPPER(r.Technician) in (select * from rpt.fnNamesUnderManagersIncludingManagers(@FullStartDate, @FullEndDate, @TechnicianADIDS))
and UPPER(r.RequesterEmail) in (select * from rpt.fnEmailUnderManagersIncludingManagers(@FullStartDate,@FullEndDate,@RequesterADIDS))
group by category, subcategory, item
order by category, Subcategory, item
-- select * from #aggregate

DECLARE @cat VARCHAR(60)
DECLARE @subcat VARCHAR(60)
DECLARE @item VARCHAR(60)
DECLARE @colname VARCHAR(60)
DECLARE @sla INT
DECLARE @pct INT

Declare @MonthsBack int=14
WHILE @MonthsBack >= 0
BEGIN
	Declare @StartDate DateTime = DateAdd(month,-@MonthsBack,@FullEndDate)
	set @StartDate =  DATEADD(month, DATEDIFF(month, 0, @StartDate), 0)
	Declare @EndDate DateTime = EOMONTH(@StartDate)

	--Select cast(@MonthsBack as varchar(2)) as MonthsBack, cast(@StartDate as varchar(40)) as StartDate, cast(@EndDate as varchar(40)) as EndDate
	DECLARE Month_Cursor CURSOR FOR
	select distinct Category, Subcategory, item, avg(sla.BusinessHours) as SLA, 
			cast((cast(count(case when r.CompletionTimeInBusinessOpenHours <= sla.BusinessHours then 1 else null end) as float) / cast(count(*) as float)) * 100 as integer) as WithinSLA
	from [ServiceDesk].dbo.request r

	left join [ServiceCatalog].dbo.Task task on 
		   task.ServiceDeskCategory = r.category and
		   (task.ServiceDeskSubCategory=r.subcategory or task.ServiceDeskSubCategory='[app name]' or task.ServiceDeskSubCategory='[item]') and
		   (task.ServiceDeskItem = r.item or task.ServiceDeskItem='')
	left join [ServiceCatalog].dbo.ServiceLevelAgreement sla on sla.ServiceLevelAgreementID = task.ServiceLevelAgreementID
    
	where 
	BusinessHours is not null --and BusinessHours != 0
	and Category != ''
	and r.CreatedTime between @StartDate and @EndDate
    and UPPER(r.Technician) in (select * from rpt.fnNamesUnderManagersIncludingManagers(@StartDate, @EndDate, @TechnicianADIDS))
    and UPPER(r.RequesterEmail) in (select * from rpt.fnEmailUnderManagersIncludingManagers(@StartDate, @EndDate, @RequesterADIDS))
	group by category, subcategory, item
	order by category, Subcategory, item

	--go through each row
	OPEN Month_Cursor
    FETCH NEXT FROM Month_Cursor Into @cat, @subcat, @item, @sla, @pct;
	WHILE @@FETCH_STATUS = 0
	   BEGIN

		  --select @cat, @subcat, @item, @pct
		  set @colname = 'Month' + CAST((15-@MonthsBack) as VARCHAR(2))

		  --update #aggregate set @colname = @pct where Category=@cat and Subcategory=@subcat and item=@item
          declare @sql nvarchar (500);
          set @sql = 'update #aggregate set ' + @colname + '=' + cast(@pct as varchar(10)) +' where Category=''' + @cat + ''' and Subcategory=''' + @subcat + ''' and item=''' + @item + ''''
          exec sp_executesql @sql;

		  set @colname = 'ShowMonth' + CAST((15-@MonthsBack) as VARCHAR(2))
          declare @sql2 nvarchar (500);
          set @sql2 = 'update #aggregate set ' + @colname + '=1' +' where Category=''' + @cat + ''' and Subcategory=''' + @subcat + ''' and item=''' + @item + ''''
          exec sp_executesql @sql2;

		  --select @sql2 as sql2

		  FETCH NEXT FROM Month_Cursor Into @cat, @subcat, @item, @sla, @pct;

	   END;

	   -- select 'Closing'
	CLOSE Month_Cursor;
	DEALLOCATE Month_Cursor;	

	set @MonthsBack = @MonthsBack - 1
END

select * from #aggregate
order by category, Subcategory, item


END
