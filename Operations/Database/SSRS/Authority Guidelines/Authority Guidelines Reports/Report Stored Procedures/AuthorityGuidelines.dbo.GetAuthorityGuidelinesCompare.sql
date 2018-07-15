USE [AuthorityGuidelines]
GO

/****** Object:  StoredProcedure [dbo].[rpt_GetAuthorityGuideLinesCompare]    Script Date: 02/04/2016 08:05:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Steve Carignan
-- Create date: 7/15/2014
-- Description:	Gets Authority Guideline 
-- =============================================
CREATE PROCEDURE [dbo].[rpt_GetAuthorityGuideLinesCompare]
	-- Add the parameters for the stored procedure here
	
	@InputDate DATETIME = NULL
AS
BEGIN
--	-- SET NOCOUNT ON added to prevent extra result sets from
--	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF @InputDate IS NULL SET @InputDate = GetDate()

    --DECLARE @InputDate DATE
    --SET @InputDate = '8/11/2014'
    
  CREATE TABLE #CC_Pay
  (AssociateName VARCHAR(75),
  PayLimit DECIMAL(18,2),
  WMLimit DECIMAL (18,2),
   )
INSERT INTO  #CC_Pay
(AssociateName,
WMLimit,
PayLimit

)

SELECT	C.[Lastname] + ', ' + C.[FirstName] AS Associate
       	,CAST(WMLimit AS MONEY) AS WMLimit
		,CAST(AL.[LimitAmount] AS MONEY) as PayLimit
		--,AL.[CostType]
		--,AP.[Name] as [Profile]
		--,CR.Active
		--,CR.[UserName]
		--,C.[FirstName]
		--,C.[Lastname]
		--,U.[Jobtitle]
		--,CR.[PublicID]
        --,U.[Department]
		--,U.[trg_UserLocation]

 FROM [ClaimCenter].[ClaimCenter].[dbo].[cc_authoritylimit] AL  
 join  [ClaimCenter].[ClaimCenter].[dbo].[cc_user] U on AL.ProfileID = U.AuthorityProfileID 
 Join  [ClaimCenter].[ClaimCenter].[dbo].[cc_authorityprofile] AP on U.AuthorityProfileID = AP.ID 
 Join  [ClaimCenter].[ClaimCenter].[dbo].[cc_credential] CR on U.CredentialID = CR.ID  
 join  [ClaimCenter].[ClaimCenter].[dbo].[cc_contact] C on U.ContactID = C.ID 
 LEFT JOIN (SELECT ltrim(rtrim(ALT.[LimitAmount])) as WMLIMIT
		,ALT.PROFILEID
FROM 	[ClaimCenter].[ClaimCenter].[dbo].[cc_authoritylimit] ALT	
WHERE ALT.[LimitType] = 9) WM on WM.ProfileID = AL.ProfileID
WHERE AL.[LimitType] = 6
Order by C.[Lastname], C.[FirstName],AL.LimitType
  
 --select * from #CC_Pay
 --drop table #CC_Pay

--DatahavenExpenseLimits--
  
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
FROM Datahaven_for_NAV.dbo.manage_nav_autoroute m
JOIN Datahaven_for_NAV.dbo.workflow_queue wfq ON wfq.que_id = m.route_if_true
JOIN Datahaven_for_NAV.dbo.queue_types qt ON qt.qt_id = wfq.que_type

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
SET ExpenseLimit = '999999999'
WHERE Name = 'Nick Bentley'

UPDATE #AG_BUILD2
SET Name = 'Weikers, Ann'
WHERE Name = 'Ann Weikers'

UPDATE #AG_BUILD2
SET Name = 'Shakow, Amy'
WHERE Name = 'Amy Shakow'

UPDATE #AG_BUILD2
SET Name = 'Kant, Bob'
WHERE Name = 'Bob Kant'

UPDATE #AG_BUILD2
SET Name = 'Sampson, Robert'
WHERE Name = 'Bob Sampson'

UPDATE #AG_BUILD2
SET Name = 'Van Hirtum, Brenda'
WHERE Name = 'Brenda Van Hirtum'

UPDATE #AG_BUILD2
SET Name = 'Clate, Brian'
WHERE Name = 'Brian Clate'

UPDATE #AG_BUILD2
SET Name = 'Baldwin, Carol'
WHERE Name = 'Carol Baldwin'

UPDATE #AG_BUILD2
SET Name = 'Brown, Craig'
WHERE Name = 'Craig Brown'

UPDATE #AG_BUILD2
SET Name = 'DeMaria, Frank'
WHERE Name = 'Frank DeMaria'

UPDATE #AG_BUILD2
SET Name = 'Hinson, Gary'
WHERE Name = 'Gary Hinson'

UPDATE #AG_BUILD2
SET Name = 'Kelly, Jim'
WHERE Name = 'Jim Kelly'

UPDATE #AG_BUILD2
SET Name = 'Zampella, Joe'
WHERE Name = 'Joe Zampella'

UPDATE #AG_BUILD2
SET Name = 'Bator, John'
WHERE Name = 'John Bator'

UPDATE #AG_BUILD2
SET Name = 'Siegart, John'
WHERE Name = 'John Siegart'

UPDATE #AG_BUILD2
SET Name = 'Myers, Karyn'
WHERE Name = 'Karyn Myers'

UPDATE #AG_BUILD2
SET Name = 'Lemire, Maureen'
WHERE Name = 'Maureen Lemire'

UPDATE #AG_BUILD2
SET Name = 'Cairns, Megan'
WHERE Name = 'Megan Cairns'

UPDATE #AG_BUILD2
SET Name = 'Shane, Michael'
WHERE Name = 'Michael Shane'

UPDATE #AG_BUILD2
SET Name = 'Bryant, Michael'
WHERE Name = 'Mike Bryant'

UPDATE #AG_BUILD2
SET Name = 'Westover, Michael'
WHERE Name = 'Mike Westover'

UPDATE #AG_BUILD2
SET Name = 'Caroselli, Nina'
WHERE Name = 'Nina Caroselli'

UPDATE #AG_BUILD2
SET Name = 'Campbell, Renee'
WHERE Name = 'Renee Campbell'

UPDATE #AG_BUILD2
SET Name = 'Fabian, Rich'
WHERE Name = 'Rich Fabian'

UPDATE #AG_BUILD2
SET Name = 'Smith, Sara'
WHERE Name = 'Sara Smith'

UPDATE #AG_BUILD2
SET Name = 'Scott, Sherryl'
WHERE Name = 'Sherryl Scott'

UPDATE #AG_BUILD2
SET Name = 'Osborne, Stephen'
WHERE Name = 'Stephen Osborne'

UPDATE #AG_BUILD2
SET Name = 'Maienza, Steve'
WHERE Name = 'Steve Maienza'

UPDATE #AG_BUILD2
SET Name = 'Dolon, Tim'
WHERE Name = 'Tim Donlon'


UPDATE #AG_BUILD2
SET Name = 'Townsend, Wilson'
WHERE Name = 'Wilson Townsend'

UPDATE #AG_BUILD2
SET Name = 'Bentley, Nick'
WHERE Name = 'Nick Bentley'

UPDATE #AG_BUILD2
SET Name = 'Kreji, Jim'
WHERE Name = 'Jim Kreji'

UPDATE #AG_BUILD2
SET Name = 'Kunish, Matt'
Where Name = 'Matt Kunish'

UPDATE #AG_BUILD2
SET Name = 'Brown, Kiki'
Where Name = 'Kiki Brown'

UPDATE #AG_BUILD2
SET Name = 'Butchard, Marlene'
Where Name = 'Marlene Butchard'

    
     CREATE TABLE #AGL
     (
     AssociateID INT,
     AssociateName VARCHAR(75),
     AssociateFlg CHAR (1),
     Description VARCHAR(50),
     Type CHAR (1),
     WriteOffs VARCHAR(30),
     WriteOffs2 VARCHAR(30),
     ReinsRelatedDisbursements VARCHAR(30),
     ReinsRelatedDisbursements2 VARCHAR(30),
     Compromise VARCHAR(30),
     Compromise2 VARCHAR(30),
     Commute VARCHAR(30),
     Commute2 VARCHAR(30),
     ExpenseReports VARCHAR(30),
     ExpenseReports2 VARCHAR(30),
     LOCProcurements VARCHAR(30),
     LOCProcurements2 VARCHAR(30),
     PurchaseSVCContracts VARCHAR(30),
     PurchaseSVCContracts2 VARCHAR(30),
     CheckRequestWireApprovals VARCHAR(30),
     CheckRequestWireApprovals2 VARCHAR(30),
     LegalBudgets VARCHAR(30),
     LegalBudgets2 VARCHAR(30),
     CheckRequestWireTransfers VARCHAR(30),
     CheckRequestWireTransfers2 VARCHAR(30),
     PayrollBenefitDisbursements VARCHAR(30),
     PayrollBenefitDisbursements2 VARCHAR(30),
     RentLeasesSuppliesRelated VARCHAR(30),
     RentLeasesSuppliesRelated2 VARCHAR(30),
     ReserveAuthority VARCHAR(30),
     ReserveAuthority2 VARCHAR(30),
     SettlementTransactionalAuthority VARCHAR(30),
     SettlementTransactionalAuthority2 VARCHAR(30),
     ECOXPL VARCHAR(30),
     ECOXPL2 VARCHAR(30),
     GroupID INT
   
   
    )
    INSERT INTO #AGL
    (
     AssociateID,
     AssociateName,
     AssociateFlg,
     Description,
     Type,
     WriteOffs,
     --WriteOffs2,
     ReinsRelatedDisbursements,
     --ReinsRelatedDisbursements2,
     Compromise,
     --Compromise2,
     Commute,
     --Commute2,
     ExpenseReports,
     --ExpenseReports2,
     LOCProcurements,
     --LOCProcurements2,
     PurchaseSVCContracts,
     --PurchaseSVCContracts2,
     CheckRequestWireApprovals,
     --CheckRequestWireApprovals2,
     LegalBudgets,
     --LegalBudgets2,
     CheckRequestWireTransfers,
     --CheckRequestWireTransfers2,
     PayrollBenefitDisbursements,
     --PayrollBenefitDisbursements2,
     RentLeasesSuppliesRelated,
     --RentLeasesSuppliesRelated2,
     ReserveAuthority,
     --ReserveAuthority2,
     SettlementTransactionalAuthority,
     --SettlementTransactionalAuthority2,
     ECOXPL,
     --ECOXPL2,
     GroupID
   
    
        )
        
    SELECT			[AssociateID],
					[Name],
					[AssociateFlg],
					[Description],
					[Type],
					CASE [4] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [4]), 1), '.00', '') END AS [WriteOffs],
					CASE [5] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [5]), 1), '.00', '') END AS [ReinsRelatedDisbursements],
					CASE [6] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [6]), 1), '.00', '') END AS [Compromise],
					CASE [7] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [7]), 1), '.00', '') END AS [Commute],
					CASE [8] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [8]), 1), '.00', '') END AS [ExpenseReports],
					CASE [9] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [9]), 1), '.00', '') END AS [LOCProcurements],
					CASE [10] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [10]), 1), '.00', '') END AS [PurchaseSVCContracts],
					CASE [11] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [11]), 1), '.00', '') END AS [CheckRequestWireApprovals],
					CASE [12] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [12]), 1), '.00', '') END AS [LegalBudgets],
					CASE [13] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [13]), 1), '.00', '') END AS [CheckRequestWireTransfers],
					CASE [14] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [14]), 1), '.00', '') END AS [PayrollBenefitDisbursements],
					CASE [15] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [15]), 1), '.00', '') END AS [RentLeasesSuppliesRelated],
					CASE [1] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [1]), 1), '.00', '') END AS [ReserveAuthority],
					CASE [2] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [2]), 1), '.00', '') END AS [SettlementTransactionalAuthority],
					CASE [3] WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [3]), 1), '.00', '') END AS [ECOXPL],
					[GroupID]
				    
					
	FROM			(
		SELECT			[A].[AssociateID],
						[A].[LastName] + ', ' + [A].[FirstName] AS [Name],
						[A].AssociateFlg,
						[D].[Description],
						[AL].[Type],
						[A].[GroupID],
						[AL].[LimitID],
						[AL].[Limit] AS Limit
		FROM			[AG].[Associate] [A]
		JOIN			[AG].[Associate_Limit] [AL]
			ON			[AL].[AssociateID] = [A].[AssociateID]
			AND			@InputDate BETWEEN [AL].[StartDate] AND ISNULL([AL].[EndDate], GetDate())
		LEFT JOIN		[AG].[Department] [D]
			ON			[D].[DepartmentID] = [A].[DepartmentID]
			AND			@InputDate BETWEEN [D].[StartDate] AND ISNULL([D].[EndDate], GetDate())
		WHERE			@InputDate BETWEEN [A].[StartDate] AND ISNULL([A].[EndDate], GetDate())
		AND AL.Limit <> 0
		AND AL.Type = 'R'
		GROUP BY		[A].[AssociateID],
                        [A].[LastName] + ', ' + [A].[FirstName],
                        AssociateFlg,
                        [D].[Description],
                        [AL].[Type],
                        [A].[GroupID],
						[AL].[LimitID],
						[AL].[Limit] 
					) AS SourceTable
		PIVOT		(
						SUM([Limit])
						FOR [LimitID] IN ([4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[1],[2],[3])
					) AS PivotTable
		ORDER BY	[GroupID],
                    [Name],
                    [Type] DESC
    
	
				
				
UPDATE #AGL
SET [ReserveAuthority]= ''
Where [ReserveAuthority] IS NULL

UPDATE #AGL
SET [ReserveAuthority] = '999999999'
WHERE AssociateName = 'Bentley, Nick'

UPDATE #AGL
SET SettlementTransactionalAuthority = ''
WHERE SettlementTransactionalAuthority IS NULL

UPDATE #AGL
SET SettlementTransactionalAuthority = '999999999'
WHERE AssociateName = 'Bentley, Nick'

UPDATE #AGL
SET ECOXPL = ''
WHERE ECOXPL IS NULL


UPDATE #AGL
SET Writeoffs = ''
WHERE Writeoffs IS NULL


UPDATE #AGL
SET ReinsRelatedDisbursements = ''
WHERE ReinsRelatedDisbursements IS NULL

UPDATE #AGL
SET Compromise = ''
WHERE Compromise IS NULL

UPDATE #AGL
SET Commute = ''
WHERE Commute IS NULL

UPDATE #AGL 
SET ExpenseReports = ''
WHERE ExpenseReports IS NULL


UPDATE #AGL 
SET LOCProcurements = ''
WHERE LOCProcurements IS NULL

UPDATE #AGL
SET PurchaseSvcContracts = ''
WHERE PurchaseSvcContracts IS NULL


UPDATE #AGL
SET CheckRequestWireApprovals = ''
WHERE CheckRequestWireApprovals IS NULL

UPDATE #AGL
SET CheckRequestWireApprovals = '999999999'
WHERE AssociateName = 'Bentley, Nick'

UPDATE #AGL
SET CheckRequestWireApprovals = ''
WHERE AssociateName = 'Giangregorio, Larry'

UPDATE #AGL
SET CheckRequestWireApprovals = ''
WHERE AssociateName = 'Jacques, Kristina'

UPDATE #AGL
SET CheckRequestWireApprovals = ''
WHERE AssociateName = 'Jervey, Ed'

UPDATE #AGL
SET CheckRequestWireApprovals = ''
WHERE AssociateName = 'Vance, Marc'




UPDATE #AGL 
SET LegalBudgets = ''
WHERE LegalBudgets IS NULL

UPDATE #AGL
SET CheckRequestWireTransfers = ''
WHERE CheckRequestWireTransfers IS NULL

UPDATE #AGL
SET PayrollBenefitDisbursements = ''
WHERE PayrollBenefitDisbursements IS NULL

			

SELECT
#AGL.AssociateID ,
#AGL.AssociateName, 
#AGL.AssociateFlg,
#AGL.Description,
#AGL.[Type],
--CASE #AGL.[ReserveAuthority] WHEN '0.00' THEN NULL ELSE #AGL.[ReserveAuthority] END AS [ReserveAuthority] ,
--CASE #AGL.[SettlementTransactionalAuthority] WHEN '0.00' THEN NULL ELSE #AGL.[SettlementTransactionalAuthority] END AS [SettlementTransactionalAuthority],
#AGL.[ReserveAuthority],
#AGL.[SettlementTransactionalAuthority],
#AGL.[ECOXPL],
#AGL.Writeoffs,
#AGL.ReinsRelatedDisbursements,
#AGL.Compromise,
#AGL.Commute,
#AGL.ExpenseReports,
#AGL.LOCProcurements,
#AGL.PurchaseSvcContracts,
--CASE #AGL.CheckRequestWireApprovals WHEN '0.00' THEN NULL ELSE #AGL.CheckRequestWireApprovals END AS CheckRequestWireApprovals,
#AGL.CheckRequestWireApprovals,
#AGL.LegalBudgets,
#AGL.CheckRequestWireTransfers,
#AGL.PayrollBenefitDisbursements,
#AGL.RentLeasesSuppliesRelated,
CASE
WHEN #CC_Pay.PayLimit IS NULL
THEN 0.00
ELSE #CC_Pay.PayLimit
END AS PayLimit,
CASE
WHEN WMLimit IS NULL
THEN '0.00'
ELSE WMLimit
END AS WM_Limit,
CASE
WHEN #AG_BUILD2.ExpenseLimit IS NULL
THEN '0.00'
ELSE #AG_BUILD2.ExpenseLimit
END AS ExpenseLimit
FROM #AGL
LEFT JOIN #CC_Pay
ON #AGL.AssociateName = #CC_Pay.AssociateName
LEFT JOIN #AG_BUILD2
ON #AGL.AssociateName = #AG_BUILD2.Name
--WHERE #AGL.groupid = 1
--WHERE #AGL.Limit <> '0'
--and Type = 'R'

GROUP BY
#AGL.[AssociateID],
#AGL.[AssociateName], 
#AGL.AssociateFlg,
#AGL.[Description],
#AGL.[Type],
#AGL.[ReserveAuthority],
#AGL.[SettlementTransactionalAuthority],
#AGL.[ECOXPL],
#AGL.[Writeoffs],
#AGL.[ReinsRelatedDisbursements],
#AGL.[Compromise],
#AGL.[Commute],
#AGL.[ExpenseReports],
#AGL.[LOCProcurements],
#AGL.[PurchaseSvcContracts],
#AGL.CheckRequestWireApprovals,
#AGL.[LegalBudgets],
#AGL.[CheckRequestWireTransfers],
#AGL.[PayrollBenefitDisbursements],
#AGL.[RentLeasesSuppliesRelated],
#CC_Pay.PayLimit,
#CC_Pay.WMLimit,
ExpenseLimit
ORDER BY #AGL.[AssociateName]
--#AGL.[Type] DESC



 
DROP TABLE #AGL
DROP TABLE #CC_Pay
DROP TABLE #AG_BUILD
DROP TABLE #AG_BUILD2

END
 


grant execute on [dbo].[rpt_GetAuthorityGuideLinesCompare] to rl_execproc
GO


