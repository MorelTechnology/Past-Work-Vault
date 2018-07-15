USE [DataHaven_for_Nav]
GO

/****** Object:  StoredProcedure [dbo].[rpt_ExpenseLimit]    Script Date: 02/03/2016 07:41:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.rpt_ExpenseLimit','P') IS NOT NULL
    DROP PROCEDURE dbo.rpt_ExpenseLimit;
go

Create PROCEDURE [dbo].[rpt_ExpenseLimit]
	
AS


-- ================================================
 /*STORED PROCEDURE                                                                                  
  *  ==============                                                                                  
  *                                                                                                    
  *    ---Datahaven Expense Limit Report---                                                                *
  *                                                                                                 
  *                                                                                               
  *  DESCRIPTION                                                                                       
  *  ===========                                                                                       
  *                                                                                                    
  * ---This sp is used to produces the Expense Limit Report from Datahaven                               
  *                                                                                                    
  *                                                                                                    
  *                                                                                                    
  *  USAGE                                                                                             
  *  =====                                                                                             
  *                                                                                                    
  *  This will be used to generate an SSRS report that provides ExpenseLimits by user in Datahaven                      
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
  *     8/16/2013    1.0            Steve Carignan         Origianl Version                               
  *                                                                                                    

*/
BEGIN
SET NOCOUNT ON;

    CREATE TABLE #AG_BUILD
    (
    rule_order INT,
    Name VARCHAR(50),
    ExpenseLimit MONEY,
    LastModDate DATE
    )

    INSERT INTO #AG_BUILD
    SELECT rule_order,
    LEFT(rule_short_name,PATINDEX('%-%',rule_short_name) - 2)AS Name,
    CASE  
    WHEN rule_short_name LIKE '%<=%' THEN RIGHT(rule_short_name,LEN(rule_Short_name)-PATINDEX('%<=%',rule_short_name)-3)
    WHEN  rule_short_name LIKE'>=%'THEN RIGHT(rule_short_name,LEN(rule_Short_name)-PATINDEX('%>=%',rule_short_name)-3)
    WHEN rule_short_name LIKE '%<%' THEN RIGHT(rule_short_name,LEN(rule_Short_name)-PATINDEX('%<%',rule_short_name)-3)
    WHEN rule_short_name LIKE '%>%' THEN RIGHT(rule_short_name,LEN(rule_Short_name)-PATINDEX('%>%',rule_short_name)-2)
    ELSE ''
    END AS ExpenseLimit,
    CONVERT(DATE,last_mod_date)LastModDate
    FROM manage_nav_autoroute m
    JOIN dbo.workflow_queue wfq ON wfq.que_id = m.route_if_true
    JOIN dbo.queue_types qt ON qt.qt_id = wfq.que_type

    CREATE TABLE #AG_BUILD2
    (
    Name VARCHAR(50),
    ExpenseLimit MONEY,
    LastModDate DATE
    )

    INSERT INTO #AG_BUILD2
    SELECT Name,
    CONVERT(MONEY,MIN(ExpenseLimit)) AS ExpenseLimit,
    MAX(LastModDate)AS LastModDate
    FROM #AG_BUILD
    GROUP BY Name

    UPDATE #AG_BUILD2
    SET ExpenseLimit = '100,000,000,000'
    WHERE Name = 'Nick Bentley'

    SELECT * FROM #AG_BUILD2
    ORDER BY Name

    DROP TABLE #AG_BUILD
    DROP TABLE #AG_BUILD2

END


GO


