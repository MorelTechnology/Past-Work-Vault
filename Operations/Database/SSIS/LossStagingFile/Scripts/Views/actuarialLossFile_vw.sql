USE [RS_Reporting]

/****** Object:  View [dbo].[ActuarialLossFile_vw]    Script Date: 3/15/2018 10:19:53 AM ******/

IF EXISTS (SELECT OBJECT_ID(N'ActuarialLossFile_vw', 'V' ))
BEGIN
	DROP VIEW ActuarialLossFile_vw
END
GO

CREATE VIEW [dbo].[ActuarialLossFile_vw] AS

SELECT        
	  Valuationdate
	, Portfolio

	, CASE WHEN (lsf.RiskState = 'CA' AND lsf.Portfolio = 'ASI' AND lsf.Special_Tracking_Group like '%cd%' AND 
	lsf.Program = 'E0183'
	)
	THEN 'Casualty CA CD' ELSE las.ActuarialSegment END AS ActuarialSegment_LSF
	,  revact.[Actuarial_Segment] AS ActuarialSegment
	, lsf.DataSource
	, lsf.Exposure_no
	, WorkMatter_No
	, WM_Description
	, lsf.Workmatter_Type
	, LTRIM(RTRIM(lsf.Claim_No))AS Claim_no
	, Loss_Date
	, Policy_Number
	, Policy_Type_desc
	, Policy_Effective_Date
	, lsf.Policy_Exp_Date
	, Jurisdiction_State_Cd
	, RiskState
	, NAIC_cd
	, Product_Description
	, Subproduct_Description
	, Coverage_Description
	, ASLOB
	, Novation_Status
	, Financial_Entity
	, BusinessType
	, Navision_Reporting_Cd
	, RS_ReinsurerID
	, Sapiens_Contract_ID
	, Sap_Reinsurer_ID
	, Contract_Section
	, Affiliate_code
	, lsf.Special_Tracking_Group
	, lsf.Reported_Date
	, lsf.Historic_RiverStone_Claim_No
	, lsf.Client_Claim_No
	, asr.PolicyBasisDesc
	, lsf.Claimbasis_Desc
	, lsf.Contract_Type
	, lsf.Contract_Effective_date
	, lsf.Producer_Code
	, lsf.Program
	, lsf.Glassboxrulegroup
	, lsf.Glassboxruledescription
	, SUM(Paid_Loss) AS Paid_Loss
	, SUM(Paid_Adjusting_Expense) AS Paid_Adjusting_Expense
	, SUM(Paid_Adjusting_Expense_InLimits) AS Paid_Adjusting_Expense_InLimits
	, SUM(Paid_Coverage_DJ_Expense) AS Paid_Coverage_DJ_Expense
	, SUM(Recovery_Deductible_Loss) AS Recovery_Deductible_Loss
	, SUM(Recovery_deductible_expense) AS Recovery_deductible_expense
	, SUM(Recovery_salvage) AS Recovery_salvage
	, SUM(Recovery_subrogation_loss) AS Recovery_subrogation_loss
	, SUM(Recovery_subrogation_expense) AS Recovery_subrogation_expense
	, SUM(Case_loss) AS Case_Loss
	, SUM(Case_coverage_dj_expense) AS Case_coverage_dj_expense
	, SUM(Case_defense_expense) AS Case_defense_expense
	, SUM(lsf.case_change_coverage_dj_expense) AS Case_change_coverage_dj_expense
	, SUM(lsf.case_change_defense_expense) AS Case_change_defense_expense
	, SUM(lsf.case_change_loss) AS Case_change_loss
	, COUNT_BIG(*) AS CountLines
FROM            
	dbo.LossStagingFile AS lsf 
LEFT OUTER JOIN
    ref.ActuarialSegmentRules AS asr ON lsf.Portfolio = asr.PortfolioCd AND asr.DataSource = 'LSF' 
LEFT OUTER JOIN
	[dbo].[Rev_ACT_Segment_Mapping_current] revact ON LTRIM(RTRIM(lsf.Claim_No))= LTRIM(RTRIM(revact.ClaimNo))
LEFT OUTER JOIN
    ref.ActuarialSegment AS las ON las.PortfolioCD = lsf.Portfolio 
								AND CASE WHEN asr.ProgramCd = 1 THEN COALESCE (lsf.Program, '') ELSE COALESCE (las.ProgramCD, '') END = COALESCE (las.ProgramCD, '') 
								AND CASE WHEN asr.ProducerCd = 1 THEN COALESCE (lsf.Producer_Code, '') ELSE COALESCE (las.ProducerCD, '') END = COALESCE (las.ProducerCD, '') 
								AND CASE WHEN asr.NAICCD = 1 THEN COALESCE (lsf.NAIC_cd, '') ELSE COALESCE (las.NAICCD, '') END = COALESCE (las.NAICCD, '') 
								AND CASE WHEN asr.PolicyProductDesc = 1 THEN COALESCE (lsf.Product_Description, '') ELSE COALESCE (las.PolicyProductDesc, '') END = COALESCE (las.PolicyProductDesc, '') 
								AND CASE WHEN asr.PolicySubProductDesc = 1 THEN COALESCE (lsf.Subproduct_Description, '') ELSE COALESCE (las.PolicySubProductDesc, '') END = COALESCE (las.PolicySubProductDesc, '') 
								AND CASE WHEN asr.CoverageDesc = 1 THEN COALESCE (lsf.Coverage_Description, '') ELSE COALESCE (las.CoverageDesc, '') END = COALESCE (las.CoverageDesc, '') 
								AND CASE WHEN asr.PolicyBasisDesc = 1 THEN COALESCE (lsf.Policy_Basis_Desc, '') ELSE COALESCE (las.PolicyBasisDesc, '') END = COALESCE (las.PolicyBasisDesc, '') 
								AND CASE WHEN asr.SpecialTrackingGroupCD = 1 THEN COALESCE (lsf.Special_Tracking_Group, '') ELSE COALESCE (las.SpecialTrackingGroupCD, '') END = COALESCE (las.SpecialTrackingGroupCD, '')

GROUP BY 
	  Valuationdate
	, Portfolio
	, LTRIM(RTRIM(lsf.Claim_No))
	, Loss_Date
	, Policy_Number
	, Policy_Type_desc
	, Policy_Effective_Date
	, Jurisdiction_State_Cd
	, RiskState
	, NAIC_cd
	, Product_Description
	, Subproduct_Description
	, Coverage_Description
	, ASLOB
	, Novation_Status
	, Financial_Entity
	, BusinessType
	, Navision_Reporting_Cd
	, RS_ReinsurerID
	, Sapiens_Contract_ID
	, Sap_Reinsurer_ID
	, Contract_Section
	, lsf.Contract_Effective_date
	, Affiliate_code
	, las.ActuarialSegment
	, WorkMatter_No
	, WM_Description
	, lsf.Workmatter_Type
	, lsf.Special_Tracking_Group
	, lsf.Reported_Date
	, lsf.Historic_RiverStone_Claim_No
	, lsf.Client_Claim_No
	, asr.PolicyBasisDesc
	, lsf.Claimbasis_Desc
	, lsf.Contract_Type
	, lsf.Producer_Code
	, lsf.Program
	, lsf.Glassboxrulegroup
	, lsf.Glassboxruledescription
	, lsf.DataSource
	, lsf.Policy_Exp_Date
	, lsf.Exposure_no
	, revact.[Actuarial_Segment]

GO