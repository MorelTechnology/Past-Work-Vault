USE [AuthorityGuidelines]
GO

/****** Object:  StoredProcedure [dbo].[rpt_Sr_Mgmnt]    Script Date: 02/04/2016 08:03:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Steve Carignan
-- Create date: 7/15/2014
-- Description:	Gets Authority Guideline Group 1 Items
-- =============================================
CREATE PROCEDURE [dbo].[rpt_Sr_Mgmnt]
	-- Add the parameters for the stored procedure here
	
	@InputDate DATETIME = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF @InputDate IS NULL SET @InputDate = GetDate()

    IF CAST(GETDATE() AS date) = CAST(@InputDate AS date)
	SET @InputDate = DATEADD(minute, -1, GetDate())
    
     CREATE TABLE #AGL
     (
     AssociateID INT,
     AssociateName VARCHAR(75),
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
     Description,
     Type,
     WriteOffs,
    ReinsRelatedDisbursements,
    Compromise,
    Commute,
    ExpenseReports,
    LOCProcurements,
    PurchaseSVCContracts,
    CheckRequestWireApprovals,
    LegalBudgets,
    CheckRequestWireTransfers,
    PayrollBenefitDisbursements,
    RentLeasesSuppliesRelated,
    ReserveAuthority,
    SettlementTransactionalAuthority,
    ECOXPL,
    GroupID
 
     )
     
SELECT			[AssociateID],
					[Name],
					[Description],
					[Type],
					CASE [4] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [4]), 1), '.00', '') END AS [WriteOffs],
					CASE [5] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [5]), 1), '.00', '') END AS [ReinsRelatedDisbursements],
					CASE [6] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [6]), 1), '.00', '') END AS [Compromise],
					CASE [7] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [7]), 1), '.00', '') END AS [Commute],
					CASE [8] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [8]), 1), '.00', '') END AS [ExpenseReports],
					CASE [9] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [9]), 1), '.00', '') END AS [LOCProcurements],
					CASE [10] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [10]), 1), '.00', '') END AS [PurchaseSVCContracts],
					CASE [11] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [11]), 1), '.00', '') END AS [CheckRequestWireApprovals],
					CASE [12] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [12]), 1), '.00', '') END AS [LegalBudgets],
					CASE [13] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [13]), 1), '.00', '') END AS [CheckRequestWireTransfers],
					CASE [14] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [14]), 1), '.00', '') END AS [PayrollBenefitDisbursements],
					CASE [15] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [15]), 1), '.00', '') END AS [RentLeasesSuppliesRelated],
					CASE [1] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [1]), 1), '.00', '') END AS [ReserveAuthority],
					CASE [2] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [2]), 1), '.00', '') END AS [SettlementTransactionalAuthority],
					CASE [3] WHEN 0 then '' WHEN -1 THEN 'Unlimited' WHEN -2 THEN 'Policy_Limits' WHEN -3 THEN 'DH_Approver' ELSE REPLACE(CONVERT(varchar, CONVERT(money, [3]), 1), '.00', '') END AS [ECOXPL],
					[GroupID]
	FROM			(
		SELECT			[A].[AssociateID],
						[A].[LastName] + ', ' + [A].[FirstName] AS [Name],
						[D].[Description],
						[AL].[Type],
						[A].[GroupID],
						[AL].[LimitID],
						[AL].[Limit]
		FROM			[AG].[Associate] [A]
		JOIN			[AG].[Associate_Limit] [AL]
			ON			[AL].[AssociateID] = [A].[AssociateID]
			AND			@InputDate BETWEEN [AL].[StartDate] AND ISNULL([AL].[EndDate], GetDate())
		LEFT JOIN		[AG].[Department] [D]
			ON			[D].[DepartmentID] = [A].[DepartmentID]
			AND			@InputDate BETWEEN [D].[StartDate] AND ISNULL([D].[EndDate], GetDate())
		WHERE			@InputDate BETWEEN [A].[StartDate] AND ISNULL([A].[EndDate], GetDate())
		GROUP BY		[A].[AssociateID],
                        [A].[LastName] + ', ' + [A].[FirstName],
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
                    

			

SELECT
#AGL.AssociateID,
#AGL.AssociateName, 
#AGL.Description,
#AGL.Type,
#AGL.ReserveAuthority,
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
#AGL.[RentLeasesSuppliesRelated]
FROM #AGL
WHERE #AGL.groupid = 1
GROUP BY
#AGL.[AssociateID],
#AGL.[AssociateName], 
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
#AGL.[RentLeasesSuppliesRelated]
ORDER BY #AGL.[AssociateName],
#AGL.[Type] DESC


 
DROP TABLE #AGL

END
 
/****** Object:  StoredProcedure [dbo].[rpt_LatentCD]    Script Date: 11/30/2014 10:59:08 AM ******/
SET ANSI_NULLS ON

GO


