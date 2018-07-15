USE [ServiceCatalog]
GO

/****** Object:  Table [dbo].[CatalogService]    Script Date: 4/13/2015 2:18:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CatalogService](
	[CatalogID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
 CONSTRAINT [PK_GroupItem] PRIMARY KEY CLUSTERED 
(
	[CatalogID] ASC,
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CatalogService]  WITH CHECK ADD  CONSTRAINT [FK_GroupItem_Group] FOREIGN KEY([CatalogID])
REFERENCES [dbo].[Catalog] ([CatalogID])
GO

ALTER TABLE [dbo].[CatalogService] CHECK CONSTRAINT [FK_GroupItem_Group]
GO

ALTER TABLE [dbo].[CatalogService]  WITH CHECK ADD  CONSTRAINT [FK_GroupItem_Item] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Service] ([ServiceID])
GO

ALTER TABLE [dbo].[CatalogService] CHECK CONSTRAINT [FK_GroupItem_Item]
GO


