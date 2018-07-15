 Use SSISDB
 Go
 
 
-- Environment Variable CFIGLFiles\CFIGLFiles_Env\CFICaseReservesxls
Declare @CFICaseReservesxls  sql_variant = N'\\mansan02\batchdev$\CFI\CFIGLFiles\CFICaseReserves.xls'

-- Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIPaidsxls
Declare @CFIPaidsxls sql_variant = N'\\mansan02\batchdev$\CFI\CFIGLFiles\CFIPaids.xls'

-- Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIFilesSummaryxls
Declare @CFIFilesSummaryxls sql_variant = N'\\mansan02\batchdev$\CFI\CFIGLFiles\CFIFilesSummary.xls'

-- Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIQuarterSummaryxls
Declare @CFIQuarterSummaryxls sql_variant = N'\\mansan02\batchdev$\CFI\CFIGLFiles\CFIQuarterSummary.xls'

-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFICaseReservesxlsx
Declare @ArchiveCFICaseReservesxlsx  sql_variant = N'\\mansan02\batchdev$\CFI\CFIGLFiles\History\CFICaseReserves'

-- Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIPaidsxls
Declare @ArchiveCFIPaidsxlsx sql_variant = N'\\mansan02\batchdev$\CFI\CFIGLFiles\History\CFIPaids'

-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIFilesSummaryxlsx
Declare @ArchiveCFIFilesSummaryxlsx sql_variant = N'\\mansan02\batchdev$\CFI\CFIGLFiles\History\CFIFilesSummary'

-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIQuarterSummaryxlsx
Declare @ArchiveCFIQuarterSummaryxlsx sql_variant = N'\\mansan02\batchdev$\CFI\CFIGLFiles\History\CFIQuarterSummary'


-- CFIGLFiles_Env ENVIRONMENT VARIABLE VALUES --

 -- Environment Variable CFIGLFiles\CFIGLFiles_Env\RS_ReportingCon
Declare @RS_ReportingCon sql_variant = N'Data Source= DWDev;Initial Catalog=RS_Reporting;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;'

-- Environment Variable CFIGLFiles\CFIGLFiles_Env\SMTPRelayCon
Declare @SMTPRelayCon sql_variant = N'SmtpServer=manrelay01.trg.com;UseWindowsAuthentication=False;EnableSsl=False;Timeout=2000;'


-- Environment Variable CFIGLFiles\CFIGLFiles_Env\CFI_DistributionList
Declare @CFI_DistributionList sql_variant = N'roja_kasula@trg.com'


 -- Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConCaseReserves
Declare @ExcelConCaseReserves sql_variant = N'Data Source=\\mansan02\batchdev$\CFI\CFIGLFiles\CFICaseReserves.xls;Extended Properties="EXCEL 8.0;HDR=YES";'

 -- Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConPaids
Declare @ExcelConPaids sql_variant = N'Data Source=\\mansan02\batchdev$\CFI\CFIGLFiles\CFIPaids.xls;Extended Properties="EXCEL 8.0;HDR=YES";'

-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConFilesSummary
Declare @ExcelConFilesSummary sql_variant = N'Data Source=\\mansan02\batchdev$\CFI\CFIGLFiles\CFIFilesSummary.xls;Extended Properties="EXCEL 8.0;HDR=YES";'


-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConQuarterSummary
Declare @ExcelConQuarterSummary sql_variant = N'Data Source=\\mansan02\batchdev$\CFI\CFIGLFiles\CFIQuarterSummary.xls;Extended Properties="EXCEL 8.0;HDR=YES";'



-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ValuationDate
Declare @ValuationDate sql_variant = N'11-30-2017'




------------------------------

/*
Script Name: C:\Users\rkasu\Desktop\CFIGLFilesEnvironmentScript\CFIGLFiles\4_BIDEVETL01_SSISDB_CFIGLFiles_CFIGLFiles_Env.environment.sql
Generated From Catalog Instance: BIDEVETL01
Catalog Name: SSISDB
Folder Name: CFIGLFiles
Environment Name: CFIGLFiles_Env
Generated By: TRG\rkasu
Generated Date: 10/2/2017 12:59:48 PM
Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP
*/

print 'Script Name: C:\Users\rkasu\Desktop\CFIGLFilesEnvironmentScript\CFIGLFiles\4_BIDEVETL01_SSISDB_CFIGLFiles_CFIGLFiles_Env.environment.sql
Generated From Catalog Instance: BIDEVETL01
Catalog Name: SSISDB
Folder Name: CFIGLFiles
Environment Name: CFIGLFiles_Env
Generated By: TRG\rkasu
Generated Date: 10/2/2017 12:59:48 PM
Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP'
print ''
print '------------------------------------------------------------'
print 'Deployed to Instance: ' + @@servername
print 'Deploy Date: ' + Convert(varchar,GetDate(), 101) + ' ' + Convert(varchar,GetDate(), 108)
print 'Deployed By: ' + original_login()
print '------------------------------------------------------------'
print ''

 -- Environment SSISDB\CFIGLFiles\CFIGLFiles_Env
print 'Environment SSISDB\CFIGLFiles\CFIGLFiles_Env'
If Not Exists(Select * 
           From SSISDB.[catalog].environments e 
           Join SSISDB.[catalog].folders f 
             On f.folder_id = e.folder_id 
           Where e.name = N'CFIGLFiles_Env'
             And f.name = N'CFIGLFiles')

 begin
  print ' - Creating Environment SSISDB\CFIGLFiles\CFIGLFiles_Env'

  Exec SSISDB.[catalog].create_environment 
      @environment_name=N'CFIGLFiles_Env'
    , @folder_name=N'CFIGLFiles'
  print ' - Environment SSISDB\CFIGLFiles\CFIGLFiles_Env created'
 end
else
 print ' - Environment SSISDB\CFIGLFiles\CFIGLFiles_Env already exists.'
print ''

-- Environment Variable CFIGLFiles\CFIGLFiles_Env\CFICaseReservesxls
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\CFICaseReservesxls'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'CFICaseReservesxls' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\CFICaseReservesxls'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'CFICaseReservesxls' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFICaseReservesxls dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\CFICaseReservesxls'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'CFICaseReservesxls'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @CFICaseReservesxls
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFICaseReservesxls created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\CFICaseReservesxls
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\CFICaseReservesxls value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'CFICaseReservesxls' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @CFICaseReservesxls
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFICaseReservesxls value set'
print ''


-- Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIFilesSummaryxls
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIFilesSummaryxls'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'CFIFilesSummaryxls' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIFilesSummaryxls'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'CFIFilesSummaryxls' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIFilesSummaryxls dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIFilesSummaryxls'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'CFIFilesSummaryxls'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @CFIFilesSummaryxls
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIFilesSummaryxls created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIFilesSummaryxls
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIFilesSummaryxls value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'CFIFilesSummaryxls' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @CFIFilesSummaryxls
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIFilesSummaryxls value set'
print ''


-- Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIPaidsxls
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIPaidsxls'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'CFIPaidsxls' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIPaidsxls'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'CFIPaidsxls' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIPaidsxls dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIPaidsxls'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'CFIPaidsxls'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @CFIPaidsxls
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIPaidsxls created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIPaidsxls
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIPaidsxls value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'CFIPaidsxls' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @CFIPaidsxls
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIPaidsxls value set'
print ''


-- Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIQuarterSummaryxls
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIQuarterSummaryxls'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'CFIQuarterSummaryxls' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIQuarterSummaryxls'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'CFIQuarterSummaryxls' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIQuarterSummaryxls dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIQuarterSummaryxls'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'CFIQuarterSummaryxls'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @CFIQuarterSummaryxls
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIQuarterSummaryxls created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIQuarterSummaryxls
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIQuarterSummaryxls value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'CFIQuarterSummaryxls' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @CFIQuarterSummaryxls
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFIQuarterSummaryxls value set'
print ''


-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConCaseReserves
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConCaseReserves'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'ExcelConCaseReserves' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConCaseReserves'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'ExcelConCaseReserves' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConCaseReserves dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConCaseReserves'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'ExcelConCaseReserves'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @ExcelConCaseReserves
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConCaseReserves created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConCaseReserves
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConCaseReserves value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'ExcelConCaseReserves' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @ExcelConCaseReserves
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConCaseReserves value set'
print ''

-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConPaids
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConPaids'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'ExcelConPaids' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConPaids'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'ExcelConPaids' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConPaids dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConPaids'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'ExcelConPaids'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @ExcelConPaids
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConPaids created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConPaids
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConPaids value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'ExcelConPaids' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @ExcelConPaids
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConPaids value set'
print ''



-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConFilesSummary
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConFilesSummary'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'ExcelConFilesSummary' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConFilesSummary'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'ExcelConFilesSummary' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConFilesSummary dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConFilesSummary'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'ExcelConFilesSummary'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @ExcelConFilesSummary
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConFilesSummary created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConFilesSummary
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConFilesSummary value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'ExcelConFilesSummary' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @ExcelConFilesSummary
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConFilesSummary value set'
print ''




-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConQuarterSummary
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConQuarterSummary'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'ExcelConQuarterSummary' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConQuarterSummary'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'ExcelConQuarterSummary' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConQuarterSummary dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConQuarterSummary'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'ExcelConQuarterSummary'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @ExcelConQuarterSummary
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConQuarterSummary created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConQuarterSummary
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConQuarterSummary value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'ExcelConQuarterSummary' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @ExcelConQuarterSummary
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ExcelConQuarterSummary value set'
print ''


-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFICaseReservesxlsx
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFICaseReservesxlsx'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'ArchiveCFICaseReservesxlsx' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFICaseReservesxlsx'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'ArchiveCFICaseReservesxlsx' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFICaseReservesxlsx dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFICaseReservesxlsx'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'ArchiveCFICaseReservesxlsx'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @ArchiveCFICaseReservesxlsx
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFICaseReservesxlsx created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFICaseReservesxlsx
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFICaseReservesxlsx value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'ArchiveCFICaseReservesxlsx' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @ArchiveCFICaseReservesxlsx
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFICaseReservesxlsx value set'
print ''



-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIPaidsxlsx
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIPaidsxlsx'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'ArchiveCFIPaidsxlsx' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIPaidsxlsx'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'ArchiveCFIPaidsxlsx' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIPaidsxlsx dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIPaidsxlsx'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'ArchiveCFIPaidsxlsx'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @ArchiveCFIPaidsxlsx
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIPaidsxlsx created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIPaidsxlsx
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIPaidsxlsx value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'ArchiveCFIPaidsxlsx' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @ArchiveCFIPaidsxlsx
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIPaidsxlsx value set'
print ''


-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIFilesSummaryxlsx
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIFilesSummaryxlsx'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'ArchiveCFIFilesSummaryxlsx' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIFilesSummaryxlsx'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'ArchiveCFIFilesSummaryxlsx' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIFilesSummaryxlsx dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIFilesSummaryxlsx'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'ArchiveCFIFilesSummaryxlsx'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @ArchiveCFIFilesSummaryxlsx
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIFilesSummaryxlsx created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIFilesSummaryxlsx
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIFilesSummaryxlsx value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'ArchiveCFIFilesSummaryxlsx' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @ArchiveCFIFilesSummaryxlsx
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIFilesSummaryxlsx value set'
print ''



-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIQuarterSummaryxlsx
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIQuarterSummaryxlsx'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'ArchiveCFIQuarterSummaryxlsx' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIQuarterSummaryxlsx'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'ArchiveCFIQuarterSummaryxlsx' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIQuarterSummaryxlsx dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIQuarterSummaryxlsx'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'ArchiveCFIQuarterSummaryxlsx'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @ArchiveCFIQuarterSummaryxlsx
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIQuarterSummaryxlsx created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIQuarterSummaryxlsx
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIQuarterSummaryxlsx value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'ArchiveCFIQuarterSummaryxlsx' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @ArchiveCFIQuarterSummaryxlsx
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ArchiveCFIQuarterSummaryxlsx value set'
print ''

-- Environment Variable CFIGLFiles\CFIGLFiles_Env\RS_ReportingCon
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\RS_ReportingCon'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'RS_ReportingCon' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\RS_ReportingCon'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'RS_ReportingCon' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\RS_ReportingCon dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\RS_ReportingCon'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'RS_ReportingCon'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @RS_ReportingCon
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\RS_ReportingCon created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\RS_ReportingCon
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\RS_ReportingCon value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'RS_ReportingCon' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @RS_ReportingCon
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\RS_ReportingCon value set'
print ''


-- Environment Variable CFIGLFiles\CFIGLFiles_Env\SMTPRelayCon
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\SMTPRelayCon'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'SMTPRelayCon' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\SMTPRelayCon'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'SMTPRelayCon' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\SMTPRelayCon dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\SMTPRelayCon'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'SMTPRelayCon'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @SMTPRelayCon
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\SMTPRelayCon created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\SMTPRelayCon
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\SMTPRelayCon value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'SMTPRelayCon' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @SMTPRelayCon
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\SMTPRelayCon value set'
print ''


-- Environment Variable CFIGLFiles\CFIGLFiles_Env\ValuationDate
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\ValuationDate'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'ValuationDate' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\ValuationDate'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'ValuationDate' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ValuationDate dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\ValuationDate'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'ValuationDate'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @ValuationDate
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ValuationDate created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ValuationDate
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\ValuationDate value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'ValuationDate' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @ValuationDate
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\ValuationDate value set'
print ''

-- Environment Variable CFIGLFiles\CFIGLFiles_Env\CFI_DistributionList
print 'Environment Variable CFIGLFiles\CFIGLFiles_Env\CFI_DistributionList'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
	       Join SSISDB.[catalog].environments e 
			On e.environment_id = ev.environment_id 
		   Join SSISDB.[catalog].folders f 
			On f.folder_id = e.folder_id 
		   Where e.name = N'CFIGLFiles_Env' 
			And ev.name = N'CFI_DistributionList' 
			And f.name = N'CFIGLFiles') 
 begin
  print ' - Dropping Environment Variable CFIGLFiles\CFIGLFiles_Env\CFI_DistributionList'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'CFI_DistributionList' 
  , @environment_name=N'CFIGLFiles_Env' 
  , @folder_name=N'CFIGLFiles' 
  print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFI_DistributionList dropped'
 end
print ' - Creating Environment Variable CFIGLFiles\CFIGLFiles_Env\CFI_DistributionList'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'CFI_DistributionList'
    , @sensitive = 0
    , @environment_name = N'CFIGLFiles_Env'
    , @folder_name = N'CFIGLFiles'
    , @value = @CFI_DistributionList
    , @data_type = N'String'

print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFI_DistributionList created'

-- Set Environment Variable CFIGLFiles\CFIGLFiles_Env\CFI_DistributionList
print ' - Set Environment Variable CFIGLFiles\CFIGLFiles_Env\CFI_DistributionList value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'CFI_DistributionList' 
    , @environment_name = N'CFIGLFiles_Env' 
    , @folder_name = N'CFIGLFiles' 
    , @value = @CFI_DistributionList
print ' - Environment Variable CFIGLFiles\CFIGLFiles_Env\CFI_DistributionList value set'
print ''












