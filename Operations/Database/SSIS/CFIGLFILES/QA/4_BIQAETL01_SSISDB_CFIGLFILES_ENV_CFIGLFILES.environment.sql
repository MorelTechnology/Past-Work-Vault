 -- ENV_CFIGLFILES ENVIRONMENT VARIABLE VALUES --
USE SSISDB
-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\ArchirveDirectory
Declare @ArchirveDirectory_0 sql_variant = N'\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\HISTORY'

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_CaseReserveFile
Declare @CFI_CaseReserveFile_1 sql_variant = N'\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFICaseReserves.txt'

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_DistributionList
Declare @CFI_DistributionList_2 sql_variant = N'james_gabel@trg.com'

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_FileSummary
Declare @CFI_FileSummary_3 sql_variant = N'\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIFileSummary.txt'

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_Paids
Declare @CFI_Paids_4 sql_variant = N'\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIPaids.txt'

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_QuarterSummaryFile
Declare @CFI_QuarterSummaryFile_5 sql_variant = N'\\MANSAN02\BATCHQA$\CFI\CFIGLFILES\CFIQuarterSummary.txt'

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\HomeDirectory
Declare @HomeDirectory_6 sql_variant = N'\\MANSAN02\BATCHQA$\CFI\CFIGLFILES'

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\RS_ReportingCon
Declare @RS_ReportingCon_7 sql_variant = N'Data Source=DWQA;Initial Catalog=RS_Reporting;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;'

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\SMTPRelayCon
Declare @SMTPRelayCon_8 sql_variant = N'manrelay01trg.com'

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\ValuationDate
Declare @ValuationDate_9 sql_variant = N'9999-12-31'

 ---------------------------------

/*
Script Name: H:\My SQL Code\Deployment Scripts\BIDEVETL01\CFIGLFILES\\4_BIDEVETL01_SSISDB_CFIGLFILES_ENV_CFIGLFILES.environment.sql
Generated From Catalog Instance: BIDEVETL01
Catalog Name: SSISDB
Folder Name: CFIGLFILES
Environment Name: ENV_CFIGLFILES
Generated By: TRG\jgabe
Generated Date: 2/9/2018 9:32:24 AM
Generated From: CatalogBase v2.0.2.0 executing on: JGABEL-LAP1
*/

print 'Script Name: H:\My SQL Code\Deployment Scripts\BIQAETL01\CFIGLFILES\\4_BIDEVETL01_SSISDB_CFIGLFILES_ENV_CFIGLFILES.environment.sql
Generated From Catalog Instance: BIDEVETL01
Catalog Name: SSISDB
Folder Name: CFIGLFILES
Environment Name: ENV_CFIGLFILES
Generated By: TRG\jgabe
Generated Date: 2/9/2018 9:32:24 AM
Generated From: CatalogBase v2.0.2.0 executing on: JGABEL-LAP1'
print ''
print '------------------------------------------------------------'
print 'Deployed to Instance: ' + @@servername
print 'Deploy Date: ' + Convert(varchar,GetDate(), 101) + ' ' + Convert(varchar,GetDate(), 108)
print 'Deployed By: ' + original_login()
print '------------------------------------------------------------'
print ''
declare @ErrMsg varchar(100) 
print 'Check for folder: CFIGLFILES ' 
If Not Exists(Select name 
              From SSISDB.[catalog].folders 
              Where name = N'CFIGLFILES') 
 begin 
  set @ErrMsg = ' - CFIGLFILES does not exist.' 
  raisError(@ErrMsg, 16, 1) 
  return 
 end 
Else 
 begin 
  print ' - CFIGLFILES folder exists.' 
 end 
print '' 

 -- Environment SSISDB\CFIGLFILES\ENV_CFIGLFILES
print 'Environment SSISDB\CFIGLFILES\ENV_CFIGLFILES'
If Not Exists(Select * 
           From SSISDB.[catalog].environments e 
           Join SSISDB.[catalog].folders f 
             On f.folder_id = e.folder_id 
           Where e.name = N'ENV_CFIGLFILES'
             And f.name = N'CFIGLFILES')

 begin
  print ' - Creating Environment SSISDB\CFIGLFILES\ENV_CFIGLFILES'

  Exec SSISDB.[catalog].create_environment 
      @environment_name=N'ENV_CFIGLFILES'
    , @folder_name=N'CFIGLFILES'
  print ' - Environment SSISDB\CFIGLFILES\ENV_CFIGLFILES created'
 end
else
 print ' - Environment SSISDB\CFIGLFILES\ENV_CFIGLFILES already exists.'
print ''

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\ArchirveDirectory
print 'Environment Variable CFIGLFILES\ENV_CFIGLFILES\ArchirveDirectory'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
       Join SSISDB.[catalog].environments e 
On e.environment_id = ev.environment_id 
   Join SSISDB.[catalog].folders f 
On f.folder_id = e.folder_id 
   Where e.name = N'ENV_CFIGLFILES' 
And ev.name = N'ArchirveDirectory' 
And f.name = N'CFIGLFILES') 
 begin
  print ' - Dropping Environment Variable CFIGLFILES\ENV_CFIGLFILES\ArchirveDirectory'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'ArchirveDirectory' 
  , @environment_name=N'ENV_CFIGLFILES' 
  , @folder_name=N'CFIGLFILES' 
  print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\ArchirveDirectory dropped'
 end
print ' - Creating Environment Variable CFIGLFILES\ENV_CFIGLFILES\ArchirveDirectory'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'ArchirveDirectory'
    , @sensitive = 0
    , @environment_name = N'ENV_CFIGLFILES'
    , @folder_name = N'CFIGLFILES'
    , @value = @ArchirveDirectory_0
    , @data_type = N'String'

print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\ArchirveDirectory created'

-- Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\ArchirveDirectory
print ' - Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\ArchirveDirectory value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'ArchirveDirectory' 
    , @environment_name = N'ENV_CFIGLFILES' 
    , @folder_name = N'CFIGLFILES' 
    , @value = @ArchirveDirectory_0
print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\ArchirveDirectory value set'
print ''

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_CaseReserveFile
print 'Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_CaseReserveFile'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
       Join SSISDB.[catalog].environments e 
On e.environment_id = ev.environment_id 
   Join SSISDB.[catalog].folders f 
On f.folder_id = e.folder_id 
   Where e.name = N'ENV_CFIGLFILES' 
And ev.name = N'CFI_CaseReserveFile' 
And f.name = N'CFIGLFILES') 
 begin
  print ' - Dropping Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_CaseReserveFile'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'CFI_CaseReserveFile' 
  , @environment_name=N'ENV_CFIGLFILES' 
  , @folder_name=N'CFIGLFILES' 
  print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_CaseReserveFile dropped'
 end
print ' - Creating Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_CaseReserveFile'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'CFI_CaseReserveFile'
    , @sensitive = 0
    , @environment_name = N'ENV_CFIGLFILES'
    , @folder_name = N'CFIGLFILES'
    , @value = @CFI_CaseReserveFile_1
    , @data_type = N'String'

print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_CaseReserveFile created'

-- Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_CaseReserveFile
print ' - Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_CaseReserveFile value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'CFI_CaseReserveFile' 
    , @environment_name = N'ENV_CFIGLFILES' 
    , @folder_name = N'CFIGLFILES' 
    , @value = @CFI_CaseReserveFile_1
print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_CaseReserveFile value set'
print ''

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_DistributionList
print 'Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_DistributionList'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
       Join SSISDB.[catalog].environments e 
On e.environment_id = ev.environment_id 
   Join SSISDB.[catalog].folders f 
On f.folder_id = e.folder_id 
   Where e.name = N'ENV_CFIGLFILES' 
And ev.name = N'CFI_DistributionList' 
And f.name = N'CFIGLFILES') 
 begin
  print ' - Dropping Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_DistributionList'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'CFI_DistributionList' 
  , @environment_name=N'ENV_CFIGLFILES' 
  , @folder_name=N'CFIGLFILES' 
  print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_DistributionList dropped'
 end
print ' - Creating Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_DistributionList'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'CFI_DistributionList'
    , @sensitive = 0
    , @environment_name = N'ENV_CFIGLFILES'
    , @folder_name = N'CFIGLFILES'
    , @value = @CFI_DistributionList_2
    , @data_type = N'String'

print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_DistributionList created'

-- Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_DistributionList
print ' - Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_DistributionList value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'CFI_DistributionList' 
    , @environment_name = N'ENV_CFIGLFILES' 
    , @folder_name = N'CFIGLFILES' 
    , @value = @CFI_DistributionList_2
print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_DistributionList value set'
print ''

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_FileSummary
print 'Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_FileSummary'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
       Join SSISDB.[catalog].environments e 
On e.environment_id = ev.environment_id 
   Join SSISDB.[catalog].folders f 
On f.folder_id = e.folder_id 
   Where e.name = N'ENV_CFIGLFILES' 
And ev.name = N'CFI_FileSummary' 
And f.name = N'CFIGLFILES') 
 begin
  print ' - Dropping Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_FileSummary'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'CFI_FileSummary' 
  , @environment_name=N'ENV_CFIGLFILES' 
  , @folder_name=N'CFIGLFILES' 
  print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_FileSummary dropped'
 end
print ' - Creating Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_FileSummary'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'CFI_FileSummary'
    , @sensitive = 0
    , @environment_name = N'ENV_CFIGLFILES'
    , @folder_name = N'CFIGLFILES'
    , @value = @CFI_FileSummary_3
    , @data_type = N'String'

print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_FileSummary created'

-- Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_FileSummary
print ' - Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_FileSummary value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'CFI_FileSummary' 
    , @environment_name = N'ENV_CFIGLFILES' 
    , @folder_name = N'CFIGLFILES' 
    , @value = @CFI_FileSummary_3
print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_FileSummary value set'
print ''

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_Paids
print 'Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_Paids'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
       Join SSISDB.[catalog].environments e 
On e.environment_id = ev.environment_id 
   Join SSISDB.[catalog].folders f 
On f.folder_id = e.folder_id 
   Where e.name = N'ENV_CFIGLFILES' 
And ev.name = N'CFI_Paids' 
And f.name = N'CFIGLFILES') 
 begin
  print ' - Dropping Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_Paids'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'CFI_Paids' 
  , @environment_name=N'ENV_CFIGLFILES' 
  , @folder_name=N'CFIGLFILES' 
  print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_Paids dropped'
 end
print ' - Creating Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_Paids'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'CFI_Paids'
    , @sensitive = 0
    , @environment_name = N'ENV_CFIGLFILES'
    , @folder_name = N'CFIGLFILES'
    , @value = @CFI_Paids_4
    , @data_type = N'String'

print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_Paids created'

-- Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_Paids
print ' - Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_Paids value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'CFI_Paids' 
    , @environment_name = N'ENV_CFIGLFILES' 
    , @folder_name = N'CFIGLFILES' 
    , @value = @CFI_Paids_4
print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_Paids value set'
print ''

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_QuarterSummaryFile
print 'Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_QuarterSummaryFile'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
       Join SSISDB.[catalog].environments e 
On e.environment_id = ev.environment_id 
   Join SSISDB.[catalog].folders f 
On f.folder_id = e.folder_id 
   Where e.name = N'ENV_CFIGLFILES' 
And ev.name = N'CFI_QuarterSummaryFile' 
And f.name = N'CFIGLFILES') 
 begin
  print ' - Dropping Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_QuarterSummaryFile'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'CFI_QuarterSummaryFile' 
  , @environment_name=N'ENV_CFIGLFILES' 
  , @folder_name=N'CFIGLFILES' 
  print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_QuarterSummaryFile dropped'
 end
print ' - Creating Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_QuarterSummaryFile'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'CFI_QuarterSummaryFile'
    , @sensitive = 0
    , @environment_name = N'ENV_CFIGLFILES'
    , @folder_name = N'CFIGLFILES'
    , @value = @CFI_QuarterSummaryFile_5
    , @data_type = N'String'

print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_QuarterSummaryFile created'

-- Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_QuarterSummaryFile
print ' - Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_QuarterSummaryFile value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'CFI_QuarterSummaryFile' 
    , @environment_name = N'ENV_CFIGLFILES' 
    , @folder_name = N'CFIGLFILES' 
    , @value = @CFI_QuarterSummaryFile_5
print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\CFI_QuarterSummaryFile value set'
print ''

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\HomeDirectory
print 'Environment Variable CFIGLFILES\ENV_CFIGLFILES\HomeDirectory'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
       Join SSISDB.[catalog].environments e 
On e.environment_id = ev.environment_id 
   Join SSISDB.[catalog].folders f 
On f.folder_id = e.folder_id 
   Where e.name = N'ENV_CFIGLFILES' 
And ev.name = N'HomeDirectory' 
And f.name = N'CFIGLFILES') 
 begin
  print ' - Dropping Environment Variable CFIGLFILES\ENV_CFIGLFILES\HomeDirectory'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'HomeDirectory' 
  , @environment_name=N'ENV_CFIGLFILES' 
  , @folder_name=N'CFIGLFILES' 
  print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\HomeDirectory dropped'
 end
print ' - Creating Environment Variable CFIGLFILES\ENV_CFIGLFILES\HomeDirectory'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'HomeDirectory'
    , @sensitive = 0
    , @environment_name = N'ENV_CFIGLFILES'
    , @folder_name = N'CFIGLFILES'
    , @value = @HomeDirectory_6
    , @data_type = N'String'

print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\HomeDirectory created'

-- Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\HomeDirectory
print ' - Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\HomeDirectory value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'HomeDirectory' 
    , @environment_name = N'ENV_CFIGLFILES' 
    , @folder_name = N'CFIGLFILES' 
    , @value = @HomeDirectory_6
print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\HomeDirectory value set'
print ''

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\RS_ReportingCon
print 'Environment Variable CFIGLFILES\ENV_CFIGLFILES\RS_ReportingCon'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
       Join SSISDB.[catalog].environments e 
On e.environment_id = ev.environment_id 
   Join SSISDB.[catalog].folders f 
On f.folder_id = e.folder_id 
   Where e.name = N'ENV_CFIGLFILES' 
And ev.name = N'RS_ReportingCon' 
And f.name = N'CFIGLFILES') 
 begin
  print ' - Dropping Environment Variable CFIGLFILES\ENV_CFIGLFILES\RS_ReportingCon'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'RS_ReportingCon' 
  , @environment_name=N'ENV_CFIGLFILES' 
  , @folder_name=N'CFIGLFILES' 
  print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\RS_ReportingCon dropped'
 end
print ' - Creating Environment Variable CFIGLFILES\ENV_CFIGLFILES\RS_ReportingCon'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'RS_ReportingCon'
    , @sensitive = 0
    , @environment_name = N'ENV_CFIGLFILES'
    , @folder_name = N'CFIGLFILES'
    , @value = @RS_ReportingCon_7
    , @data_type = N'String'

print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\RS_ReportingCon created'

-- Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\RS_ReportingCon
print ' - Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\RS_ReportingCon value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'RS_ReportingCon' 
    , @environment_name = N'ENV_CFIGLFILES' 
    , @folder_name = N'CFIGLFILES' 
    , @value = @RS_ReportingCon_7
print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\RS_ReportingCon value set'
print ''

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\SMTPRelayCon
print 'Environment Variable CFIGLFILES\ENV_CFIGLFILES\SMTPRelayCon'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
       Join SSISDB.[catalog].environments e 
On e.environment_id = ev.environment_id 
   Join SSISDB.[catalog].folders f 
On f.folder_id = e.folder_id 
   Where e.name = N'ENV_CFIGLFILES' 
And ev.name = N'SMTPRelayCon' 
And f.name = N'CFIGLFILES') 
 begin
  print ' - Dropping Environment Variable CFIGLFILES\ENV_CFIGLFILES\SMTPRelayCon'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'SMTPRelayCon' 
  , @environment_name=N'ENV_CFIGLFILES' 
  , @folder_name=N'CFIGLFILES' 
  print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\SMTPRelayCon dropped'
 end
print ' - Creating Environment Variable CFIGLFILES\ENV_CFIGLFILES\SMTPRelayCon'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'SMTPRelayCon'
    , @sensitive = 0
    , @environment_name = N'ENV_CFIGLFILES'
    , @folder_name = N'CFIGLFILES'
    , @value = @SMTPRelayCon_8
    , @data_type = N'String'

print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\SMTPRelayCon created'

-- Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\SMTPRelayCon
print ' - Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\SMTPRelayCon value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'SMTPRelayCon' 
    , @environment_name = N'ENV_CFIGLFILES' 
    , @folder_name = N'CFIGLFILES' 
    , @value = @SMTPRelayCon_8
print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\SMTPRelayCon value set'
print ''

-- Environment Variable CFIGLFILES\ENV_CFIGLFILES\ValuationDate
print 'Environment Variable CFIGLFILES\ENV_CFIGLFILES\ValuationDate'
If Exists(Select * 
          From SSISDB.[catalog].environment_variables ev 
       Join SSISDB.[catalog].environments e 
On e.environment_id = ev.environment_id 
   Join SSISDB.[catalog].folders f 
On f.folder_id = e.folder_id 
   Where e.name = N'ENV_CFIGLFILES' 
And ev.name = N'ValuationDate' 
And f.name = N'CFIGLFILES') 
 begin
  print ' - Dropping Environment Variable CFIGLFILES\ENV_CFIGLFILES\ValuationDate'
  Exec SSISDB.[catalog].delete_environment_variable 
    @variable_name=N'ValuationDate' 
  , @environment_name=N'ENV_CFIGLFILES' 
  , @folder_name=N'CFIGLFILES' 
  print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\ValuationDate dropped'
 end
print ' - Creating Environment Variable CFIGLFILES\ENV_CFIGLFILES\ValuationDate'
Exec SSISDB.[catalog].create_environment_variable 
      @variable_name = N'ValuationDate'
    , @sensitive = 0
    , @environment_name = N'ENV_CFIGLFILES'
    , @folder_name = N'CFIGLFILES'
    , @value = @ValuationDate_9
    , @data_type = N'String'

print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\ValuationDate created'

-- Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\ValuationDate
print ' - Set Environment Variable CFIGLFILES\ENV_CFIGLFILES\ValuationDate value'
Exec SSISDB.[catalog].set_environment_variable_value 
      @variable_name = N'ValuationDate' 
    , @environment_name = N'ENV_CFIGLFILES' 
    , @folder_name = N'CFIGLFILES' 
    , @value = @ValuationDate_9
print ' - Environment Variable CFIGLFILES\ENV_CFIGLFILES\ValuationDate value set'
print ''