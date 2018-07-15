---------------------------------------------------------------------------------------------------
--Run this script on BIDM01 Server.
--This script is for creating database ClearwaterSAP along with all the databse objects on BIDM01 Server 
---------------------------------------------------------------------------------------------------


USE [master]
GO


IF  EXISTS (
	SELECT name 
		FROM sys.databases 
		WHERE name = N'ClearwaterSAP'
)
DROP DATABASE [ClearwaterSAP]
GO


CREATE DATABASE [ClearwaterSAP]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ClearwaterSAP', FILENAME = N'E:\Data\ClearwaterSAP.mdf' , SIZE = 819200KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ClearwaterSAP_log', FILENAME = N'E:\log\ClearwaterSAP_log.ldf' , SIZE = 7460992KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [ClearwaterSAP] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ClearwaterSAP].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [ClearwaterSAP] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET ARITHABORT OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [ClearwaterSAP] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [ClearwaterSAP] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET  DISABLE_BROKER 
GO

ALTER DATABASE [ClearwaterSAP] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [ClearwaterSAP] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET RECOVERY FULL 
GO

ALTER DATABASE [ClearwaterSAP] SET  MULTI_USER 
GO

ALTER DATABASE [ClearwaterSAP] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [ClearwaterSAP] SET DB_CHAINING OFF 
GO

ALTER DATABASE [ClearwaterSAP] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [ClearwaterSAP] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [ClearwaterSAP] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [ClearwaterSAP] SET  READ_WRITE 
GO






USE [ClearwaterSAP]
GO

------------------------------------------------------------------------------------------------------------------------
--Tables
------------------------------------------------------------------------------------------------------------------------

IF OBJECT_ID('AccountTranslation','U') IS NOT NULL
DROP TABLE AccountTranslation
GO


CREATE TABLE [dbo].[AccountTranslation](
	[CICAccountNumber] [varchar](20) NULL,
	[CICAcctBase] [varchar](20) NULL,
	[CICBusCat] [varchar](20) NULL,
	[NavisionAccountNumber] [varchar](20) NULL
) ON [PRIMARY]

GO



IF OBJECT_ID('CICCurrentJEData','U') IS NOT NULL
DROP TABLE CICCurrentJEData
GO

CREATE TABLE [dbo].[CICCurrentJEData](
	[CICEntryID] [int] IDENTITY(1,1) NOT NULL,
	[Ld] [varchar](50) NULL,
	[LedgerGrp] [varchar](50) NULL,
	[CoCd] [varchar](50) NULL,
	[DocumentNo] [varchar](50) NULL,
	[Typ] [varchar](50) NULL,
	[PstngDate] [varchar](50) NULL,
	[Reference] [varchar](50) NULL,
	[Curr] [varchar](50) NULL,
	[PK] [varchar](50) NULL,
	[Account] [varchar](50) NULL,
	[Amount] [varchar](50) NULL,
	[AmountInLC] [varchar](50) NULL,
	[AmountInLocCurr2] [varchar](50) NULL,
	[Amtinloccurr3] [varchar](50) NULL,
	[Text] [varchar](50) NULL,
	[Description] [varchar](50) NULL,
	[Assignment] [varchar](50) NULL,
	[ProfitCtr] [varchar](50) NULL,
	[CostCtr] [varchar](50) NULL,
	[TrPrt] [varchar](50) NULL,
	[TTy] [varchar](50) NULL,
	[ProductCode] [varchar](50) NULL,
	[UWYear] [varchar](50) NULL,
	[AccYear] [varchar](50) NULL,
	[ReinLoc] [varchar](50) NULL,
	[BusCat] [varchar](50) NULL,
	[Contract] [varchar](50) NULL,
	[CatCode] [varchar](50) NULL,
	[PrTrFund] [varchar](50) NULL,
	[Broker] [varchar](50) NULL,
	[SubProd] [varchar](50) NULL,
	[ReinName] [varchar](50) NULL,
	[RiskTerr] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[TCode] [varchar](50) NULL,
	[Year] [varchar](50) NULL,
	[Period] [varchar](50) NULL,
	[Currency] [varchar](50) NULL,
	[GeneralLedgerAmt] [varchar](50) NULL,
	[EntryDate] [varchar](50) NULL
)
GO




IF OBJECT_ID('CICJEExcluded','U') IS NOT NULL
DROP TABLE CICJEExcluded
GO


CREATE TABLE [dbo].[CICJEExcluded](
	[CICEntryID] [int] NULL,
	[Ld] [varchar](50) NULL,
	[LedgerGrp] [varchar](50) NULL,
	[CoCd] [varchar](50) NULL,
	[DocumentNo] [varchar](50) NULL,
	[Typ] [varchar](50) NULL,
	[PstngDate] [varchar](50) NULL,
	[Reference] [varchar](50) NULL,
	[Curr] [varchar](50) NULL,
	[PK] [varchar](50) NULL,
	[Account] [varchar](50) NULL,
	[Amount] [varchar](50) NULL,
	[AmountInLC] [varchar](50) NULL,
	[AmountInLocCurr2] [varchar](50) NULL,
	[Amtinloccurr3] [varchar](50) NULL,
	[Text] [varchar](50) NULL,
	[Description] [varchar](50) NULL,
	[Assignment] [varchar](50) NULL,
	[ProfitCtr] [varchar](50) NULL,
	[CostCtr] [varchar](50) NULL,
	[TrPrt] [varchar](50) NULL,
	[TTy] [varchar](50) NULL,
	[ProductCode] [varchar](50) NULL,
	[UWYear] [varchar](50) NULL,
	[AccYear] [varchar](50) NULL,
	[ReinLoc] [varchar](50) NULL,
	[BusCat] [varchar](50) NULL,
	[Contract] [varchar](50) NULL,
	[CatCode] [varchar](50) NULL,
	[PrTrFund] [varchar](50) NULL,
	[Broker] [varchar](50) NULL,
	[SubProd] [varchar](50) NULL,
	[ReinName] [varchar](50) NULL,
	[RiskTerr] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[TCode] [varchar](50) NULL,
	[Year] [varchar](50) NULL,
	[Period] [varchar](50) NULL,
	[Currency] [varchar](50) NULL,
	[GeneralLedgerAmt] [varchar](50) NULL,
	[EntryDate] [varchar](50) NULL
)
GO




IF OBJECT_ID('CICJENewAcctNo','U') IS NOT NULL
DROP TABLE CICJENewAcctNo
GO


CREATE TABLE [dbo].[CICJENewAcctNo](
	[CICEntryID] [int] NULL,
	[Ld] [varchar](50) NULL,
	[LedgerGrp] [varchar](50) NULL,
	[CoCd] [varchar](50) NULL,
	[DocumentNo] [varchar](50) NULL,
	[Typ] [varchar](50) NULL,
	[PstngDate] [varchar](50) NULL,
	[Reference] [varchar](50) NULL,
	[Curr] [varchar](50) NULL,
	[PK] [varchar](50) NULL,
	[Account] [varchar](50) NULL,
	[Amount] [varchar](50) NULL,
	[AmountInLC] [varchar](50) NULL,
	[AmountInLocCurr2] [varchar](50) NULL,
	[Amtinloccurr3] [varchar](50) NULL,
	[Text] [varchar](50) NULL,
	[Description] [varchar](50) NULL,
	[Assignment] [varchar](50) NULL,
	[ProfitCtr] [varchar](50) NULL,
	[CostCtr] [varchar](50) NULL,
	[TrPrt] [varchar](50) NULL,
	[TTy] [varchar](50) NULL,
	[ProductCode] [varchar](50) NULL,
	[UWYear] [varchar](50) NULL,
	[AccYear] [varchar](50) NULL,
	[ReinLoc] [varchar](50) NULL,
	[BusCat] [varchar](50) NULL,
	[Contract] [varchar](50) NULL,
	[CatCode] [varchar](50) NULL,
	[PrTrFund] [varchar](50) NULL,
	[Broker] [varchar](50) NULL,
	[SubProd] [varchar](50) NULL,
	[ReinName] [varchar](50) NULL,
	[RiskTerr] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[TCode] [varchar](50) NULL,
	[Year] [varchar](50) NULL,
	[Period] [varchar](50) NULL,
	[Currency] [varchar](50) NULL,
	[GeneralLedgerAmt] [varchar](50) NULL,
	[EntryDate] [varchar](50) NULL
)
GO




IF OBJECT_ID('CICNavisionJE','U') IS NOT NULL
DROP TABLE CICNavisionJE
GO


CREATE TABLE [dbo].[CICNavisionJE](
	[CICNavisionID] [int] IDENTITY(1,1) NOT NULL,
	[NavisionSubCoNumber] [varchar](4) NULL,
	[Basis] [varchar](1) NULL,
	[CustAffil] [varchar](4) NULL,
	[Location] [varchar](20) NULL,
	[Subdept] [varchar](20) NULL,
	[ServiceType] [varchar](20) NULL,
	[State] [varchar](20) NULL,
	[LOB] [varchar](20) NULL,
	[Treaty] [varchar](20) NULL,
	[AccidentYear] [varchar](20) NULL,
	[PstngDate] [date] NULL,
	[Text] [varchar](50) NULL,
	[GLAccount] [varchar](20) NULL,
	[NavisionAccountNumber] [varchar](20) NULL,
	[SumOfAmount] [float] NULL,
	[SumOfDRAmount] [float] NULL,
	[SumOfCRAmount] [float] NULL,
	[SumOfAmountInLocCurr2] [float] NULL,
	[SumOfDRAmountInLocCurr2] [float] NULL,
	[SumOfCRAmountInLocCurr2] [float] NULL,
	[SourceCode] [varchar](10) NULL,
	[BatchName] [varchar](10) NULL,
	[JournalTemplate] [nvarchar](10) NULL,
	[ValuationDate] [date] NULL,
	[RunDate] [datetime] NULL,
	[JournalDate] [date] NULL,
	[JournalID] [varchar](8) NULL,
	[DocumentNo] [varchar](20) NULL,
	[Future] [varchar](1) NULL,
	[Project] [varchar](20) NULL,
	[OldNavGLAcctNo] [varchar](20) NULL,
	[OldNavGLAcctName] [varchar](50) NULL,
	[SysAddDate] [smalldatetime] NULL,
	[SysAddUser] [varchar](20) NULL
)
GO



IF OBJECT_ID('CustAffilTranslation','U') IS NOT NULL
DROP TABLE CustAffilTranslation
GO


CREATE TABLE [dbo].[CustAffilTranslation](
	[TrPrt] [varchar](5) NULL,
	[CustAffil] [varchar](4) NULL
)
GO



IF OBJECT_ID('SubCoTranslation','U') IS NOT NULL
DROP TABLE SubCoTranslation
GO


CREATE TABLE [dbo].[SubCoTranslation](
	[CICSubCoNumber] [varchar](4) NULL,
	[NavisionSubCoNumber] [varchar](4) NULL
)
GO



------------------------------------------------------------------------------------------------------------------------
--Procedure
------------------------------------------------------------------------------------------------------------------------

IF OBJECT_ID('batch_CICCreateNavisionJE','P') IS NOT NULL
DROP PROCEDURE batch_CICCreateNavisionJE
GO


CREATE PROCEDURE [dbo].[batch_CICCreateNavisionJE]
   @ValidationDate date
AS
-- 7/15/2013 PP
--
-- Added task to SSIS package to get the valuation date and it will
-- be passed in as a parameter
--
--EXEC ROC.ROC.dbo.roc_GetValuationDate 'SAPImport', @ValidationDate OUTPUT

--
-- SAPImport job has ProcessID = 225
--

DELETE FROM dbo.CICNavisionJE

--DBCC CHECKIDENT ('CICNavisionJE', RESEED, 0)

--TRUNCATE TABLE dbo.CICNavisionJE


CREATE TABLE #tmpje (ID int,
                     PostingDate varchar(10),
                     BusCat varchar(2),
                     TTy varchar(3),
                     NewAcct varchar(20),
                     SubCo varchar(20),
                     Affil varchar(5),
                     Amt money,
                     DRAmt money,
                     CRAmt money,
                     AmtInLocCurr2 money,
                     DRAmtInLocCurr2 money,
                     CRAmtInLocCurr2 money,
                     Ref varchar(100),
                     CoCode varchar(4),
                     TrPrt varchar(5),
                     SubDept varchar(3),
                     GLAccount varchar(20),
                     DocumentNo varchar(20))

--INSERT INTO #tmpje(@ValidationDate,                                          
INSERT INTO #tmpje(ID,
                   PostingDate,
                   GLAccount,
                   BusCat,
                   TTy,
                   Affil,
                   Ref,
                   Amt,
                   AmtInLocCurr2,
                   CoCode,
                   TrPrt,
                   DocumentNo,
                   SubDept)                                   
SELECT je.CICEntryID,
       CAST(@ValidationDate AS date),                               --Posting date is always the last day of the month
       je.Account + je.BusCat,
       je.BusCat,
       je.TTy,
       ' ',
       CASE 
          WHEN substring(je.Text,1,4) = 'Line' THEN 'SAP Data'    
          ELSE 'SAP Data: ' + ltrim(rtrim(je.Text))
       END,
       rtrim(ltrim(je.Amount)),
       rtrim(ltrim(je.AmountInLocCurr2)),
       je.CoCd,
       je.TrPrt,
       ltrim(rtrim(je.DocumentNo)),
       ' '
FROM dbo.CICCurrentJEData je 


UPDATE #tmpje
  SET #tmpje.SubCo = subco.NavisionSubCoNumber 
FROM dbo.SubCotranslation subco 
  INNER JOIN #tmpje ON #tmpje.CoCode = subco.CICSubCoNumber 
  

----------------------------------------------------
--
-- Get the Navision account number from the account
-- number mapping which is based on the Clearwater 
-- account
--
----------------------------------------------------
UPDATE #tmpje
SET #tmpje.NewAcct = acct.NavisionAccountNumber
FROM dbo.AccountTranslation acct
  INNER JOIN #tmpje on #tmpje.GLAccount = acct.CICAccountNumber


-----------------------------------------------------
--
-- 12/19/2013 PP
--    The paid losses where the BusCat starts with a 
--    '3' should be mapped to the 1241210030 account
--
-----------------------------------------------------
UPDATE #tmpje
--SET #tmpje.NewAcct = '1241210030'
SET #tmpje.NewAcct = '2710000000'
WHERE SUBSTRING(#tmpje.BusCat,1,1) = '3'
  and #tmpje.NewAcct = 'IGNORE'


-----------------------------------------------------
--
-- Mark any journal entries where there is an account
-- number we haven't seen before
--
-----------------------------------------------------    
UPDATE #tmpje
SET #tmpje.NewAcct = 'NO MATCH'
WHERE #tmpje.NewAcct is null

  

------------------------------------------------------
--
-- Add the customer affiliate for journal entries with
-- a TrPrt number
--
------------------------------------------------------
UPDATE #tmpje
SET #tmpje.Affil = affil.CustAffil
FROM dbo.CustAffilTranslation affil
  INNER JOIN #tmpje on #tmpje.TrPrt = affil.TrPrt


-------------------------------------------------------
--
-- Add a department number for any expense accounts
--
-------------------------------------------------------
UPDATE #tmpje
SET #tmpje.SubDept = '999'
WHERE #tmpje.NewAcct >= '8000000000'



UPDATE #tmpje
SET #tmpje.DRAmt = 0,
    #tmpje.CRAmt = 0,
    #tmpje.DRAmtInLocCurr2 = 0,
    #tmpje.CRAmtInLocCurr2 = 0

UPDATE #tmpje
SET DRAmt = Amt,
    DRAmtInLocCurr2 = AmtInLocCurr2
FROM #tmpje
WHERE CAST(Amt as money) > 0

UPDATE #tmpje
SET CRAmt = Amt,
    CRAmtInLocCurr2 = AmtInLocCurr2
FROM #tmpje
WHERE CAST(Amt as money) < 0

----------------------------------------------------
--
-- If we find a new Clearwater account number, then
-- save the journal entry so it can be emailed to
-- Finance so they can decide how to handle it
--
----------------------------------------------------
DELETE FROM dbo.CICJENewAcctNo


INSERT INTO dbo.CICJENewAcctNo(CICEntryID,
                               Ld,
	                           LedgerGrp,
	                           CoCd,
	                           DocumentNo,
	                           Typ,
	                           PstngDate,
	                           Reference,
	                           Curr,
	                           PK,
	                           Account,
	                           Amount,
	                           AmountInLC,
	                           AmountInLocCurr2,
	                           Amtinloccurr3,
	                           Text,
	                           Description,
	                           Assignment,
	                           ProfitCtr,
	                           CostCtr,
	                           TrPrt,
	                           TTy,
	                           ProductCode,
	                           UWYear,
	                           AccYear,
		                       ReinLoc,
		                       BusCat,
		                       Contract,
		                       CatCode,
		                       PrTrFund,
		                       Broker,
		                       SubProd,
		                       ReinName,
		                       RiskTerr,
		                       Status,
		                       TCode,
		                       Year,
		                       Period,
		                       Currency,
		                       GeneralLedgerAmt,
		                       EntryDate)
SELECT je.CICEntryID, 
       je.Ld,
	   je.LedgerGrp,
       je.CoCd,
       je.DocumentNo,
       je.Typ,
	   je.PstngDate,
	   je.Reference,
	   je.Curr,
	   je.PK,
	   je.Account,
	   je.Amount,
	   je.AmountInLC,
	   je.AmountInLocCurr2,
	   je.Amtinloccurr3,
	   je.Text,
	   je.Description,
	   je.Assignment,
	   je.ProfitCtr,
	   je.CostCtr,
	   je.TrPrt,
	   je.TTy,
	   je.ProductCode,
	   je.UWYear,
	   je.AccYear,
	   je.ReinLoc,
	   je.BusCat,
	   je.Contract,
	   je.CatCode,
	   je.PrTrFund,
	   je.Broker,
	   je.SubProd,
	   je.ReinName,
	   je.RiskTerr,
	   je.Status,
	   je.TCode,
	   je.Year,
	   je.Period,
	   je.Currency,
	   je.GeneralLedgerAmt,
	   je.EntryDate
FROM dbo.CICCurrentJEData je
  INNER JOIN #tmpje tje ON je.CICEntryID = tje.ID
WHERE tje.NewAcct = 'NO MATCH'


DELETE FROM #tmpje  
WHERE NewAcct = 'NO MATCH'



--------------------------------------------------------
--
-- Check to see if there are any journal entries
-- containing Clearwater accounts which should not
-- be processed
--
-- Save any journal entries with accounts to ignore 
-- and delete them from the list of entries
-- going into Navision
--
--------------------------------------------------------
DELETE FROM dbo.CICJEExcluded


INSERT INTO dbo.CICJEExcluded(CICEntryID,
                              Ld,
	                          LedgerGrp,
	                          CoCd,
	                          DocumentNo,
	                          Typ,
	                          PstngDate,
	                          Reference,
	                          Curr,
	                          PK,
	                          Account,
	                          Amount,
	                          AmountInLC,
	                          AmountInLocCurr2,
	                          Amtinloccurr3,
	                          Text,
	                          Description,
	                          Assignment,
	                          ProfitCtr,
	                          CostCtr,
	                          TrPrt,
	                          TTy,
	                          ProductCode,
	                          UWYear,
	                          AccYear,
		                      ReinLoc,
		                      BusCat,
		                      Contract,
		                      CatCode,
		                      PrTrFund,
		                      Broker,
		                      SubProd,
		                      ReinName,
		                      RiskTerr,
		                      Status,
		                      TCode,
		                      Year,
		                      Period,
		                      Currency,
		                      GeneralLedgerAmt,
		                      EntryDate)
SELECT je.CICEntryID, 
       je.Ld,
	   je.LedgerGrp,
       je.CoCd,
       je.DocumentNo,
       je.Typ,
	   je.PstngDate,
	   je.Reference,
	   je.Curr,
	   je.PK,
	   je.Account,
	   je.Amount,
	   je.AmountInLC,
	   je.AmountInLocCurr2,
	   je.Amtinloccurr3,
	   je.Text,
	   je.Description,
	   je.Assignment,
	   je.ProfitCtr,
	   je.CostCtr,
	   je.TrPrt,
	   je.TTy,
	   je.ProductCode,
	   je.UWYear,
	   je.AccYear,
	   je.ReinLoc,
	   je.BusCat,
	   je.Contract,
	   je.CatCode,
	   je.PrTrFund,
	   je.Broker,
	   je.SubProd,
	   je.ReinName,
	   je.RiskTerr,
	   je.Status,
	   je.TCode,
	   je.Year,
	   je.Period,
	   je.Currency,
	   je.GeneralLedgerAmt,
	   je.EntryDate
FROM dbo.CICCurrentJEData je
  INNER JOIN #tmpje tje ON je.CICEntryID = tje.ID
WHERE tje.NewAcct = 'IGNORE'


/*
----------------------------------------------------------
--
-- We need to create offset entries for paid loss entries
-- which have account numbers that are not to be included
--
----------------------------------------------------------


INSERT INTO #tmpje(GLAccount,
                   NewAcct,
                   Ref,
                   SubCo,
                   Affil,
                   PostingDate,
                   SubDept,
                   DRAmt,
                   CRAmt,
                   AmtInLocCurr2,
                   DRAmtInLocCurr2,
                   CRAmtInLocCurr2,
                   Amt)
SELECT SUBSTRING(GLAccount,1,7),
       '2710000000',
       'SAP Data: Paid Loss Offset Acct ' + SUBSTRING(GLAccount,1,7),
       SubCo,
       ' ',
       @ValidationDate,
       ' ',
       SUM(DRAmt),
       SUM(CRAmt),
       SUM(DRAmtInLocCurr2) + SUM(CRAmtInLocCurr2),
       SUM(DRAmtInLocCurr2),
       SUM(CRAmtInLocCurr2),
       SUM(DRAmt) + SUM(CRAmt)
FROM #tmpje
WHERE NewAcct = 'IGNORE'
  AND SUBSTRING(GLAccount,1,7) IN ('5010000','5018900','5019900','5020000','5110000','5118900',
                                   '5119900','5210005','5210007','6917005','6917007')
GROUP BY SUBSTRING(GLAccount,1,7),SubCo
*/

--------------------------------------------------------------
--
-- Find out the total difference between DR and CR amounts
-- of the journal entries to be excluded and create an offset
-- entry for them with Navision account number 2710000000
--
--------------------------------------------------------------
INSERT INTO #tmpje(GLAccount,
                   NewAcct,
                   Ref,
                   SubCo,
                   Affil,
                   PostingDate,
                   SubDept,
                   DRAmt,
                   CRAmt,
                   AmtInLocCurr2,
                   DRAmtInLocCurr2,
                   CRAmtInLocCurr2,
                   Amt)
SELECT ' ',
       '2710000000',
       'SAP Data: Exclusions Offset',
       SubCo,
       ' ',
       @ValidationDate,
       ' ',
       SUM(DRAmt) AS 'SumDRAmt',
       SUM(CRAmt) AS 'SumCRAmt',
       SUM(DRAmtInLocCurr2) + SUM(CRAmtInLocCurr2) AS 'SumDRCRAmtInLocCurr2',
       SUM(DRAmtInLocCurr2) AS 'SumDRAmtInLocCurr2',
       SUM(CRAmtInLocCurr2) AS 'SumCRAmtInLocCurr2',
       SUM(DRAmt) + SUM(CRAmt)
FROM #tmpje
WHERE NewAcct = 'IGNORE'
GROUP BY SubCo



DELETE FROM #tmpje
WHERE #tmpje.NewAcct = 'IGNORE'



-----------------------------------------------------
--
--  Create journal entries for premiums
--
-----------------------------------------------------


INSERT INTO #tmpje(GLAccount,
                   NewAcct,
                   Ref,
                   SubCo,
                   Affil,
                   PostingDate,
                   SubDept,
                   DRAmt,
                   CRAmt,
                   AmtInLocCurr2,
                   DRAmtInLocCurr2,
                   CRAmtInLocCurr2,
                   Amt)
SELECT SUBSTRING(GLAccount,1,7),
       '1213200020',
       'SAP Data: Premium Gross Up Entry',
       SubCo,
       ' ',
       @ValidationDate,
       ' ',
       SUM(DRAmt) * -1,
       SUM(CRAmt) * -1,
       (SUM(DRAmtInLocCurr2) + SUM(CRAmtInLocCurr2)) * -1,
       SUM(DRAmtInLocCurr2)  * -1,
       SUM(CRAmtInLocCurr2) * -1,
       (SUM(DRAmt) + SUM(CRAmt))  * -1
FROM #tmpje
WHERE SUBSTRING(GLAccount,1,7) = '4010000'
GROUP BY SUBSTRING(GLAccount,1,7),SubCo



INSERT INTO #tmpje(GLAccount,
                   NewAcct,
                   Ref,
                   SubCo,
                   Affil,
                   PostingDate,
                   SubDept,
                   DRAmt,
                   CRAmt,
                   AmtInLocCurr2,
                   DRAmtInLocCurr2,
                   CRAmtInLocCurr2,
                   Amt)
SELECT '4010000',
       '1213200030',
       'SAP Data: Premium Gross Up Entry 2',
       SubCo,
       ' ',
       @ValidationDate,
       ' ',
       SUM(DRAmt) * -1,
       SUM(CRAmt) * -1,
       SUM(DRAmtInLocCurr2 + CRAmtInLocCurr2) * -1,
       SUM(DRAmtInLocCurr2) * -1,
       SUM(CRAmtInLocCurr2) * -1,
       SUM(DRAmt + CRAmt) * -1
FROM #tmpje
WHERE Ref = 'SAP Data: Premium Gross Up Entry'
GROUP BY SubCo




INSERT INTO dbo.CICNavisionJE (NavisionSubCoNumber,         --Global Dimension 1 Code            
                               CustAffil,                   --Shortcut Dimension 3 Code
                               Subdept,                     --Shortcut Dimension 6 Code
                               PstngDate,                   --Posting Date                 
                               GLAccount,
                               NavisionAccountNumber,       --G_L Account No_
                               SumOfAmount,                 
                               SumOfDRAmount,
                               SumOfCRAmount,                 
                               SumOfAmountInLocCurr2,       --Amount
                               SumOfDRAmountInLocCurr2,     --Debit Amount
                               SumOfCRAmountInLocCurr2,     --Credit Amount
                               ValuationDate,               --Validation Date
                               RunDate,                     --Run Date
                               JournalDate,                 --Journal Date
                               JournalID,                   --JournalID
                               DocumentNo,                  --Document No_
                               Text)                        --Description                        
SELECT SubCo,
       Affil,
       SubDept,
       PostingDate,
       GLAccount,
       NewAcct,
       CAST(Amt as money),
       CAST(DRAmt as money),
       CAST(CRAmt as money),
       CAST(AmtInLocCurr2 as money),
       CAST(DRAmtInLocCurr2 as money),
       CAST(CRAmtInLocCurr2 as money),
       @ValidationDate,
       GETDATE(),                                                   --Run Date
       PostingDate,                                                 --Journal Date
       replace(convert(char(10), @ValidationDate, 101),'/', ''),    --Journal ID   
       DocumentNo,
       substring(Ref,1,50)                                          --Description
FROM #tmpje

UPDATE dbo.CICNavisionJE
SET                             
                                                      --Description
      Basis = '2',                                    --Global Dimension 2 Code
      BatchName = 'CONVERSION',                       --Journal Batch Name                           
      SourceCode = 'GENJNL',                          --Source Code                             
      ServiceType = ' ',
      State = ' ',                                    --Shortcut Dimension 7 Code
      LOB = ' ',                                      --Shortcut Dimension 8 Code
      Treaty = ' ',
      AccidentYear = ' ',
      Future = ' ',                                   --Shortcut Dimension 5 Code
      Location = ' ',
      Project = ' ',                                  --Shortcut Dimension 4 Code
      SysAddDate = GETDATE(),
      SysAddUser = 'BATCH'





DROP TABLE #tmpje



GO
