
Use ServiceDesk;
go


IF OBJECT_ID('rpt.GetServiceDeskRequests','P') IS NOT NULL
    DROP PROCEDURE rpt.GetServiceDeskRequests;
go

CREATE PROCEDURE rpt.GetServiceDeskRequests (
	   @Department VARCHAR(200),
	   @ManagerList VARCHAR(MAX),
	   @ManagerListSelectedCount INT = 0
)
AS
/*
    Name:		 rpt.GetServiceDeskRequests
    Example:	 EXEC rpt.GetServiceDeskRequests @Department = 'Reinsurance', @ManagerList = 'S-1-5-21-129303055-1531586736-945835055-2518,S-1-5-21-129303055-1531586736-945835055-16551,S-1-5-21-129303055-1531586736-945835055-1620'
			 EXEC rpt.GetServiceDeskRequests @Department = 'Reinsurance', @ManagerList = 'S-1-5-21-129303055-1531586736-945835055-2518,S-1-5-21-129303055-1531586736-945835055-16551,S-1-5-21-129303055-1531586736-945835055-1620', @ManagerListSelectedCount = 9
    Modification History
    Date		 Name		  Description
    02/18/2016	 Pburke		  Initial Create
*/
BEGIN
SET NOCOUNT ON;

    BEGIN TRY
	   DECLARE @AllInd BIT;
	   SET @AllInd = 0;

	   IF @Department = 'Reinsurance'
	   BEGIN
		  DECLARE @tbl TABLE (ActiveDirectoryID varchar(200), 
						  Name varchar(200), 
						  computedLevel int, 
						  sort varchar(500)); 
		  DECLARE @ttlCount INT; 
		  DECLARE @mgrList VARCHAR(MAX); 
 				    
		  INSERT INTO @tbl (ActiveDirectoryID, Name, computedlevel, sort) 
		  EXEC rpt.spGetDepartmentHierarchy @Department ;

		  SELECT @ttlCount = Count(*) FROM @tbl; 

		  IF @ttlCount = @ManagerListSelectedCount
			 SET @AllInd = 1;
	   END
    
	   ;WITH mgrString AS (
		  SELECT CAST('<root><x>' + Replace( @ManagerList, ',' , '</x><x>') + '</x></root>' AS XML) as Ids 
	    ),
	    mgrList AS (  
		   SELECT Mgrs.ManagerId,  A.AssociateId, A.DisplayName, A.SamAccountName 
		   FROM ( 
    			 SELECT mgrs.mgr.value('.', 'varchar(max)') as ManagerId 
    			 FROM mgrString 
    			 CROSS APPLY Ids.nodes('/root/x') AS mgrs(mgr) 
    		   ) Mgrs 
		  JOIN dbo.Associate a ON Mgrs.ManagerId = A.ActiveDirectoryId  
	    ), 
	    mgrListInclude AS (
		  SELECT  ml.ManagerId as SupervisorActiveDirectoryId,  
			 ml.AssociateId as SupervisorId,  
			 ml.DisplayName as SupervisorName, 
			 inc.EmailAddress 
		  FROM mgrList ml 
		  CROSS APPLY [rpt].[fnEmailUnderManagersIncludingManagers](DATEADD(DAY,-2,GETDATE()),GETDATE(),ml.ManagerId) inc 
	    )
	    SELECT  
		   [WorkOrderID],
		   [Requester],
		   [CreatedTime] ,
		   rpt.fnStripFormatting([ShortDescription]) as ShortDescription, 
		   [Subject],
		   [Technician],
		   [Status],
		   [ServiceGroup],
		   [Category],
		   [Subcategory],
		   [Item],
		   [Priority],
		   A.[ActiveDirectoryID] 
	    FROM [ServiceDesk].[dbo].[Request]    R 
		  INNER JOIN [ServiceDesk].[dbo].[Associate]  A ON R.Requester = A.AdjustedName  
											  AND GETDATE() <= ISNULL(A.EndDate, GETDATE())
	    WHERE Status <> 'Closed' 
		  AND 1 = CASE 
				 WHEN @AllInd = 0 AND EXISTS( SELECT 1
										FROM mgrListInclude mli
										WHERE mli.EmailAddress = R.RequesterEmail ) THEN 1
				 WHEN @AllInd = 1 AND R.Department = @Department THEN 1
				 WHEN @Department = 'Other' AND (ISNULL(A.Supervisor, '') = '') THEN 1
				 ELSE 0
				END
	    ORDER BY [CreatedTime] ASC
    END TRY
    BEGIN CATCH
	   THROW;
    END CATCH


END