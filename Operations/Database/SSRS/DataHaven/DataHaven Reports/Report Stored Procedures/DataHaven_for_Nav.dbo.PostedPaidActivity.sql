USE [DataHaven_for_Nav]
GO

/****** Object:  StoredProcedure [dbo].[rpt_PostedPaidActivity]    Script Date: 02/03/2016 07:43:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



IF OBJECT_ID('dbo.rpt_PostedPaidActivity','P') IS NOT NULL
    DROP PROCEDURE dbo.rpt_PostedPaidActivity;
go

CREATE PROCEDURE [dbo].[rpt_PostedPaidActivity] 

 @USERCODE VARCHAR (10),
 @StartDate DATE,
 @EndDate DATE

AS
-- ================================================
 /*STORED PROCEDURE                                                                                  
  *  ==============                                                                                  
  *                                                                                                    
  *    ---Datahaven Posted Paid Activity---                                                                *
  *                                                                                                 
  *                                                                                               
  *  DESCRIPTION                                                                                       
  *  ===========                                                                                       
  *                                                                                                    
  * ---This sp produces Posted Paid Invoice infornmation from Datahaven along with related detail from
  * ---Navision                                
  *                                                                                                    
  *                                                                                                    
  *                                                                                                    
  *  USAGE                                                                                             
  *  =====                                                                                             
  *                                                                                                    
  *    -----This SP is USED in the SSRS Posted Paid Activity Report-----                         
  *                                                                                                    
  *                                                                                                     
  *  PARAMETERS                                                                                        
  *  ==========                                                                                        
  *   @StartDate                                                                                                 
  *   @EndDate                                    
  *   @CreatedBy                                                                                                
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
  *     7/8/2015      1.1            Lincoln Ford				Any reference to Source Number needed to be expanded to Varchar(100)                                                                                                    
	   02/03/2016	  1.2		  Pburke				Corrected LEFT JOIN syntax on final output query.
												Removed LEFT JOINs not being used in the final output query
												Changed user id security test to EXISTS instead of INSERT	
												Changed join for #CHECK from LEFT to INNER
												Added test for NULL doc_id before executing function call to dbo.fn_dh_DocIdToFileId
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
    source_number varchar (100),
    InvoiceAmount MONEY
    )


    INSERT INTO #AMOUNT
    SELECT m.obj_id ,m.obj_id_fk,m.obj_type,source_table_id,m.tbl_fk,
    u.source_number,Navision.dbo.CalcPostedInvAmount(u.source_number)InvoiceAmount
    FROM Ud_dt_dynamics_doc u                                                                                                                                                                                                           
    JOIN manage_object m on m.obj_id = u.object_fk
    JOIN edc_doc ed ON ed.doc_id = m.obj_id_fk


    CREATE TABLE #AMOUNT2
    (
    Source_number varchar (100),
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
    Source_Num varchar (100),
    NavInvNum VARCHAR (50),
    CreatedBy VARCHAR (10),
    LastModBy VARCHAR (10)
    )

    INSERT INTO #INV_NUM
    SELECT No_,[Vendor Invoice No_],[Created By],[Last Modified By]---,[Overnight Mail]  
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


    ----POSTED PAID INVOICES-----

    DECLARE @UserIDTEMP VARCHAR (10)
    SET @UserIDTEMP =  @USERCODE
    SET @UserIDTEMP =  REPLACE(@UserIDTEMP,'TRG\','')


    SELECT DISTINCT ia.CreatedBy as [Created By],
	   ia.LastModBy,
	   ia.NavInvNum as [Invoice Number],
	   a.source_number as [PPI Number],
	   d.doc_name as [Datahaven Document Name],
	   a.InvoiceAmount,
	   CASE
		  WHEN c.CheckNo IS NULL THEN '' 
		  ELSE c.CheckNo
	   END as [Check Number],
	   CASE
		  WHEN c.PostingDate IS NULL THEN '' 
		  ELSE c.PostingDate
	   END as [Check Date],
	   CASE
		  WHEN cm.VendorCrMemo IS NULL THEN '' 
		  ELSE 'YES'
	   END as [Credit Memo],
	   CASE
		  WHEN DATEDIFF(day,d.doc_key_date,c.PostingDate) IS NULL THEN ''
		  ELSE DATEDIFF(day,d.doc_key_date,c.PostingDate)
	   END AS [Inv Processing Time],
	   CONVERT (DATE,d.doc_key_date) as [Date Entered Workflow],
	   CASE 
	     WHEN d.doc_id IS NOT NULL THEN dbo.fn_dh_DocIdToFileId(d.doc_id)
		ELSE NULL
	   END as FILE_ID
    FROM #AMOUNT a 
	   JOIN #CHECK c ON a.source_number = c.PPI_NUM
				    AND c.CheckNo IS NOT NULL
				    AND c.PostingDate > @StartDate
				    AND c.PostingDate < @EndDate
	   LEFT JOIN edc_doc d ON a.obj_id_fk = d.doc_id
	   LEFT JOIN #INV_NUM ia ON ia.Source_Num = a.source_number 
	   LEFT JOIN #CREDITMEMO cm ON a.source_number = cm.PPI_Number 
	   
	   --LEFT JOIN edc_folder df ON d.folder_fk = df.folder_id
	   --LEFT JOIN edc_Notes en ON d.doc_id = en.document_fk 
	   --LEFT JOIN workflow_docs wfd ON a.obj_id = wfd.obj_fk
	   --LEFT JOIN workflow_queue wfq ON wfd.docs_queue_fk = wfq.que_id
	   --LEFT JOIN queue_types qt ON wfq.que_type = qt.qt_id 
	   --JOIN RSRpt_Group dg ON ia.createdby = dg.QueName
	   --LEFT JOIN #CHECK c ON c.PPI_NUM = a.source_number
	   --LEFT JOIN #CREDITMEMO cm ON cm.PPI_Number = a.source_number
    WHERE obj_type = 2 
	   AND source_table_id = 122
	   AND EXISTS (SELECT dg.QueName 
						  FROM dbo.RSRpt_Group dg 
							 JOIN dbo.RSRpt_User qu ON dg.QueNum = qu.QueNum 
						  WHERE dg.QueNum BETWEEN 1 AND 12
						  AND  qu.userid = @UserIDTEMP
						  AND  dg.QueName = ia.CreatedBy)
	   --AND c.CheckNo IS NOT NULL
	   --AND c.PostingDate > @StartDate
	   --AND c.PostingDate < @EndDate
    ORDER BY [Check Number]

    DROP TABLE #AMOUNT
    DROP TABLE #AMOUNT2
    DROP TABLE #CHECK
    DROP TABLE #INV_NUM	
END;


GO


