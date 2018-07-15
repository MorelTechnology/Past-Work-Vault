USE [ServiceDesk]
GO
/****** Object:  UserDefinedFunction [rpt].[fnEmailUnderManagersIncludingManagers]    Script Date: 2/17/2016 1:45:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================
-- Author:           Scott Marcus
-- Create date: July 15, 2015
-- Description:      Returns email addresses of the associates
--              under one or more managers
--              including the managers
-- ========================================================
ALTER FUNCTION [rpt].[fnEmailUnderManagersIncludingManagers](@startdate date,@enddate date,@managers varchar(2000))
RETURNS @requestors TABLE(EmailAddress varchar(255))
BEGIN
       WITH BOMcte(ActiveDirectoryID, EmailAddress, Name, ComputedLevel, Sort) 
       AS 
       ( 
              SELECT b.ActiveDirectoryID, b.EmailAddress, CAST(b.DisplayName as nvarchar(100)), 0, CAST('\' + b.DisplayName as nvarchar(254)) 
              FROM Associate AS b 
              WHERE b.ActiveDirectoryID in (SELECT * FROM rpt.SplitString(@managers, ',')) and EndDate is null

              UNION ALL 

              SELECT b.ActiveDirectoryID, b.EmailAddress, CAST(b.DisplayName as nvarchar(100)), ComputedLevel + 1, CAST(cte.Sort + '\' + b.DisplayName as nvarchar(254)) 
              FROM Associate as b 
              INNER JOIN BOMcte AS cte 
              ON b.SupervisorID = cte.ActiveDirectoryID 
              WHERE (@startdate >= b.StartDate) and (b.StartDate <= @enddate) and ((b.EndDate is null) or (b.EndDate >= @enddate))
       ) 
       INSERT @requestors
       SELECT EmailAddress 
       FROM BOMcte 
       WHERE EmailAddress is not null
       ORDER BY Sort;
       RETURN
END
