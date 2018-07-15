USE [RS_ODS]
GO

/****** Object:  StoredProcedure [dbo].[usp_SapiensClaimFeedBalanceValidation]    Script Date: 5/2/2017 11:05:40 AM

Author : Roja Kasula 
Create Date : May 01 ,2017

******/
DROP PROCEDURE [dbo].[usp_SapiensClaimFeedBalanceValidation]
GO

/****** Object:  StoredProcedure [dbo].[usp_SapiensClaimFeedBalanceValidation]    Script Date: 5/2/2017 11:05:40 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ------------------------------------------------------------------
--  To Be used in Conjunction with Balance Claim Feed.xlsx 
--  ------------------------------------------------------------------

CREATE Procedure [dbo].[usp_SapiensClaimFeedBalanceValidation] 
(
@ResultOutput VARCHAR(MAX) Output,
@ResultOutPutMessage VARCHAR(MAX) Output

)
AS
BEGIN

/*
Drop Temporary table 
*/
if object_id('tempdb..##Tmp_FinancialTransactions') is not null
    drop table ##Tmp_FinancialTransactions
if object_id('tempdb..##Tmp_CaseSummary') is not null
    drop table ##Tmp_CaseSummary
if object_id('tempdb..##Tmp_TransactionCode') is not null
    drop table ##Tmp_TransactionCode
if object_id('tempdb..##Tmp_Variance') is not null
    drop table ##Tmp_Variance
/**/

DECLARE @Hd VARCHAR(MAX) ,@body VARCHAR(MAX),@FinancialTransactions VARCHAR(MAX), @CaseSummary VARCHAR(MAX),@TransactionCode VARCHAR(MAX),@Varaiance VARCHAR(MAX)
Declare @valdt Date ,@lastrundt DATE,@rundt Date
select @valdt = EOMONTH(GETDATE()) ,@lastrundt= EOMONTH (GETDATE(), -1), @rundt = GetDate()

--select @valdt = '03/31/2017' ,@lastrundt= '02/28/2017', @rundt = GetDate() 


---select @valdt = '04/30/2017' ,@lastrundt= '03/31/2017', @rundt = GetDate() 
--  Copy this total to line 3 columns a-h in spreadsheet
--  --------------------------------------'04/30/2017'----------------------------
SELECT 
    SUM(Paid_Adjusting_Expense)             AS pdadjexp, 
    SUM(Paid_Adjusting_Expense_InLimits)    AS inlim, 
    SUM(Paid_Coverage_DJ_Expense)           AS pddj, 
    SUM(Paid_Loss)                          AS pdloss,
    SUM(Recovery_Subrogation_Loss)          AS subloss,
    SUM(Recovery_Deductible_Loss)           AS recovded,
    SUM(Recovery_Salavage)                  AS recovsalv, 
    SUM(Recovery_Subrogation_Expense)       AS subexp
  --,SUM(Case_Adjusting_Expense) AS caseexp, SUM(Case_Coverage_DJ_Expense) AS djexp, SUM(Case_Loss) AS csloss--, Transaction_Source_Cd--, CAST(ft.Sys_Create_Dt AS DATE)
  --ft.Portfolio_Cd
 Into ##Tmp_FinancialTransactions 
FROM 
  trn.Financial_Transaction AS ft
  
  --  Get unique list of exposures on transactions which have been created since last rundate
  JOIN 
    ( SELECT    Exposure_Id 
      FROM      trn.Financial_Transaction 
      WHERE     Sys_Create_Dt > @lastrundt
		  GROUP BY  Exposure_Id 
    )  AS ft2 
  ON ft.Exposure_Id = ft2.Exposure_Id

  --  Get Exposure related to Xact
  JOIN  clm.Exposure AS e     
  ON  ft.ExposureSK   = e.ExposureSK
  
  --  Get Claims related to Exposures
  JOIN  clm.Claim AS c        
  ON  e.ClaimSK       = c.ClaimSK

  --  Join to Claim Exclusions table
  LEFT JOIN rein.Sapiens_ClaimFeedExclusion AS scfe 
  ON  c.Claim_No      = scfe.Claim_No 
  AND e.Exposure_No   = COALESCE(scfe.Exposure_No, e.Exposure_No)

  --  Get PolicyPeriod related to Exposures
  JOIN  pol.Policy_Period   AS pp 
  ON  e.Policy_Period_ID  = pp.Policy_Period_Id 
  AND e.PolicyVersion     = pp.PolicyVersion

  --  Join to Policy Exclusions table
  LEFT JOIN ref.Rein_Policy_Exclusion AS rpe   
  ON  pp.Policy_No              = rpe.Policy_No 
  AND pp.Policy_Effective_Dt    = rpe.Policy_Effective_Dt 
  AND pp.Portfolio_Cd           = rpe.Portfolio_Cd

  --  LEFT JOIN rein.Sapiens_HistoricalFeed_finder AS shff 
  --  ON c.Claim_No = shff.Claim_No
 JOIN ref.PortfolioFilter AS pf
	 ON pf.Portfolio_Cd = pp.Portfolio_Cd 
	 AND GETDATE() BETWEEN pf.rowActivefromdate 
	 AND pf.rowActiveEndDate 
	 AND pf.FeedIncExc = 1
JOIN ref.DataFeed AS df 
	ON df.FeedID = pf.FeedID 
AND df.FeedName = 'Sapiens'

WHERE 
    scfe.Claim_No             IS NULL       --  omit claims which are in the exclusions table
    --ft.Valuation_Dt         = @valdt
    AND ft.Sys_Create_Dt    BETWEEN  @lastrundt AND @valdt --  Create since lastrundate  
    --AND shff.Claim_No       IS NULL
    AND rpe.Policy_No         IS NULL       --  omit claims related to policies in the policy exclusion table
    AND pp.Policy_No          NOT LIKE 'Z%' --  omit claims related to policies shells
	--AND ft.Portfolio_Cd != 'MMK'


----  ------------------------------------------------------------------
----  copy these totals to line 3 columns j-k 
----  ------------------------------------------------------------------

SELECT 
    SUM(Case_Adjusting_Expense)     AS caseexp, 
    SUM(Case_Coverage_DJ_Expense)   AS djexp, 
    SUM(Case_Loss)                  AS csloss
Into ##Tmp_CaseSummary
FROM
  trn.Financial_Transaction AS ft

  --  Get unique list of exposures on transactions which are assigned to the specified Valuation date
  JOIN 
    ( SELECT    Exposure_Id 
      FROM      trn.Financial_Transaction 
      WHERE     Valuation_Dt = @valdt           --Sys_Create_Dt >@closedt 
      GROUP BY  Exposure_Id 
    )  AS ft2 
  ON  ft.Exposure_Id = ft2.Exposure_Id
  
  --  Current Exposure Rows related to Xacts
  JOIN clm.Exposure AS e 
  ON    ft2.Exposure_Id   = e.Exposure_ID 
  AND   e.Sys_RowEndDt    = '12/31/9999'
  
  --  Get PolicyPeriods related to Exposures with Xacts
  LEFT JOIN pol.Policy_Period AS pp 
  ON  e.Policy_Period_ID  = pp.Policy_Period_Id 
  AND e.PolicyVersion     = pp.PolicyVersion
  
  --  Get Claims related to Exposures
  LEFT JOIN clm.Claim AS c 
  ON  e.ClaimSK           = c.ClaimSK
  
  --  Join to Claim Exclusions table 
  LEFT JOIN rein.Sapiens_ClaimFeedExclusion AS scfe 
  ON  c.Claim_No          = scfe.Claim_No 
  AND e.Exposure_No       = COALESCE(scfe.Exposure_No, e.Exposure_No)
  
  --  Join to Policy Exclusions table
  LEFT JOIN ref.Rein_Policy_Exclusion AS rpe   
  ON  pp.Policy_No            = rpe.Policy_No 
  AND pp.Policy_Effective_Dt  = rpe.Policy_Effective_Dt 
  AND pp.Portfolio_Cd         = rpe.Portfolio_Cd

 JOIN ref.PortfolioFilter AS pf
	 ON pf.Portfolio_Cd = pp.Portfolio_Cd 
	 AND GETDATE() BETWEEN pf.rowActivefromdate 
	 AND pf.rowActiveEndDate 
	 AND pf.FeedIncExc = 1
JOIN ref.DataFeed AS df 
	ON df.FeedID = pf.FeedID 
	AND df.FeedName = 'Sapiens'
  
  --LEFT JOIN rein.Sapiens_HistoricalFeed_finder AS shff ON c.Claim_No = shff.Claim_No

WHERE 
   -- ft.Sys_Create_Dt    BETWEEN  @lastrundt AND @valdt      --  omit transactions with valuation date more recent than Val Date
	ft.Valuation_Dt                 !> @valdt 
    AND ft.Transaction_Category_Cd  = 'RESCHG'      --  include only RESCHG transactions
    AND pp.Policy_No                NOT LIKE 'Z%'   --  omit claims related to policies shells
    AND scfe.Claim_No               IS NULL         --   omit claims which are in the exclusions table
    AND rpe.Policy_No               IS NULL         --  omit claims related to policies in the policy exclusion table
		----AND ft.Portfolio_Cd != 'MMK'
		----  AND shff.Claim_No               IS NULL

----  ------------------------------------------------------------------
----  These totals you need to match by trancode.  
----  Most of the time the trancodes in the spreadsheet are all we get but if there are more you 
----  need to find the correct bucket for comparison  see below for buckets I know of
----  ------------------------------------------------------------------

SELECT 
      Transaction_Cd, 
      SUM(COALESCE(Transaction_Amnt,0)) AS Transaction_Amnt, 
      COUNT(*) Numberofrows
Into ##Tmp_TransactionCode
FROM 
      rein.Sapiens_CLaim_Feed AS sclf
GROUP BY 
      Transaction_Cd
ORDER BY 
      sclf.Transaction_Cd; 


--select * from ##Tmp_FinancialTransactions

--select * from ##Tmp_CaseSummary

--select * from ##Tmp_TransactionCode


;With CTE1
AS
(
select SUM(Transaction_Amnt) EDJ_ECO_TotalAmt
FROm ##Tmp_TransactionCode
Where Transaction_Cd in ('EDJ','ECO')
)
,CTE2  
AS
(
select SUM(Transaction_Amnt) EOL_TotalAmt
FROm ##Tmp_TransactionCode
Where Transaction_Cd in ('EOL')
)
,CTE3 
AS
(
select SUM(Transaction_Amnt) EWL_TotalAmt
FROm ##Tmp_TransactionCode
Where Transaction_Cd in ('EWL')
)
,CTE4
AS
(
select SUM(Transaction_Amnt) IDM_TotalAmt
FROm ##Tmp_TransactionCode
Where Transaction_Cd in ('IDM')
)

,CTE5
AS
(
select SUM(Transaction_Amnt) ODJ_TotalAmt
FROm ##Tmp_TransactionCode
Where Transaction_Cd in ('ODJ')
)

,CTE6
AS
(
select SUM(Transaction_Amnt) ODM_TotalAmt
FROm ##Tmp_TransactionCode
Where Transaction_Cd in ('ODM')
)
,CTE7
AS
(
select SUM(Transaction_Amnt) OOL_TotalAmt
FROm ##Tmp_TransactionCode
Where Transaction_Cd in ('OOL')
)
,CTE8
AS
(
Select ( EDJ_ECO_TotalAmt - pddj ) ECO_EDJ_Varaince
FROM CTE1
Cross Apply ##Tmp_FinancialTransactions
 ) 
,
CTE9
AS 
(
Select ( EOL_TotalAmt - pdadjexp - subexp ) EOL_Variance
FROM CTE2
Cross Apply ##Tmp_FinancialTransactions
) 
,
CTE10 
AS
(
Select ( EWL_TotalAmt - inlim ) EWL_Variance
FROM CTE3
Cross Apply ##Tmp_FinancialTransactions
) 
,
CTE11 AS
(
Select (pdloss + subloss + recovded + recovsalv - IDM_TotalAmt) IDM_Variance
FROM CTE4
Cross Apply ##Tmp_FinancialTransactions

)

,CTE12 AS 
(
Select (ODJ_TotalAmt - djexp ) ODJ_Variance

FROM CTE5
Cross Apply ##Tmp_CaseSummary
)
,
CTE13 AS
(
Select (ODJ_TotalAmt - djexp ) ODM_Variance
FROM CTE5
Cross Apply ##Tmp_CaseSummary
)
,
CTE14 AS
(
Select (ODM_TotalAmt - csloss ) OOL_Variance
FROM CTE6
Cross Apply ##Tmp_CaseSummary
) 

SELECT * INTO ##Tmp_Variance
FROM CTE8 ,CTE9,CTE10,CTE11,CTE12,CTE13,CTE14

SET @Hd =
     CAST(( 
        SELECT @valdt AS 'td',''
              ,@rundt As 'td',''
			  ,@lastrundt As 'td'
	    FOR XML PATH('tr'),ELEMENTS ) AS VARCHAR(MAX));


SET @FinancialTransactions =
      CAST((
        SELECT isnull(d.inlim,0) AS 'td',''
              ,isnull(d.pdadjexp,0) AS 'td',''
              ,isnull(d.pddj,0) AS 'td',''
              ,isnull(d.pdloss,0) AS 'td',''
              ,isnull(d.recovded,0) AS 'td',''
              ,isnull(d.recovsalv,0) AS 'td',''
              ,isnull(d.subexp,0) AS 'td',''
              ,isnull(d.subloss,0)  AS 'td',''
	    FROM ##Tmp_FinancialTransactions AS d
		   FOR XML PATH('tr'),ELEMENTS ) AS VARCHAR(MAX));

SET @CaseSummary=   CAST((SELECT isnull(d.caseexp,0) AS 'td',''
              ,isnull(d.csloss,0) AS 'td',''
              ,isnull( d.djexp,0) AS 'td',''
        FROM ##Tmp_CaseSummary AS d
         FOR XML PATH('tr'),ELEMENTS ) AS VARCHAR(MAX));

SET @TransactionCode = 
 CAST((
		SELECT d.Transaction_Cd AS 'td','' 
              ,d.Transaction_Amnt AS 'td',''
              ,d.Numberofrows AS 'td',''
        FROM ##Tmp_TransactionCode AS d
        FOR XML PATH('tr'),ELEMENTS ) AS VARCHAR(MAX));

SET @Varaiance =  CAST(( SELECT ECO_EDJ_Varaince AS 'td',''  
        ,EOL_Variance AS 'td','' 
        ,EWL_Variance AS 'td','' 
		,IDM_Variance AS 'td','' 
		,ODJ_Variance AS 'td','' 
		,ODM_Variance AS 'td','' 
		,OOL_Variance AS 'td','' 
		FROM ##Tmp_Variance 
		  FOR XML PATH('tr'),ELEMENTS ) AS VARCHAR(MAX));

SET  @body = '<html><body><H3>Sapiens Claim Feed Balance Validation Results</H3> ' +
' <table border = 1> <tr><th colspan="3" align="left" bgcolor="lightblue"><b>Dates Information</b></th></tr>
		<tr> <th> Valuation Date </th> <th> Run Date </th> <th> Last Run Date </th> </tr> ' + @Hd + 
' </table><br><table border = 1> <tr><th colspan="8" align="left" bgcolor="lightblue"><b>Transaction Summary </b></th></tr> <tr> <th> PaidAdjustingExpense </th> <th> Paid_Adjusting_Expense_InLimits </th> <th> Paid_Coverage_DJ_Expense </th><th> Paid_Loss </th> <th> Recovery_Subrogation_Loss </th><th> Recovery_Deductible_Loss </th> <th> Recovery_Salavage </th> <th> Recovery_Subrogation_Expense </th> </tr>'  + @FinancialTransactions +
' </table><br><table border = 1><tr><th colspan="3" align="left" bgcolor="lightblue"><b>Case Summary </b></th></tr> <tr> <th> Case_Adjusting_Expense </th> <th> Case_Coverage_DJ_Expense </th><th> Case_Loss </th> </tr> ' + @CaseSummary + 
' </table><br><table border = 1><tr><th colspan="3" align="left" bgcolor="lightblue"><b>Transaction Codes </b></th></tr> <tr> <th> Transaction_Cd </th> <th> Transaction_Amnt </th> <th> Numberofrows </th> </tr> ' + @TransactionCode +
' </table><br><table border = 1><tr><th colspan="7" align="left" bgcolor="lightblue"><b>Variance Summary </b></th></tr> <tr> <th>ECO_EDJ_Varaince</th><th> EOL_Variance </th> <th> EWL_Variance </th> <th> IDM_Variance </th> <th> ODJ_Variance </th> <th> ODM_Variance </th> <th> OOL_Variance </th> </tr> ' + @Varaiance 


SET @body = @body  + '</table></body></html>'



Select * FROm ##Tmp_Variance
SELECT  @ResultOutput= CASE WHEN (
 ECO_EDJ_Varaince  
+EOL_Variance
+EWL_Variance
+IDM_Variance
+ODJ_Variance
+ODM_Variance
+OOL_Variance ) = 0 Then 
'Sapiens Claim Feed Balance Validation Successful' 
ELSE 
'Sapiens Claim Feed Balance Validation Failed'
END  
,@ResultOutPutMessage=  @body  
FROM ##Tmp_Variance
RETURN 100;
END





GO


