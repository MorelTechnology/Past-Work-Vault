IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RI].[TruncateTablesInRISchema]') AND type in (N'P', N'PC'))
DROP PROCEDURE [RI].[TruncateTablesInRISchema]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RI].[TruncateTablesInRISchema]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [RI].[TruncateTablesInRISchema] AS' 
END
GO
-- =============================================
-- Author:		John Roche
-- Create date: 2017-10-16
-- Description:	Cursor truncate the tables in RI schema
-- =============================================
ALTER PROCEDURE RI.TruncateTablesInRISchema 

AS
BEGIN

DECLARE @table_name NVARCHAR(50) -- database name 
DECLARE @SQL NVARCHAR(250)
 
DECLARE table_cursor CURSOR FOR  
SELECT s.name + '.' + t.name 
  FROM sys.tables AS t
  INNER JOIN sys.schemas AS s
  ON t.[schema_id] = s.[schema_id]
  WHERE s.name = N'RI'

OPEN table_cursor   
FETCH NEXT FROM table_cursor INTO @table_name   

WHILE @@FETCH_STATUS = 0   
BEGIN  

	SET @sql = N'TRUNCATE TABLE ' + @table_name
	EXEC sp_executesql @sql
	RAISERROR (N'Table  %s was truncated', -- Message text.  
			   10, -- Severity,  
			   1, -- State,  
			   @table_name)

	FETCH NEXT FROM table_cursor INTO @table_name    
END   

CLOSE table_cursor   
DEALLOCATE table_cursor


END