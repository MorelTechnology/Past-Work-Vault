USE [ServiceCatalog]
GO

/****** Object:  Table [dbo].[ServiceLevelAgreement]    Script Date: 4/13/2015 2:19:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ServiceLevelAgreement](
	[ServiceLevelAgreementID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Description] [varchar](1000) NOT NULL,
	[BusinessHours] [int] NULL,
 CONSTRAINT [PK_ServiceLevelAgreement] PRIMARY KEY CLUSTERED 
(
	[ServiceLevelAgreementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


