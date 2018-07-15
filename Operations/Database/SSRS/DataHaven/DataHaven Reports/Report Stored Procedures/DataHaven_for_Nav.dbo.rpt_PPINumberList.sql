USE [DataHaven_for_Nav]
GO

/****** Object:  StoredProcedure [dbo].[rpt_PPINumberList]    Script Date: 2/2/2016 9:32:18 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.rpt_PPINumberList', 'P') IS NOT NULL
    DROP PROCEDURE dbo.rpt_PPINumberList;
go

CREATE PROCEDURE [dbo].[rpt_PPINumberList]
AS 
/*
    Name:	  dbo.rpt_PPINumberList
    Example:	 EXEC dbo.rpt_PPINumberList

    Notes:	 Returns a distinct list of PPI numbers currently available sorted in alpha ascending order.
			 Primarily used as the source of parameter selection list

    Modification History
    date		 name		  description
    01/29/2016	 pburke		  Initial create

*/
BEGIN
SET NOCOUNT ON;

    BEGIN TRY
	   SELECT u.source_number
	   FROM Ud_dt_dynamics_doc u
	   WHERE source_type <> 999
	   GROUP BY u.source_number
	   ORDER BY u.source_number;

    END TRY
    BEGIN CATCH
	   DECLARE @errSev SMALLINT;
	   DECLARE @errState SMALLINT;
	   DECLARE @errLine SMALLINT;
	   DECLARE @errMsg NVARCHAR(MAX);

	   SELECT @errSev = ERROR_SEVERITY(),
		  @errState = ERROR_STATE(),
		  @errLine = ERROR_LINE(),
		  @errMsg = ERROR_MESSAGE() + '( error number:  ' + CAST(ERROR_NUMBER() AS VARCHAR(5)) + ')';

	   RAISERROR(@errMsg, @errSev, @errState);
    END CATCH

END;

GO


