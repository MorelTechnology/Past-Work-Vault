USE [ServiceDesk];
GO

IF OBJECT_ID('rpt.spGetDepartmentList','P') IS NOT NULL
    DROP PROCEDURE rpt.spGetDepartmentList;
go

CREATE PROCEDURE [rpt].[spGetDepartmentList]  
AS
/*

-- =============================================
-- Author:		Rich Tremblay
-- Create date: July 10, 2015
-- Description:	Return a list of departments

    Modification History
    Date		 Name		  Description
    02/18/2016	 Pburke		  Added "Other" as an output list item

-- =============================================
*/
Begin 
	SELECT Department
	From Associate
	WHERE ISNULL(Department, '') <> ''
	GROUP BY Department

	Union All
	SELECT '-ALL-' 

	Union All
	SELECT 'Other'

	ORDER BY 1
END

GO


