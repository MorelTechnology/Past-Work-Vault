USE [DataHaven_for_Nav]
GO

/****** Object:  StoredProcedure [dbo].[rpt_CreditMemo]    Script Date: 02/03/2016 07:39:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('rpt_CreditMemo','P') IS NOT NULL
    DROP PROCEDURE dbo.rpt_CreditMemo;
go

CREATE PROCEDURE [dbo].[rpt_CreditMemo] 

@USERCODE VARCHAR (10),
@PPINumber VARCHAR (25)
 
AS


-- ================================================
 /*STORED PROCEDURE                                                                                  
  *  ==============                                                                                  
  *                                                                                                    
  *    ---Datahaven Credit Memo Detail---                                                                *
  *                                                                                                 
  *                                                                                               
  *  DESCRIPTION                                                                                       
  *  ===========                                                                                       
  *                                                                                                    
  * ---This sp produces Credit Memo Detail from Datahaven along with related Navision attributes                               
  *                                                                                                    
  *                                                                                                    
  *                                                                                                    
  *  USAGE                                                                                             
  *  =====                                                                                             
  *                                                                                                    
  *  This SP is used in conjunction with Posted Paid and Posted UnPaid Activity Report                        
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
    WHERE  Navision.dbo.CalcPostedInvAmount(u.source_number) IS NOT NULL
    ----WHERE m.obj_type = 2 AND u.source_table_id = 122

    CREATE TABLE #INV_NUM
    (
    Source_Num VARCHAR (25),
    NavInvNum VARCHAR (50),
    CreatedBy VARCHAR (10),
    LastModBy VARCHAR (10)
    )

    INSERT INTO #INV_NUM
    SELECT No_,[Vendor Invoice No_],[Created By],[Last Modified By]  
    FROM Navision.dbo.[THC_RiverStone$Purch_ Inv_ Header]
    WHERE [Vendor Invoice No_] <> ''

    ---Credit Memo---


    DECLARE @UserIDTEMP VARCHAR (10)
    SET @UserIDTEMP =  @USERCODE
    SET @UserIDTEMP =  REPLACE(@UserIDTEMP,'TRG\','')


    SELECT ia.CreatedBy,
	   [Applies-to Doc_ No_] [PPI Number],
	   cmh.No_ [Credit Number],
	   [Vendor Cr_ Memo No_] AS [Credit Memo Invoice Number],
	   cmh.[Posting Date],
	   SUM(CONVERT(MONEY,cml.[Direct Unit Cost]))AS [Amount]
    FROM Navision.dbo.[THC_RiverStone$Purch_ Cr_ Memo Hdr_]cmh
	   JOIN Navision.dbo.[THC_RiverStone$Purch_ Cr_ Memo Line] cml ON cmh.No_ = cml.[Document No_]
	   JOIN #AMOUNT a ON a.source_number = cmh.[Applies-to Doc_ No_] COLLATE SQL_Latin1_General_CP1_CI_AS
	   JOIN #INV_NUM ia ON ia.Source_Num = a.source_number
	   JOIN dbo.RSRpt_Group dg ON ia.createdby = dg.QueName  
    WHERE [Applies-to Doc_ Type] = 2
	   AND ia.CreatedBy IN (SELECT dg.QueName 
					    FROM dbo.RSRpt_Group dg JOIN dbo.RSRpt_User qu ON dg.QueNum = qu.QueNum 
					    WHERE dg.QueNum BETWEEN 1 AND 12
					    AND  qu.userid = @UserIDTEMP)
	   AND [Applies-to Doc_ No_] <> ''
	   AND [Applies-to Doc_ No_] = @PPINumber
    GROUP BY ia.CreatedBy,
	   [Applies-to Doc_ No_],
	   [Applies-to Doc_ Type],
	   cmh.No_,
	   [Posting Description],
	   [Vendor Cr_ Memo No_],
    cmh.[Posting Date]

    DROP TABLE #AMOUNT
    DROP TABLE #INV_NUM	
END;

GO


