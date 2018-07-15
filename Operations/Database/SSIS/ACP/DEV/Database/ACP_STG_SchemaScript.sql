USE [master]
GO
/****** Object:  Database [ACP_STG]    Script Date: 1/26/2017 9:23:00 AM ******/
CREATE DATABASE [ACP_STG]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ACP_STG', FILENAME = N'E:\Data\ACP_STG.mdf' , SIZE = 1579008KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ACP_STG_log', FILENAME = N'E:\log\ACP_STG_log.ldf' , SIZE = 517184KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ACP_STG].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ACP_STG] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ACP_STG] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ACP_STG] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ACP_STG] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ACP_STG] SET ARITHABORT OFF 
GO
ALTER DATABASE [ACP_STG] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ACP_STG] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [ACP_STG] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ACP_STG] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ACP_STG] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ACP_STG] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ACP_STG] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ACP_STG] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ACP_STG] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ACP_STG] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ACP_STG] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ACP_STG] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ACP_STG] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ACP_STG] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ACP_STG] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ACP_STG] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ACP_STG] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ACP_STG] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ACP_STG] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ACP_STG] SET  MULTI_USER 
GO
ALTER DATABASE [ACP_STG] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ACP_STG] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ACP_STG] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ACP_STG] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ACP_STG', N'ON'
GO
USE [ACP_STG]
GO
/****** Object:  User [TRG\Production Support]    Script Date: 1/26/2017 9:23:01 AM ******/
CREATE USER [TRG\Production Support] FOR LOGIN [TRG\Production Support]
GO
/****** Object:  Table [dbo].[claims_budget]    Script Date: 1/26/2017 9:23:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[claims_budget](
	[Acuity_Id] [varchar](50) NULL,
	[Work_Matter_Id] [varchar](50) NULL,
	[Work Matter] [varchar](100) NULL,
	[Firm_ID] [varchar](50) NULL,
	[Firm_Name] [varchar](100) NULL,
	[Year] [varchar](50) NULL,
	[Budget_Status] [varchar](50) NULL,
	[Task_Activity_Expenses_Code] [varchar](50) NULL,
	[Budget] [varchar](50) NULL,
	[Remaining_Budget] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[claims_invoice_details]    Script Date: 1/26/2017 9:23:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[claims_invoice_details](
	[Invoice_Number] [varchar](300) NULL,
	[Acuity_Id] [varchar](50) NULL,
	[Firm_Id] [varchar](50) NULL,
	[Item_Number] [varchar](50) NULL,
	[Item_Date] [varchar](50) NULL,
	[TK_Id] [varchar](50) NULL,
	[TK_Name] [varchar](50) NULL,
	[Task_Code] [varchar](50) NULL,
	[Activity_Code] [varchar](50) NULL,
	[Expense_Code] [varchar](50) NULL,
	[Cost_Share] [varchar](50) NULL,
	[Upload_Units] [varchar](50) NULL,
	[Upload_Rate] [varchar](50) NULL,
	[Upload_Total] [varchar](50) NULL,
	[Submitted_Units] [varchar](50) NULL,
	[Submitted_Rate] [varchar](50) NULL,
	[Submitted_Total] [varchar](50) NULL,
	[Approved_Units] [varchar](50) NULL,
	[Approved_Rate] [varchar](50) NULL,
	[Approved_Total] [varchar](50) NULL,
	[Descrption] [varchar](6000) NULL,
	[Comment] [varchar](3000) NULL,
	[Timekeeper_Role] [varchar](50) NULL,
	[Timekeeper_Rate] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[claims_invoices]    Script Date: 1/26/2017 9:23:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[claims_invoices](
	[Invoice_Number] [varchar](300) NULL,
	[Invoice_Status] [varchar](50) NULL,
	[Batch_Number] [varchar](50) NULL,
	[Batch_Status] [varchar](50) NULL,
	[Acuity_Id] [varchar](50) NULL,
	[Work_Matter_Id] [varchar](50) NULL,
	[Matter_Name] [varchar](300) NULL,
	[Firm_Id] [varchar](50) NULL,
	[Firm_Name] [varchar](300) NULL,
	[Pay_To_Name] [varchar](300) NULL,
	[Invoice_Date] [varchar](50) NULL,
	[Current_Reviewer] [varchar](50) NULL,
	[Cost_Share] [varchar](50) NULL,
	[Upload_Amount] [varchar](50) NULL,
	[Submitted_Amount] [varchar](50) NULL,
	[Approved_Amount] [varchar](50) NULL,
	[Batch_Upload_Amount] [varchar](50) NULL,
	[Batch_Submitted_Amount] [varchar](50) NULL,
	[Batch_Approved_Amount] [varchar](50) NULL,
	[Upload_Date] [varchar](50) NULL,
	[Submitted_Date] [varchar](50) NULL,
	[Final_Approved_Date] [varchar](50) NULL,
	[Check_Number] [varchar](50) NULL,
	[Paid_Date] [varchar](50) NULL,
	[Paid_Amount] [varchar](50) NULL,
	[Tax_Id] [varchar](50) NULL,
	[Global_Vendor_Number] [varchar](50) NULL,
	[Vendor_Address_1] [varchar](100) NULL,
	[Vendor_Address_2] [varchar](100) NULL,
	[Vendor_City] [varchar](50) NULL,
	[Vendor_State] [varchar](50) NULL,
	[Vendor_Zip] [varchar](50) NULL,
	[IsDeleted] [varchar](50) NULL,
	[Telephone_Number] [varchar](50) NULL,
	[Contact_Name] [varchar](50) NULL,
	[Contact_Email_Address] [varchar](100) NULL,
	[Last_Approver_Id] [varchar](50) NULL,
	[Last_Approver_Name] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[claims_matter]    Script Date: 1/26/2017 9:23:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[claims_matter](
	[Matter_Name] [varchar](300) NULL,
	[Practice_Area] [varchar](50) NULL,
	[Account_Claim_Type] [varchar](50) NULL,
	[Acuity_Id] [varchar](50) NULL,
	[Account] [varchar](100) NULL,
	[Work_Matter] [varchar](100) NULL,
	[Work_Matter_Id] [varchar](50) NULL,
	[Litigation_Matter_Id] [varchar](50) NULL,
	[Manager] [varchar](50) NULL,
	[Supervisor] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[Status_Date] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[Claim_Type] [varchar](50) NULL,
	[Matter_Type] [varchar](50) NULL,
	[Firm_Id] [varchar](50) NULL,
	[Firm_Name] [varchar](100) NULL,
	[Firm_Cost_Share] [varchar](50) NULL,
	[Docket_Number] [varchar](50) NULL,
	[Docket_Id] [varchar](50) NULL,
	[Supporting_Manager] [varchar](50) NULL,
	[Insured] [varchar](100) NULL,
	[Jurisdiction] [varchar](50) NULL,
	[Loss_Reserve] [varchar](50) NULL,
	[Dj_Reserve] [varchar](50) NULL,
	[Definese_Expense_Reserve] [varchar](50) NULL,
	[Loss_Paid] [varchar](50) NULL,
	[Non_DJ_Expense_Paid] [varchar](50) NULL,
	[DJ_Expense_Paid] [varchar](50) NULL,
	[Total_Loss_Incurred] [varchar](50) NULL,
	[Affiliate] [varchar](100) NULL,
	[Special_Tracking_Group] [varchar](50) NULL,
	[Disease_Type] [varchar](50) NULL,
	[Claim_Number] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[claims_utilities]    Script Date: 1/26/2017 9:23:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[claims_utilities](
	[User_Id] [varchar](50) NULL,
	[User] [varchar](50) NULL,
	[Supervisor_Id] [varchar](50) NULL,
	[Supervisor] [varchar](50) NULL,
	[Approval_Limit] [varchar](100) NULL,
	[Group] [varchar](400) NULL,
	[Status] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[masstort_budget]    Script Date: 1/26/2017 9:23:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[masstort_budget](
	[Acuity_Id] [varchar](50) NULL,
	[Work_Matter_Id] [varchar](50) NULL,
	[Work Matter] [varchar](100) NULL,
	[Firm_ID] [varchar](50) NULL,
	[Firm_Name] [varchar](100) NULL,
	[Year] [varchar](50) NULL,
	[Budget_Status] [varchar](50) NULL,
	[Task_Activity_Expenses_Code] [varchar](50) NULL,
	[Budget] [varchar](50) NULL,
	[Remaining_Budget] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[masstort_invoice_details]    Script Date: 1/26/2017 9:23:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[masstort_invoice_details](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Invoice_Number] [varchar](5000) NULL,
	[Acuity_Id] [varchar](50) NULL,
	[Firm_Id] [varchar](50) NULL,
	[Item_Number] [varchar](50) NULL,
	[Item_Date] [varchar](50) NULL,
	[TK_Id] [varchar](50) NULL,
	[TK_Name] [varchar](50) NULL,
	[Task_Code] [varchar](50) NULL,
	[Activity_Code] [varchar](50) NULL,
	[Expense_Code] [varchar](50) NULL,
	[Cost_Share] [varchar](50) NULL,
	[Upload_Units] [varchar](50) NULL,
	[Upload_Rate] [varchar](50) NULL,
	[Upload_Total] [varchar](50) NULL,
	[Submitted_Units] [varchar](50) NULL,
	[Submitted_Rate] [varchar](50) NULL,
	[Submitted_Total] [varchar](50) NULL,
	[Approved_Units] [varchar](50) NULL,
	[Approved_Rate] [varchar](50) NULL,
	[Approved_Total] [varchar](50) NULL,
	[Descrption] [varchar](6000) NULL,
	[Comment] [varchar](3000) NULL,
	[Timekeeper_Role] [varchar](50) NULL,
	[Timekeeper_Rate] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[masstort_invoices]    Script Date: 1/26/2017 9:23:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[masstort_invoices](
	[Invoice_Number] [varchar](300) NULL,
	[Invoice_Status] [varchar](50) NULL,
	[Batch_Number] [varchar](50) NULL,
	[Batch_Status] [varchar](50) NULL,
	[Acuity_Id] [varchar](50) NULL,
	[Work_Matter_Id] [varchar](50) NULL,
	[Matter_Name] [varchar](300) NULL,
	[Firm_Id] [varchar](50) NULL,
	[Firm_Name] [varchar](300) NULL,
	[Pay_To_Name] [varchar](300) NULL,
	[Invoice_Date] [varchar](50) NULL,
	[Current_Reviewer] [varchar](50) NULL,
	[Cost_Share] [varchar](50) NULL,
	[Upload_Amount] [varchar](50) NULL,
	[Submitted_Amount] [varchar](50) NULL,
	[Approved_Amount] [varchar](50) NULL,
	[Batch_Upload_Amount] [varchar](50) NULL,
	[Batch_Submitted_Amount] [varchar](50) NULL,
	[Batch_Approved_Amount] [varchar](50) NULL,
	[Upload_Date] [varchar](50) NULL,
	[Submitted_Date] [varchar](50) NULL,
	[Final_Approved_Date] [varchar](50) NULL,
	[Check_Number] [varchar](50) NULL,
	[Paid_Date] [varchar](50) NULL,
	[Paid_Amount] [varchar](50) NULL,
	[Tax_Id] [varchar](50) NULL,
	[Global_Vendor_Number] [varchar](50) NULL,
	[Vendor_Address_1] [varchar](100) NULL,
	[Vendor_Address_2] [varchar](100) NULL,
	[Vendor_City] [varchar](50) NULL,
	[Vendor_State] [varchar](50) NULL,
	[Vendor_Zip] [varchar](50) NULL,
	[IsDeleted] [varchar](50) NULL,
	[Telephone_Number] [varchar](50) NULL,
	[Contact_Name] [varchar](50) NULL,
	[Contact_Email_Address] [varchar](100) NULL,
	[Last_Approver_Id] [varchar](50) NULL,
	[Last_Approver_Name] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[masstort_matter]    Script Date: 1/26/2017 9:23:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[masstort_matter](
	[Matter_Name] [varchar](300) NULL,
	[Practice_Area] [varchar](50) NULL,
	[Account_Claim_Type] [varchar](50) NULL,
	[Acuity_Id] [varchar](50) NULL,
	[Account] [varchar](100) NULL,
	[Work_Matter] [varchar](100) NULL,
	[Work_Matter_Id] [varchar](50) NULL,
	[Litigation_Matter_Id] [varchar](50) NULL,
	[Manager] [varchar](50) NULL,
	[Supervisor] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[Status_Date] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[Claim_Type] [varchar](50) NULL,
	[Matter_Type] [varchar](50) NULL,
	[Firm_Id] [varchar](50) NULL,
	[Firm_Name] [varchar](100) NULL,
	[Firm_Cost_Share] [varchar](50) NULL,
	[Docket_Number] [varchar](50) NULL,
	[Docket_Id] [varchar](50) NULL,
	[Supporting_Manager] [varchar](50) NULL,
	[Insured] [varchar](100) NULL,
	[Jurisdiction] [varchar](50) NULL,
	[Loss_Reserve] [varchar](50) NULL,
	[Dj_Reserve] [varchar](50) NULL,
	[Definese_Expense_Reserve] [varchar](50) NULL,
	[Loss_Paid] [varchar](50) NULL,
	[Non_DJ_Expense_Paid] [varchar](50) NULL,
	[DJ_Expense_Paid] [varchar](50) NULL,
	[Total_Loss_Incurred] [varchar](50) NULL,
	[Affiliate] [varchar](100) NULL,
	[Special_Tracking_Group] [varchar](50) NULL,
	[Disease_Type] [varchar](50) NULL,
	[Claim_Number] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[masstort_matter_new]    Script Date: 1/26/2017 9:23:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[masstort_matter_new](
	[Matter_Name] [varchar](300) NULL,
	[Practice_Area] [varchar](50) NULL,
	[Account_Claim_Type] [varchar](50) NULL,
	[Acuity_Id] [varchar](50) NULL,
	[Account] [varchar](100) NULL,
	[Work_Matter] [varchar](100) NULL,
	[Work_Matter_Id] [varchar](50) NULL,
	[Lit_Matter_Id] [varchar](50) NULL,
	[Manager] [varchar](50) NULL,
	[Supervisor] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[WM_Open_Date] [varchar](50) NULL,
	[WM_Close_Date] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[Claim_Type] [varchar](50) NULL,
	[Matter_Type] [varchar](50) NULL,
	[Claim_Number] [varchar](50) NULL,
	[Decedent_First_Name] [varchar](50) NULL,
	[Decedent_Middle_Name] [varchar](50) NULL,
	[Decedent_Last_Name] [varchar](50) NULL,
	[Decedent_Suffix] [varchar](50) NULL,
	[Case_Name] [varchar](150) NULL,
	[Date_of_First_Exposure] [varchar](50) NULL,
	[Date_of_Last_Exposure] [varchar](50) NULL,
	[SSN] [varchar](20) NULL,
	[Year_of_Birth_Decedent] [varchar](20) NULL,
	[Decedent_Deceased] [varchar](5) NULL,
	[Year_of_Death_Decedent] [varchar](20) NULL,
	[Disease_Type] [varchar](50) NULL,
	[Other_Cancer_Type] [varchar](50) NULL,
	[Firm_Id] [varchar](50) NULL,
	[Firm_Name] [varchar](100) NULL,
	[Firm_Cost_Share] [varchar](50) NULL,
	[Opposing_Counsel] [varchar](50) NULL,
	[Docket_Number] [varchar](50) NULL,
	[Docket_Id] [varchar](50) NULL,
	[Supporting_Manager] [varchar](50) NULL,
	[Insured] [varchar](100) NULL,
	[Jurisdiction] [varchar](50) NULL,
	[Resolution_Type] [varchar](50) NULL,
	[Close_Date] [varchar](50) NULL,
	[Resolution_Date] [varchar](50) NULL,
	[Initial_Assessment_Value] [varchar](50) NULL,
	[Total_Resolution_Amount] [varchar](50) NULL,
	[Trial_Date] [varchar](50) NULL,
	[Demand] [varchar](50) NULL,
	[Loss_Reserve] [varchar](50) NULL,
	[Dj_Reserve] [varchar](50) NULL,
	[Defense_Expense_Reserve] [varchar](50) NULL,
	[Loss_Paid] [varchar](50) NULL,
	[Non_DJ_Expense_Paid] [varchar](50) NULL,
	[DJ_Expense_Paid] [varchar](50) NULL,
	[Total_Loss_Incurred] [varchar](50) NULL,
	[Affiliate] [varchar](100) NULL,
	[Special_Tracking_Group] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[masstort_utilities]    Script Date: 1/26/2017 9:23:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[masstort_utilities](
	[User_Id] [varchar](50) NULL,
	[User] [varchar](50) NULL,
	[Supervisor_Id] [varchar](50) NULL,
	[Supervisor] [varchar](50) NULL,
	[Approval_Limit] [varchar](100) NULL,
	[Group] [varchar](300) NULL,
	[Status] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC [ACP_STG].sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Acuity Claimant Processing - Staging database' 
GO
USE [master]
GO
ALTER DATABASE [ACP_STG] SET  READ_WRITE 
GO
