USE [SSISDB]
GO
/****** Object:  Trigger [trg_CurrentValDate_Update_IO]    Script Date: 10/30/2017 1:45:51 PM ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[custom].[trg_CurrentValDate_Update_IO]'))
DROP TRIGGER [custom].[trg_CurrentValDate_Update_IO]
GO
/****** Object:  StoredProcedure [custom].[CurrentValuationDate_Update]    Script Date: 10/30/2017 1:45:51 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[custom].[CurrentValuationDate_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [custom].[CurrentValuationDate_Update]
GO
/****** Object:  StoredProcedure [custom].[CurrentValuationDate_GET]    Script Date: 10/30/2017 1:45:51 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[custom].[CurrentValuationDate_GET]') AND type in (N'P', N'PC'))
DROP PROCEDURE [custom].[CurrentValuationDate_GET]
GO
/****** Object:  Table [custom].[CurrentValuationDate]    Script Date: 10/30/2017 1:45:51 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[custom].[CurrentValuationDate]') AND type in (N'U'))
DROP TABLE [custom].[CurrentValuationDate]
GO
/****** Object:  Table [custom].[CurrentValuationDate]    Script Date: 10/30/2017 1:45:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[custom].[CurrentValuationDate]') AND type in (N'U'))
BEGIN
CREATE TABLE [custom].[CurrentValuationDate](
	[CurrentDate] [date] NOT NULL,
	[ValuationDate]  AS (CONVERT([date],eomonth([CurrentDate]))),
 CONSTRAINT [PK_custom.CurrentValuationDate] PRIMARY KEY CLUSTERED 
(
	[CurrentDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  StoredProcedure [custom].[CurrentValuationDate_GET]    Script Date: 10/30/2017 1:45:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[custom].[CurrentValuationDate_GET]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [custom].[CurrentValuationDate_GET] AS' 
END
GO
ALTER PROCEDURE [custom].[CurrentValuationDate_GET] 

AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT TOP(1) ValuationDate
	FROM custom.CurrentValuationDate
END


GO
/****** Object:  StoredProcedure [custom].[CurrentValuationDate_Update]    Script Date: 10/30/2017 1:45:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[custom].[CurrentValuationDate_Update]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [custom].[CurrentValuationDate_Update] AS' 
END
GO

-- =============================================
-- Author:		John Roche
-- Create date: 2017-10-16
-- Description:	Update the current RSG Valuation Date based on the system date of execution
-- =============================================
ALTER PROCEDURE [custom].[CurrentValuationDate_Update] 

AS
BEGIN
	
	SET NOCOUNT ON;
	IF NOT EXISTS(SELECT NULL FROM custom.CurrentValuationDate)

	INSERT INTO custom.CurrentValuationDate
           (CurrentDate)
     VALUES
           (CAST(GETDATE() as DATE))
	
ELSE

	UPDATE custom.CurrentValuationDate
	SET CurrentDate= CAST(GETDATE() as DATE)
END

GO
/****** Object:  Trigger [custom].[trg_CurrentValDate_Update_IO]    Script Date: 10/30/2017 1:45:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[custom].[trg_CurrentValDate_Update_IO]'))
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		John Roche
-- Create date: 2017-10-16
-- Description:	Prevent table inserts if a row exists
-- =============================================
CREATE TRIGGER [custom].[trg_CurrentValDate_Update_IO] 
   ON  [custom].[CurrentValuationDate] 
   INSTEAD OF  INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   IF EXISTS(Select 1 from custom.CurrentValuationDate)
		UPDATE custom.CurrentValuationDate
		SET CurrentDate = Cast(GETDATE() AS date)
   ELSE
    INSERT INTO custom.CurrentValuationDate(CurrentDate)

	SELECT CAST (GETDATE() AS date)


END
' 
GO
