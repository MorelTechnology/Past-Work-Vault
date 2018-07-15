/****** [Create Database 'DSWorkRequest'] ***************************************************************************************/
USE [master]

CREATE DATABASE [DSWorkRequest] CONTAINMENT = NONE
ON  PRIMARY ( NAME = N'DSWorkRequest', FILENAME = N'E:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\DSWorkRequest.mdf' , SIZE = 65536KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
LOG ON ( NAME = N'DSWorkRequest_log', FILENAME = N'E:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\DSWorkRequest_log.ldf' , SIZE = 20480KB , MAXSIZE = 2048GB , FILEGROWTH = 20480KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled')) begin EXEC [DSWorkRequest].[dbo].[sp_fulltext_database] @action = 'enable' end

USE [DSWorkRequest]
ALTER DATABASE [DSWorkRequest] SET COMPATIBILITY_LEVEL = 130;
ALTER DATABASE [DSWorkRequest] SET ANSI_NULL_DEFAULT OFF;
ALTER DATABASE [DSWorkRequest] SET ANSI_NULLS OFF;
ALTER DATABASE [DSWorkRequest] SET ANSI_PADDING OFF;
ALTER DATABASE [DSWorkRequest] SET ANSI_WARNINGS OFF;
ALTER DATABASE [DSWorkRequest] SET ARITHABORT OFF;
ALTER DATABASE [DSWorkRequest] SET AUTO_CLOSE OFF;
ALTER DATABASE [DSWorkRequest] SET AUTO_SHRINK OFF;
ALTER DATABASE [DSWorkRequest] SET AUTO_UPDATE_STATISTICS ON;
ALTER DATABASE [DSWorkRequest] SET CURSOR_CLOSE_ON_COMMIT OFF;
ALTER DATABASE [DSWorkRequest] SET CURSOR_DEFAULT  GLOBAL;
ALTER DATABASE [DSWorkRequest] SET CONCAT_NULL_YIELDS_NULL OFF;
ALTER DATABASE [DSWorkRequest] SET NUMERIC_ROUNDABORT OFF;
ALTER DATABASE [DSWorkRequest] SET QUOTED_IDENTIFIER OFF;
ALTER DATABASE [DSWorkRequest] SET RECURSIVE_TRIGGERS OFF;
ALTER DATABASE [DSWorkRequest] SET  DISABLE_BROKER;
ALTER DATABASE [DSWorkRequest] SET AUTO_UPDATE_STATISTICS_ASYNC OFF;
ALTER DATABASE [DSWorkRequest] SET DATE_CORRELATION_OPTIMIZATION OFF;
ALTER DATABASE [DSWorkRequest] SET TRUSTWORTHY OFF;
ALTER DATABASE [DSWorkRequest] SET ALLOW_SNAPSHOT_ISOLATION OFF;
ALTER DATABASE [DSWorkRequest] SET PARAMETERIZATION SIMPLE;
ALTER DATABASE [DSWorkRequest] SET READ_COMMITTED_SNAPSHOT OFF;
ALTER DATABASE [DSWorkRequest] SET HONOR_BROKER_PRIORITY OFF;
ALTER DATABASE [DSWorkRequest] SET RECOVERY SIMPLE;
ALTER DATABASE [DSWorkRequest] SET  MULTI_USER;
ALTER DATABASE [DSWorkRequest] SET PAGE_VERIFY CHECKSUM ;
ALTER DATABASE [DSWorkRequest] SET DB_CHAINING OFF;
ALTER DATABASE [DSWorkRequest] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF );
ALTER DATABASE [DSWorkRequest] SET TARGET_RECOVERY_TIME = 0 SECONDS;
ALTER DATABASE [DSWorkRequest] SET DELAYED_DURABILITY = DISABLED;
ALTER DATABASE [DSWorkRequest] SET QUERY_STORE = OFF;
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY; 
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
ALTER DATABASE [DSWorkRequest] SET  READ_WRITE;
GO

/****** [Create Schema 'data'] **************************************************************************************************/
USE [DSWorkRequest]
GO
CREATE SCHEMA [data] Authorization [dbo]
GO

/****** [Create Table 'Requests'] ***********************************************************************************************/
USE [DSWorkRequest]
GO
CREATE TABLE [data].[Requests](
	[RequestID] [int] IDENTITY(1,1) NOT NULL,
	[Requestor] [varchar](100) NOT NULL,
	[Manager] [varchar](100) NOT NULL,
	[Title] [varchar](256) NOT NULL,
	[Goal] [varchar](max) NOT NULL,
	[Problem] [varchar](max) NOT NULL,
	[Quantification] [varchar](max) NULL,
	[Benefit] [varchar](max) NOT NULL,
	[Status] [varchar](max) NULL,
	[StatusDate] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[Created] [datetime] NOT NULL,
	[CorporateGoals] [varchar](max) NULL,
	[GoalSupport] [varchar](max) NULL,
	[SupportsDept] [bit] NULL,
	[DeptGoalSupport] [varchar](max) NULL,
	[ConditionsOfSatisfaction] [varchar](max) NULL,
	[BusinessValueUnit] [varchar](max) NOT NULL,
	[BusinessValueAmount] [decimal](19, 2) NOT NULL,
	[RequestedCompletionDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Requests] PRIMARY KEY CLUSTERED 
(
	[RequestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [data].[Requests] ADD  CONSTRAINT [DF_Requests_BusinessValueUnit]  DEFAULT ('Unknown') FOR [BusinessValueUnit]
GO

ALTER TABLE [data].[Requests] ADD  CONSTRAINT [DF_Requests_BusinessValueAmount]  DEFAULT ((0.00)) FOR [BusinessValueAmount]
GO

ALTER TABLE [data].[Requests] ADD  CONSTRAINT [DF_Requests_RequestedCompletionDate]  DEFAULT (getdate()) FOR [RequestedCompletionDate]
GO

/****** [Create Table 'Error'] *************************************************************************************************/
USE [DSWorkRequest]
GO
CREATE TABLE [data].[Error](
	[LocalizedDate] [datetime] NOT NULL,
	[CurrentUser] [varchar](50) NULL,
	[IsAuthenticated] [bit] NOT NULL,
	[CurrentUrl] [varchar](255) NULL,
	[ExceptionMessage] [varchar](max) NULL,
	[StackTrace] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** [Create Table 'Configuration'] *************************************************************************************************/
USE [DSWorkRequest]
GO
CREATE TABLE [data].[Configuration](
	[SMTP_Server] [varchar](50) NULL,
	[SMTP_Port] [int] NULL,
	[Single_Request_Url] [varchar](255) NULL,
	[Digital_Strategy_Users] [varchar](max) NULL,
	[Access_Override_Users] [varchar](max) NULL,
	[Allowed_User_Groups] [varchar](max) NULL,
	[Prohibited_User_Groups] [varchar](max) NULL,
	[Allowed_QA_Users] [varchar](max) NULL,
	[Prohibited_QA_Users] [varchar](max) NULL,
	[Corporate_Goals] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** [Create Table 'Comments'] *************************************************************************************************/

CREATE TABLE [data].[Comments](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[WorkRequestID] [int] NOT NULL,
	[CommentDate] [datetime] NOT NULL,
	[CommentUser] [varchar](255) NOT NULL,
	[CommentText] [varchar](max) NOT NULL,
 CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** [Create SQL Login and Database User for AD Service Account] ******************************************************************************/
USE [master]
GO
IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = 'TRG\DSWR_svc') 
BEGIN CREATE LOGIN [TRG\DSWR_svc] FROM WINDOWS WITH DEFAULT_DATABASE=[DSWorkRequest], DEFAULT_LANGUAGE=[us_english] END
GO

USE [DSWorkRequest]
GO
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'TRG\DSWR_svc')
BEGIN CREATE USER [TRG\DSWR_svc] FOR LOGIN [TRG\DSWR_svc] WITH DEFAULT_SCHEMA=[data] END
GO
Exec sp_addrolemember 'db_owner', 'TRG\DSWR_svc'
GO
