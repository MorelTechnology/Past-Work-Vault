USE [ServiceDesk]
GO

/****** Object:  StoredProcedure [rpt].[spConvertADIDStoListOfNames]    Script Date: 2/15/2016 4:12:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =====================================================================
-- Author:		Scott Marcus
-- Create date: July 8, 2015
-- Description:	Turn a comma delimited list of active directory ID's
--              into a comma-space delimited list of adjust names
--              with ' and ' in place of the last ', '
-- =====================================================================
CREATE PROCEDURE [rpt].[spConvertADIDStoListOfNames]
@managers as VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	-- CREATE A TABLE VARIABLE WITH ONE COLUMN FOR NAME
	DECLARE @Names TABLE  (name varchar(200))

	-- SPLIT THE COMMA DELIMITED LIST OF ACTIVE DIRECTORY ID'S
	-- POPULATE THE TABLE VARIABLE WITH ADJUSTED NAMES  (FIRST LAST)
	insert into @Names
	SELECT Distinct AdjustedName FROM Associate AS b 
	WHERE b.ActiveDirectoryID in (SELECT * FROM rpt.SplitString(@managers, ','))

	-- TURN THE TABLE INTO A COMMA-SPACE DELIMITED STRING
	DECLARE @listStr VARCHAR(MAX)
	SELECT @listStr = COALESCE(@listStr+', ' ,'') + Name FROM @Names

	-- REPLACE THE LAST ', ' WITH ' and ' IF THERE IS ONE
	select (case when charindex(',',reverse(@listStr))>0
	then
	replace(reverse(replace(STUFF(reverse(@listStr),charindex(',',reverse(@listStr)),0,'#'),'#,',' dna ')),'and  ','and ')
	else 
	@listStr
	end) as Names

END

GO


