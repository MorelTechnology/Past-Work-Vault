USE [ServiceCatalog]
GO

/****** Object:  Table [dbo].[TaskField]    Script Date: 4/13/2015 2:19:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TaskField](
	[TaskFieldID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NOT NULL,
	[FieldID] [int] NOT NULL,
	[FixedValue] [varchar](200) NULL,
	[PickListID] [int] NULL,
	[Required] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_TaskField] PRIMARY KEY CLUSTERED 
(
	[TaskFieldID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[TaskField]  WITH CHECK ADD  CONSTRAINT [FK_TaskField_Field] FOREIGN KEY([FieldID])
REFERENCES [dbo].[Field] ([FieldID])
GO

ALTER TABLE [dbo].[TaskField] CHECK CONSTRAINT [FK_TaskField_Field]
GO

ALTER TABLE [dbo].[TaskField]  WITH CHECK ADD  CONSTRAINT [FK_TaskField_PickList] FOREIGN KEY([PickListID])
REFERENCES [dbo].[PickList] ([PickListID])
GO

ALTER TABLE [dbo].[TaskField] CHECK CONSTRAINT [FK_TaskField_PickList]
GO

ALTER TABLE [dbo].[TaskField]  WITH CHECK ADD  CONSTRAINT [FK_TaskField_Task] FOREIGN KEY([TaskID])
REFERENCES [dbo].[Task] ([TaskID])
GO

ALTER TABLE [dbo].[TaskField] CHECK CONSTRAINT [FK_TaskField_Task]
GO


