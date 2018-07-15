USE [DataHaven_for_Nav]
GO

/****** Object:  StoredProcedure [dbo].[rpt_UnPosted_Activity]    Script Date: 02/03/2016 07:45:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



IF OBJECT_ID('dbo.rpt_UnPosted_Activity','P') IS NOT NULL
    DROP PROCEDURE dbo.rpt_UnPosted_Activity;
go

CREATE PROCEDURE [dbo].[rpt_UnPosted_Activity]

@USERCODE VARCHAR (10) 
 
AS 

-- ================================================
 /*STORED PROCEDURE                                                                                  
  *  ==============                                                                                  
  *                                                                                                    
  *    ---Datahaven UnPosted Activity ---                                                                
  *                                                                                                 
  *                                                                                               
  *  DESCRIPTION                                                                                       
  *  ===========                                                                                       
  *                                                                                                    
  * ---This report produces Un-Posted activity.
                                  
  *                                                                                                    
  *                                                                                                    
  *                                                                                                    
  *  USAGE                                                                                             
  *  =====                                                                                             
  *                                                                                                    
  *    -----This SP is USED in the Datahaven Un-Posted Report-----                         
  *                                                                                                    
  *                                                                                                     
  *  PARAMETERS                                                                                        
  *  ==========                                                                                        
  *   NA                                                                                                 
  *                                        *
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
  *                                                                                                    

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

    CREATE TABLE #INV_NUM
    (
    Source_Num VARCHAR (30),
    NavInvNum VARCHAR (50),
    CreatedBy VARCHAR (10),
    LastModBy VARCHAR (10),
    OvernightMail INT
    )

    INSERT INTO #INV_NUM
    SELECT No_,[Vendor Invoice No_],[Created By],
    [Last Modified By],[Overnight Mail]  
    FROM Navision.dbo.[THC_RiverStone$Purch_ Inv_ Header]
    WHERE [Vendor Invoice No_] <> ''

    CREATE TABLE #INV_NUM2
    (
    Source_Num VARCHAR (20),
    NavInvNum VARCHAR (50),
    CreatedBy VARCHAR (10)
    ) 

    INSERT INTO #INV_NUM2
    SELECT No_,[Vendor Invoice No_],[Created By] 
    FROM Navision.dbo.[THC_RiverStone$Purchase Header]
    WHERE [Vendor Invoice No_] <> ''
 
 
    ---Open_UnPosted_Invoices---

    DECLARE @UserIDTEMP VARCHAR (10)
    SET @UserIDTEMP =  @USERCODE
    SET @UserIDTEMP =  REPLACE(@UserIDTEMP,'TRG\','')

    SELECT DISTINCT inn.CreatedBy,inn.NavInvNum [Invoice Number],qt_name [Queue Type],wfq.que_name [Queue Name],
    a.source_number [PPI Number],d.doc_name [Datahaven Document Name],a.InvoiceAmount [Invoice Amount],
    DATEDIFF(day,d.doc_key_date,GETDATE())[Days in Queue],
    dbo.fn_dh_DocIdToFileId(d.doc_id) FILE_ID,
    CASE
    WHEN ia.OverNightMail = 1
    THEN 'YES'
    ELSE ''
    END AS OverNightMail 
    FROM #AMOUNT a LEFT JOIN edc_doc d LEFT JOIN edc_folder df ON d.folder_fk = df.folder_id
    ON a.obj_id_fk = d.doc_id
    LEFT JOIN #INV_NUM2 inn ON inn.Source_Num = a.source_number
    LEFT JOIN #INV_NUM ia ON ia.Source_Num = a.source_number
    LEFT JOIN edc_Notes en ON d.doc_id = en.document_fk 
    LEFT JOIN workflow_docs wfd ON a.obj_id = wfd.obj_fk
    LEFT JOIN workflow_queue wfq ON wfd.docs_queue_fk = wfq.que_id
    LEFT JOIN queue_types ON qt_id = wfq.que_type
    JOIN dbo.RSRpt_Group dg ON inn.createdby = dg.QueName
    WHERE obj_type = 2 AND source_table_id = 38
    AND inn.CreatedBy IN (SELECT dg.QueName FROM dbo.RSRpt_Group dg JOIN dbo.RSRpt_User qu ON dg.QueNum = qu.QueNum 
    WHERE dg.QueNum BETWEEN 1 AND 12
    AND  qu.userid = @UserIDTEMP)
    AND que_name <> 'Deleted Documents'
    ORDER BY qt_name,wfq.que_name

    DROP TABLE #AMOUNT
    DROP TABLE #AMOUNT2
    DROP TABLE #INV_NUM
    DROP TABLE #INV_NUM2	
END;



GO


