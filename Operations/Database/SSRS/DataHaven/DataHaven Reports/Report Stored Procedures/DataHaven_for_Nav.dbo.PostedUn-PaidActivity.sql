USE [DataHaven_for_Nav]
GO

/****** Object:  StoredProcedure [dbo].[rpt_PostedUn-PaidActivity]    Script Date: 02/03/2016 07:43:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.[rpt_PostedUn-PaidActivity]','P') IS NOT NULL
    DROP PROCEDURE [dbo].[rpt_PostedUn-PaidActivity];
go

CREATE PROCEDURE [dbo].[rpt_PostedUn-PaidActivity]

@USERCODE VARCHAR (10)
 
AS

-- ================================================
 /*STORED PROCEDURE                                                                                  
  *  ==============                                                                                  
  *                                                                                                    
  *    ---Datahaven Posted UnPaid Activity ---                                                                *
  *                                                                                                 
  *                                                                                               
  *  DESCRIPTION                                                                                       
  *  ===========                                                                                       
  *                                                                                                    
  * ---This sp produces Posted UnPaid Invoice infornmation from datahaven along with related detail from
    ---Navision                                
  *                                                                                                    
  *                                                                                                    
  *                                                                                                    
  *  USAGE                                                                                             
  *  =====                                                                                             
  *                                                                                                    
  *    -----This SP is USED in the SSRS Posted Activity Report-----                         
  *                                                                                                    
  *                                                                                                     
  *  PARAMETERS                                                                                        
  *  ==========                                                                                        
  *   NA                                                                                                 
  *                                        
  *                                                                                                    
  *  VARIABLES                                                                                         
  *  =========                                                                                         
  *     NA                                                                                               
  *                                                                 
  *                                                                                                    
  *                                                                                                    
  *  RETURN VALUE                                                                                      
  *  ============                                                                                      
  *                                                                                                    
  *     0                  No Errors                                                                   
  *    -1                  Description of cause of non-zero return value                               
  *                                                                                                    
  *                                                                                                    
  *  PROGRAMMING NOTES                                                                                 
  *  ================= 
                                                                                  
                                                                                                                                                                                                      *
  *  CHANGE HISTORY                                                                                    
  *  ==============                                                                                    
  *                                                                                                    
  *     Date        Version          Author	        Description                                    
  *     ====        =======          ======             ===========                                    
  *                                                                                                    
  *     4/10/2013    1.0            Steve Carignan         Origianl Version                               
	   02/03/2016	 1.1			 Pburke				Removed unused joins from final output query
												Changed user id test to EXISTS instead of INSERT			
												Checked for NULL doc_id before executing dbo.fn_dh_DocIdToFileId(d.doc_id) 
	                                                                                                  

*/
BEGIN
SET NOCOUNT ON;

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
    WHERE  Navision.dbo.CalcPostedInvAmount(u.source_number) IS NOT NULL

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

    CREATE TABLE #INV_NUM
    (
    Source_Num VARCHAR (20),
    NavInvNum VARCHAR (50),
    PaymentCode VARCHAR (10),
    CreatedBy VARCHAR (10),
    LastModBy VARCHAR (10),
    OvernightMail INT
    )

    INSERT INTO #INV_NUM
    SELECT No_,[Vendor Invoice No_],[Payment Terms Code],[Created By] ,[Last Modified By],[Overnight Mail] 
    FROM Navision.dbo.[THC_RiverStone$Purch_ Inv_ Header]
    WHERE [Vendor Invoice No_] <> ''
 
    CREATE TABLE #CHECK
    (
    PPI_NUM VARCHAR (10),
    CheckNo VARCHAR (20),
    PostingDate VARCHAR (10),
    VendorNo VARCHAR (30)
    )

    INSERT INTO #CHECK
    SELECT SUBSTRING(vle.Description,20,9) PPI_NUM,
    vle.[Document No_] CheckNo, CONVERT(DATE,gle.[Posting Date]),vle.[Vendor No_]
    FROM Navision.dbo.[THC_RiverStone$Vendor Ledger Entry] vle
    LEFT JOIN Navision.dbo.[THC_RiverStone$G_L Entry] gle ON gle.[Entry No_] =  vle.[Entry No_]
    WHERE [Applies-to Doc_ Type] = 2
    AND vle.[Source Code] <> 'FINVOIDCHK'

    UPDATE #CHECK
    SET PostingDate = ''
    WHERE PostingDate IS NULL

    CREATE TABLE #CREDITMEMO
    (
    PPI_Number VARCHAR (10),
    PPCM_NUM VARCHAR (10),
    VendorCrMemo VARCHAR (25),
    Postingdate DATE,
    DirectUnitCost MONEY
    )

    INSERT INTO #CREDITMEMO
    SELECT [Applies-to Doc_ No_],cmh.No_,[Vendor Cr_ Memo No_],
    cmh.[Posting Date],SUM(cml.[Direct Unit Cost])AS [Direct Unit Cost]
    FROM Navision.dbo.[THC_RiverStone$Purch_ Cr_ Memo Hdr_]cmh
    JOIN Navision.dbo.[THC_RiverStone$Purch_ Cr_ Memo Line] cml ON cmh.No_ = cml.[Document No_]
    WHERE [Applies-to Doc_ Type] = 2
    GROUP BY [Applies-to Doc_ No_],[Applies-to Doc_ Type],cmh.No_,[Posting Description],[Vendor Cr_ Memo No_],
    cmh.[Posting Date]

    ----POSTED UNPAID INVOICES-----

    DECLARE @UserIDTEMP VARCHAR (10)
    SET @UserIDTEMP =  @USERCODE
    SET @UserIDTEMP =  REPLACE(@UserIDTEMP,'TRG\','')


    SELECT DISTINCT ia.CreatedBy[Created By],
	   ia.LastModBy,
	   ia.NavInvNum [Invoice Number],
	   a.source_number [PPI Number],
	   d.doc_name [Datahaven Document Name],
	   a.InvoiceAmount,
	   CASE
		  WHEN cm.VendorCrMemo IS NULL THEN ''
		  ELSE 'YES'
	   END  AS [Credit Memo],

	   CASE
		  WHEN DATEDIFF(day,c.PostingDate,d.doc_key_date) IS NULL THEN ''
		  ELSE DATEDIFF(day,c.PostingDate,d.doc_key_date)
	   END AS [Invoice Processing Time],
	   CONVERT (DATE,d.doc_key_date) [Date Entered Workflow],
	   CASE
		  WHEN d.doc_id IS NOT NULL THEN dbo.fn_dh_DocIdToFileId(d.doc_id) 
		  ELSE NULL
	   END as FILE_ID,
	   CASE
		  WHEN OvernightMail = 0 THEN ''
		  ELSE 'YES'
	   END AS 'OverNightMail'
    FROM #AMOUNT a 
	   LEFT JOIN edc_doc d ON a.obj_id_fk = d.doc_id
	   LEFT JOIN #INV_NUM ia ON ia.Source_Num = a.source_number 
	   LEFT JOIN #CHECK c ON c.PPI_NUM = a.source_number
	   LEFT JOIN #CREDITMEMO cm ON cm.PPI_Number = a.source_number

	   --LEFT JOIN edc_folder df ON d.folder_fk = df.folder_id
	   --LEFT JOIN edc_Notes en ON d.doc_id = en.document_fk 
	   --LEFT JOIN workflow_docs wfd ON a.obj_id = wfd.obj_fk
	   --LEFT JOIN workflow_queue wfq ON wfd.docs_queue_fk = wfq.que_id
	   --LEFT JOIN queue_types ON qt_id = wfq.que_type
	   --JOIN RSRpt_Group dg ON ia.createdby = dg.QueName
    WHERE obj_type = 2 
	   AND source_table_id = 122
	   AND EXISTS ( SELECT dg.QueName 
				FROM dbo.RSRpt_Group dg JOIN dbo.RSRpt_User qu ON dg.QueNum = qu.QueNum 
				WHERE dg.QueNum BETWEEN 1 AND 12
				AND  qu.userid = @UserIDTEMP
				AND  dg.QueName = ia.CreatedBy )
	   AND c.CheckNo IS NULL
	   AND ia.PaymentCode <> 'WIRE';


    DROP TABLE #AMOUNT
    DROP TABLE #AMOUNT2
    DROP TABLE #CHECK
    DROP TABLE #INV_NUM
    DROP TABLE #CREDITMEMO
	
END;


GO


