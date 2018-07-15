USE [ServiceDesk]
GO

/****** Object:  StoredProcedure [rpt].[spGetDepartmentHierarchy]    Script Date: 2/15/2016 4:11:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('rpt.spGetDepartmentHierarchy','P') IS NOT NULL
    DROP PROCEDURE rpt.spGetDepartmentHierarchy;
go

CREATE PROCEDURE [rpt].[spGetDepartmentHierarchy] (
    @Department nvarchar(max) 
)
AS
/*

-- =============================================
-- Author:		Rich Tremblay
-- Create date: July 10, 2015
-- Description:	Return a hierarchy of managers

    Modification History
    Date		 Name		  Description
    02/18/2016	 Pburke		  Added "Other" as an output option
-- =============================================
*/
BEGIN
    SET NOCOUNT ON;
	Declare	@adid nvarchar(max) 
			,@SamAccountName varchar(100)

	Set @adid = rpt.fnGetDepartmentManager (@Department)
	
	If @Department = '-ALL-'			
		Begin 
		Set @SamAccountName = 'nbent'
		set @Department = '%'
		End 
	Else
		Begin 
		Set @SamAccountName = 
			( select Top 1 SamAccountName  
				From	Associate  
				Where   ActiveDirectoryID = @adid 
				ORDER BY StartDate desc )
		End 
	
	;
	WITH BOMcte(ActiveDirectoryID, Name, ComputedLevel, Sort) 
	AS 
		( 
		SELECT b.ActiveDirectoryID, CAST(b.DisplayName as nvarchar(100)), 0, CAST('\' + b.DisplayName as nvarchar(254)) 
		FROM dbo.Associate AS b 
		WHERE b.SamAccountName		= @SamAccountName 
				and b.Department	like @Department 
				and EndDate			is null

		UNION ALL 

		SELECT b.ActiveDirectoryID, CAST(REPLICATE (' |−--− ' , ComputedLevel) + b.DisplayName as nvarchar(100)), ComputedLevel + 1, 
			   CAST(cte.Sort + '\' + b.DisplayName as nvarchar(254)) 
		FROM dbo.Associate as b 
			INNER JOIN BOMcte AS cte 
				ON b.SupervisorID = cte.ActiveDirectoryID 
		WHERE EndDate	is null
				and b.Department like  @Department 
		) 
    
	SELECT * 
	FROM BOMcte 
	WHERE ActiveDirectoryID in 
			(select distinct SupervisorID from dbo.Associate where EndDate is null) 
			-- and ComputedLevel != 0 
     UNION ALL
	SELECT ActiveDirectoryID,
	   Name,
	   ComputedLevel,
	   Sort
     FROM ( SELECT 'Other' as ActiveDirectoryID,
		  'Other' as Name, 
		  '0' as ComputedLevel,
		  '0' as Sort ) A
     WHERE @Department = 'Other'
	ORDER BY Sort;

END


GO


