USE [ServiceCatalog]
GO

/****** Object:  Table [dbo].[Task]    Script Date: 4/13/2015 2:19:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Task](
	[TaskID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceID] [int] NOT NULL,
	[ServiceLevelAgreementID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[ForBusinessAssoc] [bit] NOT NULL,
	[ForOperationsAssoc] [bit] NOT NULL,
	[ForITAssoc] [bit] NOT NULL,
	[ServiceDeskCategory] [varchar](100) NOT NULL,
	[ServiceDeskSubCategory] [varchar](100) NOT NULL,
	[ServiceDeskItem] [varchar](100) NOT NULL,
	[Description] [varchar](2000) NOT NULL,
	[Comment] [varchar](1000) NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Item] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Service] ([ServiceID])
GO

ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Item]
GO

ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_ServiceLevelAgreement] FOREIGN KEY([ServiceLevelAgreementID])
REFERENCES [dbo].[ServiceLevelAgreement] ([ServiceLevelAgreementID])
GO

ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_ServiceLevelAgreement]
GO


