Use DataHaven_for_Nav;
go

IF OBJECT_ID('dbo.rpt_ScannedDocuments','P') IS NOT NULL
    DROP PROCEDURE dbo.rpt_ScannedDocuments;

go

CREATE PROCEDURE dbo.rpt_ScannedDocuments
    @docName	 AS CHAR(80)
AS
/*
    Name:		 dbo.rpt_ScannedDocuments
    Example:	 EXEC dbo.rpt_ScannedDocuments 'name here'

    Note:  The input parm @docName is declared as Char(80) to match the definition of the doc_name field in the
		  edc_doc table

    Modification History:
    Date		 Name		  Description
    02/04/2016	 Pburke		  Initial Create (taking select statement from report source and moving into proc)

*/

BEGIN
SET NOCOUNT ON;
    BEGIN TRY
	   SELECT  d.doc_name, 
		  di.doc_fk,
		  di.page_nbr,
		  di.magnetic_blob
	   FROM edc_doc d 
		  LEFT JOIN edc_page_mag di ON d.doc_id = di.doc_fk
	   WHERE d.Doc_Name = @DocName;
    END TRY
    BEGIN CATCH
	   DECLARE @errNum Int;
	   DECLARE @errSev Int;
	   DECLARE @errLine Int;
	   DECLARE @errMsg nvarchar(max);

	   SELECT @errNum = ERROR_NUMBER() ,
		  @errSev = ERROR_SEVERITY(),
		  @errLine = ERROR_LINE(),
		  @errMsg = ERROR_MESSAGE();

	   Raiserror(N'Error Number %d - %s (line # %d)',  -- error message text
		  @errSev,   -- error severity
		  1,		   -- error state
		  @errNum,   -- 1st substitute in message for %d
		  @errMsg,   -- 2nd for %s
		  @errLine); -- 3rd for %d
	      
    END CATCH
END;
go
