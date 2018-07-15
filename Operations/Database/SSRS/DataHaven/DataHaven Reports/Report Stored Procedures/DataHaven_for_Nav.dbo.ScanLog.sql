USE [DataHaven_for_Nav]
GO

/****** Object:  StoredProcedure [dbo].[rpt_ScanLog]    Script Date: 02/03/2016 07:44:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.rpt_ScanLog','P') IS NOT NULL
    DROP PROCEDURE dbo.rpt_ScanLog;
go

CREATE PROCEDURE [dbo].[rpt_ScanLog]
	
AS
/*
    Name:		 dbo.rpt_ScanLog
    Example:	 EXEC dbo.rpt_ScanLog

    Modification History:
    Date		 Name		  Description
    02/03/2016	 Pburke		  removed unused joins from final output query
						  removed usused temp tables from process (#INV_NUM, #INV_NUM2)
*/
BEGIN
		
    CREATE TABLE #AMOUNT
    (
    obj_id INT NOT NULL,
    obj_id_fk INT NOT NULL,
    obj_type INT NOT NULL,
    source_table_id INT NOT NULL,
    tbl_fk INT NOT NULL,
    source_number CHAR (25),
    InvoiceAmount MONEY
    )


    INSERT INTO #AMOUNT
    SELECT m.obj_id ,m.obj_id_fk,m.obj_type,source_table_id,m.tbl_fk,
    u.source_number,Navision.dbo.CalcPostedInvAmount(u.source_number)InvoiceAmount
    FROM Ud_dt_dynamics_doc u                                                                                                                                                                                                           
    JOIN manage_object m on m.obj_id = u.object_fk
    JOIN edc_doc ed ON ed.doc_id = m.obj_id_fk
    Where Navision.dbo.CalcPostedInvAmount(u.source_number) IS NOT NULL

    CREATE TABLE #AMOUNT2
    (
    Source_number CHAR (25),
    InvoiceAmount MONEY,
    )

    INSERT INTO #AMOUNT2
    SELECT Source_number,Navision.dbo.CalcInvAmount(source_number)InvoiceAmount 
    FROM #AMOUNT
    WHERE InvoiceAmount IS NULL

    UPDATE #AMOUNT 
    SET InvoiceAmount = a2.InvoiceAmount  
    FROM #AMOUNT2 a2
    INNER JOIN 
    #AMOUNT a
    ON a.source_number = a2.source_number

    --CREATE TABLE #INV_NUM
    --(
    --Source_Num VARCHAR (20),
    --NavInvNum VARCHAR (50),
    --CreatedBy VARCHAR (10),
    --LastModBy VARCHAR (10)
    --)

    --INSERT INTO #INV_NUM
    --SELECT No_,[Vendor Invoice No_],[Created By],[Last Modified By]  
    --FROM Navision.dbo.[THC_RiverStone$Purch_ Inv_ Header]
    --WHERE [Vendor Invoice No_] <> ''


    --CREATE TABLE #INV_NUM2
    --(
    --Source_Num VARCHAR (20),
    --NavInvNum VARCHAR (50),
    --CreatedBy VARCHAR (10)
    --) 

    --INSERT INTO #INV_NUM2
    --SELECT No_,[Vendor Invoice No_],[Created By] 
    --FROM Navision.dbo.[THC_RiverStone$Purchase Header]
    --WHERE [Vendor Invoice No_] <> ''  
 
    -----ARO Invoices Scanned------

    SELECT qt.qt_id,
	   CONVERT(varchar,d.doc_key_date,101) AS DocKeyDate,
	   d.doc_key_series AS DocKeySeries,
	   dbo.fn_dh_DocIdToFileId(d.doc_id) FILE_ID,
	   '' AS Vendor,
	   '' AS [Invoice Num],
	   '' AS [Invoice Amount],
	   qt.qt_name [Queue Type],
	   wfq.que_name [Routed To],
	   ''AS [QA'd' By],
	   '' AS [Original Filed]
    FROM #AMOUNT a 
	   LEFT JOIN edc_doc d ON a.obj_id_fk = d.doc_id
	   --LEFT JOIN edc_folder df ON d.folder_fk = df.folder_id
	   --LEFT JOIN #INV_NUM ia ON ia.Source_Num = a.source_number 
	   --LEFT JOIN edc_Notes en ON d.doc_id = en.document_fk 
	   --LEFT JOIN workflow_docs wfd ON a.obj_id = wfd.obj_fk
	   LEFT JOIN workflow_queue wfq ON a.obj_id = wfq.que_id
	   LEFT JOIN queue_types qt ON wfq.que_type = qt.qt_id 
    WHERE CONVERT(varchar,d.doc_key_date,101) = CONVERT(varchar,GETDATE(),101) 
 
    DROP TABLE #AMOUNT
    DROP TABLE #AMOUNT2
    --DROP TABLE #INV_NUM
    --DROP TABLE #INV_NUM2

END;

GO


