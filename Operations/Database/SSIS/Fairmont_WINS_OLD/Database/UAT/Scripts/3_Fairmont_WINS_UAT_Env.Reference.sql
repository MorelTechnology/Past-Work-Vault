 Use [SSISDB]
 Go

/*
Script Name: C:\ProdTeamWork\Operations\SSIS\Fairmont_WINS\Database\Fairmont_WINS
Generated From Catalog Instance: BIUATETL01
Catalog Name: SSISDB
Folder Name: Fairmont_WINS
Project Name: Fairmont_WINS
Reference Name: Fairmont_WINS/Fairmont_WINS_Env
Environment Name: Fairmont_WINS_Env
Generated By: TRG\rkasu
Generated Date: 11/15/2017 11:49:21 AM
Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP
*/


 -- SSISDB\Fairmont_WINS\Fairmont_WINS\[.|Fairmont_WINS/Fairmont_WINS_Env]


--print 'Script Name: C:\ProdTeamWork\Operations\SSIS\Fairmont_WINS\Database\Fairmont_WINS
--Generated From Catalog Instance: BIUATETL01
--Catalog Name: SSISDB
--Folder Name: Fairmont_WINS
--Project Name: Fairmont_WINS
--Reference Name: Fairmont_WINS/Fairmont_WINS_Env
--Environment Name: Fairmont_WINS_Env
--Generated By: TRG\rkasu
--Generated Date: 12/29/2017 11:49:21 AM
--Generated From: CatalogBase v1.1.1.0 executing on: RKASU-LAP'


print ''
print '------------------------------------------------------------'
print 'Deployed to Instance: ' + @@servername
print 'Deploy Date: ' + Convert(varchar,GetDate(), 101) + ' ' + Convert(varchar,GetDate(), 108)
print 'Deployed By: ' + original_login()
print '------------------------------------------------------------'
print ''
print 'Reference SSISDB\Fairmont_WINS\Fairmont_WINS\[.|Fairmont_WINS/Fairmont_WINS_Env]'


If Not Exists(Select * 
              From SSISDB.[catalog].environment_references er 
              Join SSISDB.[catalog].projects cp 
                On cp.project_id = er.project_id 
              Join SSISDB.[catalog].folders cf 
                On cf.folder_id = cp.folder_id 
              Where cf.name = N'Fairmont_WINS'
                And cp.name = N'Fairmont_WINS'
                And er.environment_name = N'Fairmont_WINS_Env'
                And er.environment_folder_name Is NULL)
 begin
  print ' - Creating Reference SSISDB\Fairmont_WINS\Fairmont_WINS\[.|Fairmont_WINS/Fairmont_WINS_Env]'
  Declare @reference_id bigint 
  Exec [SSISDB].[catalog].[create_environment_reference] 
       @environment_name = N'Fairmont_WINS_Env' 
     , @reference_id = @reference_id OUTPUT 
     , @project_name = N'Fairmont_WINS' 
     , @folder_name = N'Fairmont_WINS' 
     , @environment_folder_name = NULL
     , @reference_type = R 
  print ' - Reference SSISDB\Fairmont_WINS\Fairmont_WINS\[.|Fairmont_WINS/Fairmont_WINS_Env] created'
 end
else print ' - Reference SSISDB\Fairmont_WINS\Fairmont_WINS\[.|Fairmont_WINS/Fairmont_WINS_Env] already exists.'
go


print ''