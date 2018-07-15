USE [reporting]
GO

/****** Object:  Table [dbo].[WINSDataLoad]    Script Date: 1/2/2018 12:10:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[WINSDataLoad](
	[WINSDataLoadID] [int] IDENTITY(1,1) NOT NULL,
	[Branch] [char](5) NULL,
	[Co] [char](4) NULL,
	[Policy] [char](15) NULL,
	[EffDte] [date] NULL,
	[InsNam] [varchar](70) NULL,
	[AccDte] [date] NULL,
	[AccSte] [varchar](2) NULL,
	[ClDesc] [varchar](150) NULL,
	[PrmSte] [char](2) NULL,
	[Dac] [char](1) NULL,
	[Claim] [char](15) NULL,
	[Clmnt] [char](2) NULL,
	[Examnr] [varchar](5) NULL,
	[Idadd] [char](4) NULL,
	[Redept] [char](5) NULL,
	[Compan] [varchar](5) NULL,
	[Prod] [char](3) NULL,
	[Aslob] [char](3) NULL,
	[Burlin] [char](3) NULL,
	[ActDte] [char](6) NULL,
	[Treaty] [char](6) NULL,
	[ReinCo] [char](4) NULL,
	[Affil] [char](1) NULL,
	[LossPaid] [decimal](14, 2) NULL,
	[ExpPaid] [decimal](14, 2) NULL,
	[LossReserve] [decimal](14, 2) NULL,
	[ExpReserve] [decimal](14, 2) NULL,
	[ValuationDate] [date] NULL,
 CONSTRAINT [PK_WINSDataLoad] PRIMARY KEY NONCLUSTERED 
(
	[WINSDataLoadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


