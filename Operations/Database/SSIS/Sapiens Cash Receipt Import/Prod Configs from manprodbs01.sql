/****** Script for SelectTopNRows command from SSMS  ******/
SELECT AP.[AppPackageID]
      ,A.ApplicationName
      ,P.PackageName
	  ,P.PackageFolder
	  ,PC.ConfiguredValue
	  ,PC.PackagePath
      ,AP.[ExecutionOrder]
  FROM [SSISConfig].[cfg].[AppPackages] AP
  JOIN [SSISConfig].[cfg].[Applications] A on A.ApplicationID = AP.ApplicationID
  JOIN [SSISConfig].[cfg].[Packages] P on P.PackageID = AP.PackageID
  JOIN [SSISConfig].[cfg].[PackageConfigs] PC on PC.ConfigurationFilter = A.ApplicationName
  where A.ApplicationID = 12