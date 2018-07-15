USE [ServiceCatalog]
GO

/****** Object:  Table [dbo].[Catalog]    Script Date: 4/13/2015 2:18:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Catalog](
	[CatalogID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ButtonTitle] [varchar](50) NOT NULL,
	[ButtonIcon] [varchar](50) NOT NULL,
	[ButtonText] [varchar](1000) NOT NULL,
	[PageDescription] [varchar](1000) NOT NULL,
	[LargeAreaDescription] [varchar](2000) NOT NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[CatalogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


