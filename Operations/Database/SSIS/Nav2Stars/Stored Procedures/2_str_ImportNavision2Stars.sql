USE [STARS]
GO

/****** Object:  StoredProcedure [dbo].[str_ImportNavision2Stars]    Script Date: 3/26/2018 3:10:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT OBJECT_ID(N'str_ImportNavision2Stars', 'P'))
BEGIN
	DROP PROCEDURE str_ImportNavision2Stars
END
GO

CREATE PROCEDURE [dbo].[str_ImportNavision2Stars]
AS 
    BEGIN

        CREATE TABLE #tmpTran
            (
              nav2starsid INT NOT NULL ,
              ReportingEntityCode CHAR(1) NULL ,
              EntityCode CHAR(1) NULL ,
              ValuationDate VARCHAR(27) NULL ,
              SourceSystemCode VARCHAR(50) NULL ,
              BusinessTypeCode CHAR(1) ,
              CompanyCode CHAR(4) NULL ,
              MajorPerilCode CHAR(3) NULL ,
              ActuarialLineCode INT NULL ,
              StateCode CHAR(2) NULL ,
              ReinsurerID INT NULL ,
              TreatyID INT NULL ,
              PremiumYear CHAR(4) NULL ,
              Term INT NULL ,
              AccidentYear CHAR(4) NULL ,
              Description VARCHAR(50) NULL ,
              ASLOB CHAR(4) NULL ,
              Amount DECIMAL(14, 2) ,
              PaidLoss DECIMAL(14, 2) DEFAULT ( 0 ) ,
              PaidOverheadExpense DECIMAL(14, 2) DEFAULT ( 0 ) ,
              PaidAllocatedExpense DECIMAL(14, 2) DEFAULT ( 0 ) ,
              PaidUnallocatedExpense DECIMAL(14, 2) DEFAULT ( 0 ) ,
              PaidSalSub DECIMAL(14, 2) DEFAULT ( 0 ) ,
              CaseLossReserve DECIMAL(14, 2) DEFAULT ( 0 ) ,
              CaseAllocatedExpenseReserve DECIMAL(14, 2) DEFAULT ( 0 ) ,
              CaseUnallocatedExpenseReserve DECIMAL(14, 2) DEFAULT ( 0 ) ,
              IBNRLossReserve DECIMAL(14, 2) DEFAULT ( 0 ) ,
              IBNROverheadExpense DECIMAL(14, 2) DEFAULT ( 0 ) ,
              IBNRAllocatedExpenseReserve DECIMAL(14, 2) DEFAULT ( 0 ) ,
              IBNRUnallocatedExpenseReserve DECIMAL(14, 2) DEFAULT ( 0 ) ,
              IBNRSalSubReserve DECIMAL(14, 2) DEFAULT ( 0 ) ,
              WrittenPremium DECIMAL(14, 2) DEFAULT ( 0 ) ,
              EarnedPremium DECIMAL(14, 2) DEFAULT ( 0 ) ,
              UnearnedPremium DECIMAL(14, 2) DEFAULT ( 0 ) ,
              Commission DECIMAL(14, 2) DEFAULT ( 0 ) ,
              TaxesLicenseFee DECIMAL(14, 2) DEFAULT ( 0 ) ,
              FinanceCharge DECIMAL(14, 2) DEFAULT ( 0 ) ,
              DividendPaid DECIMAL(14, 2) DEFAULT ( 0 ) ,
              Cash DECIMAL(14, 2) DEFAULT ( 0 ) ,
              CededPayableAmount DECIMAL(14, 2) DEFAULT ( 0 ) ,
              RegisterNumber VARCHAR(8) NULL ,
              AffiliateCode CHAR(4) NULL ,
              Explanation VARCHAR(100) NULL ,
              ABCSubdivision CHAR(3) NULL ,
              Subsystem CHAR(3) NULL ,
              TransID INT NULL
            )

        INSERT  INTO #tmpTran
                ( nav2starsid ,
                  ValuationDate ,
                  SourceSystemCode ,
                  CompanyCode ,
                  ActuarialLineCode ,
                  AccidentYear ,
                  Description ,
                  RegisterNumber ,
                  AffiliateCode ,
                  Explanation ,
                  ABCSubdivision ,
                  Subsystem ,
                  ASLOB ,
                  Amount,
                  StateCode
                )
                SELECT  nav2starsid ,
                        ValuationDate ,
                        'SADJ' ,
                        CASE WHEN Company = '9191' THEN '3'
                             ELSE Company
                        END ,
                        '0' ,
                        CASE WHEN AccidentYear = 'PREV' THEN '1995'
                             ELSE AccidentYear
                        END ,
                        'MJE from Navision' ,
                        RegisterNumber ,
                        CASE WHEN AffiliateCode = '' THEN NULL
                             ELSE AffiliateCode
                        END ,
                        CASE WHEN Explanation = '' THEN NULL
                             ELSE Explanation
                        END ,
                        CASE WHEN ABCSubdivision = '' THEN NULL
                             ELSE ABCSubdivision
                        END ,
                        CASE Subsystem
                          WHEN '' THEN NULL
                          WHEN 'OLRI' THEN 'OLR'
                          ELSE Subsystem
                        END ,
                        LOB,
                        CAST (Amount AS DECIMAL(14, 2)),
                        State
                FROM    nav2stars
                WHERE   TransProcessedTime IS NULL

-- ReportingEnityCode/EntityCode
        UPDATE  #tmpTran
        SET     ReportingEntityCode = company.ReportingEntityCode ,
                EntityCode = company.EntityCode
        FROM    Global.Rivernet_Global.dbo.Company company
                JOIN #tmpTran ON #tmpTran.CompanyCode = company.CompanyCode

-- TreatyID
        UPDATE  #tmpTran
        SET     TreatyID = treaty.TreatyID
        FROM    #tmpTran
                JOIN nav2stars ON nav2stars.nav2starsid = #tmpTran.nav2starsid
                JOIN Global.Rivernet_Global.dbo.Treaty treaty ON nav2stars.TreatyCode = treaty.AffiliateTreatyCode
                                                              AND nav2stars.TreatyEffectiveDate = treaty.EffectiveDate
                                                              AND #tmpTran.EntityCode = Treaty.EntityCode

-- ReinsurerID
        UPDATE  #tmpTran
        SET     ReinsurerID = reinsurer.ReinsurerID
        FROM    #tmpTran
                JOIN nav2stars ON nav2stars.nav2starsid = #tmpTran.nav2starsid
                JOIN Global.Rivernet_Global.dbo.Reinsurer reinsurer ON nav2stars.TreatyCode = reinsurer.AffiliateRecoCode
                                                              AND #tmpTran.EntityCode = reinsurer.EntityCode
	
--Check to ensure all contracts provided were either valid recos or treaties.
        DECLARE @stagingContractCount AS DECIMAL(14, 2) ,
            @tmpContractCount AS DECIMAL(14, 2)

        SET @stagingContractCount = ( SELECT COUNT(*)
                                      FROM dbo.nav2stars
                                      WHERE TreatyCode IS NULL OR LTRIM(RTRIM(TreatyCode)) = ''
                                      AND TransProcessedTime IS NULL
                                    )

        SET @tmpContractCount = ( SELECT COUNT(*)
                                  FROM #tmpTran
                                  WHERE TreatyID IS NULL AND ReinsurerID IS NULL
                                )		

        IF @stagingContractCount <> @tmpContractCount 
            BEGIN
                RAISERROR ('Invalid contract(s) detected in nav2stars staging table.',16,1)
                RETURN
            END	
	
--Now that Treaty/Reco's have been split up, allocate treaties to participants
        CREATE TABLE #tmpTran_Treaty
            (
              nav2starsid INT NOT NULL ,
              ReportingEntityCode CHAR(1) NULL ,
              EntityCode CHAR(1) NULL ,
              ValuationDate VARCHAR(27) NULL ,
              SourceSystemCode VARCHAR(50) NULL ,
              CompanyCode CHAR(4) NULL ,
              ActuarialLineCode INT NULL ,
              ReinsurerID INT NULL ,
              TreatyID INT NULL ,
              AccidentYear CHAR(4) NULL ,
              Description VARCHAR(50) NULL ,
              ASLOB CHAR(4) NULL ,
              Amount DECIMAL(14, 2) ,
              RegisterNumber VARCHAR(8) NULL ,
              AffiliateCode CHAR(4) NULL ,
              Explanation VARCHAR(100) NULL ,
              ABCSubdivision CHAR(3) NULL ,
              Subsystem CHAR(3) NULL ,
              TransID INT NULL,
              StateCode CHAR(2)
            )	

        INSERT  INTO #tmpTran_Treaty
                ( nav2starsid ,
                  ReportingEntityCode ,
                  EntityCode ,
                  ValuationDate ,
                  SourceSystemCode ,
                  CompanyCode ,
                  ActuarialLineCode ,
                  ReinsurerID ,
                  TreatyID ,
                  AccidentYear ,
                  Description ,
                  ASLOB ,
                  Amount ,
                  RegisterNumber ,
                  AffiliateCode ,
                  Explanation ,
                  ABCSubdivision ,
                  Subsystem ,
                  TransID,
                  StateCode
                )
                SELECT  nav2starsid ,
                        ReportingEntityCode ,
                        EntityCode ,
                        ValuationDate ,
                        SourceSystemCode ,
                        CompanyCode ,
                        ActuarialLineCode ,
                        ReinsurerID ,
                        TreatyID ,
                        AccidentYear ,
                        Description ,
                        ASLOB ,
                        Amount ,
                        RegisterNumber ,
                        AffiliateCode ,
                        Explanation ,
                        ABCSubdivision ,
                        Subsystem ,
                        TransID,
                        StateCode
                FROM    #tmpTran
                WHERE   TreatyID IS NOT NULL

        DELETE  #tmpTran
        WHERE   TreatyID IS NOT NULL

        INSERT  INTO #tmpTran
                ( nav2starsid ,
                  ReportingEntityCode ,
                  EntityCode ,
                  ValuationDate ,
                  SourceSystemCode ,
                  CompanyCode ,
                  ActuarialLineCode ,
                  ReinsurerID ,
                  TreatyID ,
                  AccidentYear ,
                  Description ,
                  ASLOB ,
                  Amount ,
                  RegisterNumber ,
                  AffiliateCode ,
                  Explanation ,
                  ABCSubdivision ,
                  Subsystem ,
                  TransID,
                  StateCode
                )
                SELECT  nav2starsid ,
                        ReportingEntityCode ,
                        EntityCode ,
                        ValuationDate ,
                        SourceSystemCode ,
                        CompanyCode ,
                        ActuarialLineCode ,
                        p.ReinsurerID ,
                        t.TreatyID ,
                        AccidentYear ,
                        Description ,
                        ASLOB ,
                        Amount * p.ParticipantPercent / 100 ,
                        RegisterNumber ,
                        AffiliateCode ,
                        Explanation ,
                        ABCSubdivision ,
                        Subsystem ,
                        TransID,
                        StateCode
                FROM    #tmpTran_Treaty t
                        LEFT JOIN GLOBAL.RiverNet_Global.dbo.Participant p ON p.TreatyID = t.TreatyID
 
        DROP TABLE #tmpTran_Treaty
	
--Convert State
--        UPDATE  #tmpTran
--        SET     StateCode = state.StateCode
--        FROM    #tmpTran
--                JOIN nav2stars ON nav2stars.nav2starsid = #tmpTran.nav2starsid
--                JOIN Global.Rivernet_Global.dbo.State state ON state.TIGStateCode = nav2stars.State
	
-- End of Month ValuationDate
        UPDATE  #tmpTran
        SET     ValuationDate = CASE WHEN SUBSTRING(ValuationDate,1,2) IN ('01','03','05','07','08','10','12')
										THEN SUBSTRING(ValuationDate,1,3) + '31' + SUBSTRING(ValuationDate,6,5)
									 WHEN SUBSTRING(ValuationDate,1,2) in ('04','06','09','11')
										THEN SUBSTRING(ValuationDate,1,3) + '30' + SUBSTRING(ValuationDate,6,5)
									 WHEN SUBSTRING(ValuationDate,1,2) in ('02') 
										  AND SUBSTRING(ValuationDate,7,4) IN ('2012','2016','2020','2024','2028','2032')
										THEN SUBSTRING(ValuationDate,1,3) + '29' + SUBSTRING(ValuationDate,6,5)
										ELSE SUBSTRING(ValuationDate,1,3) + '28' + SUBSTRING(ValuationDate,6,5)	
								END

-- ValuationDate (all rows in ValuationPeriod table have ValuationDates with the last day of the month)
        UPDATE  #tmpTran
        SET     ValuationDate = CAST(vp.ValuationDate AS Date)
        FROM    Global.Rivernet_Global.dbo.ValuationPeriod vp
                JOIN #tmpTran ON ( DATEPART(year, vp.ValuationDate) = DATEPART(year,
                                                              #tmpTran.ValuationDate) )
                                 AND ( DATEPART(month, vp.ValuationDate) = DATEPART(month,
                                                              #tmpTran.ValuationDate) )
                                 AND vp.EntityCode = #tmpTran.EntityCode
                                 AND vp.System = 'Reins'	
	
-- this probably could be done with subquery instead of temp table...
        CREATE TABLE #maxMajorPeril
            (
              LOB CHAR(3) ,
              MajorPeril CHAR(3)
            )

        INSERT  INTO #maxMajorPeril
                SELECT  mp.LoBCode ,
                        MAX(mp.MajorPerilCode)
                FROM    Global.Rivernet_Global.dbo.MajorPeril mp
                GROUP BY mp.LoBCode

        UPDATE  #tmpTran
        SET     MajorPerilCode = mp.MajorPeril
        FROM    #tmpTran
                JOIN #maxMajorPeril mp ON mp.LoB = SUBSTRING(#tmpTran.ASLOB, 1, 3)

        DROP TABLE #maxMajorPeril

-- Business Type
        UPDATE  #tmpTran
        SET     BusinessTypeCode = NavisionAccountMapping.StarsBusinessType
        FROM    #tmpTran
                JOIN nav2stars ON nav2stars.nav2starsid = #tmpTran.nav2starsid
                JOIN NavisionAccountMapping ON NavisionAccountMapping.NavisionAccountNumber = nav2stars.Account

--Financial Buckets
        UPDATE  #tmpTran
        SET     CaseLossReserve = #tmpTran.Amount * -1
        FROM    nav2stars
                JOIN #tmpTran ON #tmpTran.nav2starsid = nav2stars.nav2starsid
                JOIN NavisionAccountMapping ON NavisionAccountMapping.NavisionAccountNumber = nav2stars.Account
        WHERE   StarsTransField = 'CaseLossReserve'

        UPDATE  #tmpTran
        SET     CaseAllocatedExpenseReserve = #tmpTran.Amount * -1
        FROM    nav2stars
                JOIN #tmpTran ON #tmpTran.nav2starsid = nav2stars.nav2starsid
                JOIN NavisionAccountMapping ON NavisionAccountMapping.NavisionAccountNumber = nav2stars.Account
        WHERE   StarsTransField = 'CaseAllocatedExpenseReserve'

        UPDATE  #tmpTran
        SET     PaidLoss = #tmpTran.Amount
        FROM    nav2stars
                JOIN #tmpTran ON #tmpTran.nav2starsid = nav2stars.nav2starsid
                JOIN NavisionAccountMapping ON NavisionAccountMapping.NavisionAccountNumber = nav2stars.Account
        WHERE   StarsTransField = 'PaidLoss'

        UPDATE  #tmpTran
        SET     PaidAllocatedExpense = #tmpTran.Amount
        FROM    nav2stars
                JOIN #tmpTran ON #tmpTran.nav2starsid = nav2stars.nav2starsid
                JOIN NavisionAccountMapping ON NavisionAccountMapping.NavisionAccountNumber = nav2stars.Account
        WHERE   StarsTransField = 'PaidAllocatedExpense'

        UPDATE  #tmpTran
        SET     PaidUnallocatedExpense = #tmpTran.Amount
        FROM    nav2stars
                JOIN #tmpTran ON #tmpTran.nav2starsid = nav2stars.nav2starsid
                JOIN NavisionAccountMapping ON NavisionAccountMapping.NavisionAccountNumber = nav2stars.Account
        WHERE   StarsTransField = 'PaidUnallocatedExpense'

        UPDATE  #tmpTran
        SET     WrittenPremium = #tmpTran.Amount * -1
        FROM    nav2stars
                JOIN #tmpTran ON #tmpTran.nav2starsid = nav2stars.nav2starsid
                JOIN NavisionAccountMapping ON NavisionAccountMapping.NavisionAccountNumber = nav2stars.Account
        WHERE   StarsTransField = 'WrittenPremium'
        
        UPDATE  #tmpTran
        SET     IBNRAllocatedExpenseReserve = #tmpTran.Amount * -1 
        FROM    nav2stars
                JOIN #tmpTran ON #tmpTran.nav2starsid = nav2stars.nav2starsid
                JOIN NavisionAccountMapping ON NavisionAccountMapping.NavisionAccountNumber = nav2stars.Account
        WHERE   StarsTransField = 'IBNRAllocatedExpenseReserve'
        
        UPDATE  #tmpTran
        SET     IBNRLossReserve = #tmpTran.Amount * -1
        FROM    nav2stars
                JOIN #tmpTran ON #tmpTran.nav2starsid = nav2stars.nav2starsid
                JOIN NavisionAccountMapping ON NavisionAccountMapping.NavisionAccountNumber = nav2stars.Account
        WHERE   StarsTransField = 'IBNRLossReserve'
        
        UPDATE  #tmpTran
        SET     IBNROverheadExpense = #tmpTran.Amount * -1
        FROM    nav2stars
                JOIN #tmpTran ON #tmpTran.nav2starsid = nav2stars.nav2starsid
                JOIN NavisionAccountMapping ON NavisionAccountMapping.NavisionAccountNumber = nav2stars.Account
        WHERE   StarsTransField = 'IBNROverheadExpense'
        
        UPDATE  #tmpTran
        SET     UnearnedPremium = #tmpTran.Amount * -1
        FROM    nav2stars
                JOIN #tmpTran ON #tmpTran.nav2starsid = nav2stars.nav2starsid
                JOIN NavisionAccountMapping ON NavisionAccountMapping.NavisionAccountNumber = nav2stars.Account
        WHERE   StarsTransField = 'UnearnedPremium'


--///////  insert into trans table

        INSERT  INTO trans
                ( ReportingEntityCode ,
                  EntityCode ,
                  ValuationDate ,
                  SourceSystemCode ,
                  BusinessTypeCode ,
                  CompanyCode ,
                  MajorPerilCode ,
                  ActuarialLineCode ,
                  StateCode ,
                  ReinsurerID ,
                  TreatyID ,
                  PremiumYear ,
                  Term ,
                  AccidentYear ,
                  Description ,
                  PaidLoss ,
                  PaidOverheadExpense ,
                  PaidAllocatedExpense ,
                  PaidUnallocatedExpense ,
                  PaidSalSub ,
                  CaseLossReserve ,
                  CaseAllocatedExpenseReserve ,
                  CaseUnallocatedExpenseReserve ,
                  IBNRLossReserve ,
                  IBNROverheadExpense ,
                  IBNRAllocatedExpenseReserve ,
                  IBNRUnallocatedExpenseReserve ,
                  IBNRSalSubReserve ,
                  WrittenPremium ,
                  EarnedPremium ,
                  UnearnedPremium ,
                  Commission ,
                  TaxesLicenseFee ,
                  FinanceCharge ,
                  DividendPaid ,
                  Cash ,
                  CededPayableAmount ,
                  RegisterNumber ,
                  AffiliateCode ,
                  Explanation ,
                  ABCSubdivision ,
                  Subsystem ,
                  ASLOB ,
                  SysAddUser ,
                  SysAddDate ,
                  TransID
                )
                SELECT  ReportingEntityCode ,
                        EntityCode ,
                        ValuationDate ,
                        SourceSystemCode ,
                        BusinessTypeCode ,
                        CompanyCode ,
                        MajorPerilCode ,
                        ActuarialLineCode ,
                        StateCode ,
                        ReinsurerID ,
                        TreatyID ,
                        PremiumYear ,
                        Term ,
                        AccidentYear ,
                        Description ,
                        PaidLoss ,
                        PaidOverheadExpense ,
                        PaidAllocatedExpense ,
                        PaidUnallocatedExpense ,
                        PaidSalSub ,
                        CaseLossReserve ,
                        CaseAllocatedExpenseReserve ,
                        CaseUnallocatedExpenseReserve ,
                        IBNRLossReserve ,
                        IBNROverheadExpense ,
                        IBNRAllocatedExpenseReserve ,
                        IBNRUnallocatedExpenseReserve ,
                        IBNRSalSubReserve ,
                        WrittenPremium ,
                        EarnedPremium ,
                        UnearnedPremium ,
                        Commission ,
                        TaxesLicenseFee ,
                        FinanceCharge ,
                        DividendPaid ,
                        Cash ,
                        CededPayableAmount ,
                        RegisterNumber ,
                        AffiliateCode ,
                        Explanation ,
                        ABCSubdivision ,
                        Subsystem ,
                        ASLOB ,
                        'NAVISION' ,
                        GETDATE() ,
                        nav2starsid
                FROM    #tmpTran

        IF @@error = 0 
            BEGIN 
                UPDATE  nav2stars
                SET     TransProcessedTime = GETDATE() ,
                        TransactionID = trans.TransactionID
                FROM    trans WITH (INDEX(idx_sysadduser))
                        JOIN nav2stars ON nav2stars.nav2starsid = trans.TransID
                WHERE   nav2stars.TransProcessedTime IS NULL
	
				
                UPDATE  t
                SET     TransID = NULL
				FROM  trans t WITH (INDEX(idx_sysadduser))
                WHERE   SysAddUser = 'NAVISION'
				AND		TransID IS NOT NULL
            
			END
			ELSE RAISERROR ('Navision Insert Issue', 16, 1)

        DROP TABLE #tmpTran

    END












GO


