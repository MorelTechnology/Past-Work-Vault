/*
Script name: BuildEnvCompleteFromProject.sql
This script will take project parameters with their design time values in an SSIS-project and create
a script for a configuration environment, matching environment parameters and a reference between project and environment parameters.

Thsi tool is designed for developers to quickly generate a script for deploying SSIS Configuration environments. Alwasy closely review and test the output of
this script. 

  
Parts of this script were created by Henning Frettem, www.thefirstsql.com, 2013-05-28
Script extensively revised to adhere to RSG naming standards and add features by JWR4 2017-09

Naming standards here.
Project are deployed to folders that have the same name
Enviroment names are constructed from ENV_ & the Project name
Environment variables are constructed from EV_ &  project variable names

*/

USE SSISDB
GO


   
DECLARE
    @folder_name sysname      = N'SSIS_FOLDER_NAME_HERE',
	@project_name sysname      = N'PROJECT_NAME_HERE',
    @environment_name sysname = N'ENVIRONMENT_NAME_HERE' ,
    @parameter_name sysname,
    @design_default_value sql_variant,
    @sensitive bit, 
    @description nvarchar(1024), 
    @data_type sysname,
	@project_id BIGINT 
  

  SET @project_id = (SELECT [project_id] FROM [internal].[projects] WHERE name= @project_name)
  
--> Checking that the folder exists
IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[folders] WHERE name = @folder_name)
    BEGIN
        RAISERROR('Deploy your project to the server and provide the correct folder name before executing this script.', 16, 1)
        RETURN
    END
  
PRINT 'DECLARE 
    @folder_id bigint,
    @value sql_variant,
    @folder_name nvarchar(200)      = ''' + @folder_name + ''',
    @environment_name nvarchar(200) = ''' + @environment_name + ''''
  
PRINT ''
  
PRINT '--> Get folder_id for ''' + @folder_name + ''' 
    SET @folder_id = (SELECT folder_id FROM [SSISDB].[catalog].[folders] WHERE name = @folder_name)'
   
PRINT ''
  
PRINT '--> Create ' + @environment_name + ' environment in ''' + @folder_name + '''-folder if it does not exist
    IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environments] WHERE folder_id = @folder_id AND name = @environment_name)
        BEGIN
            EXEC [SSISDB].[catalog].[create_environment] @environment_name = @environment_name, @folder_name = @folder_name
            PRINT ''CREATED: Environment ' + @environment_name + ' in folder ' + @folder_name + '''
        END
    ELSE
        PRINT ''EXISTS: Environment ' + @environment_name + ' exists'''
  
PRINT ''


PRINT 'DECLARE @message nvarchar(255)'
PRINT 'DECLARE @reference_id BIGINT'

PRINT ''
 
PRINT '---) Add environment reference to project. 


	IF NOT EXISTS( SELECT 1
        FROM
            [SSISDB].[internal].[folders] f
        JOIN
            [SSISDB].[internal].[environments] e
            ON e.folder_id = f.folder_id
        JOIN
            [SSISDB].[internal].[projects] p
            ON p.folder_id = f.folder_id
        JOIN
            [SSISDB].[internal].[environment_references] r
            ON r.environment_name = e.environment_name
            AND p.project_id = r.project_id
        WHERE
            f.name = ' + QUOTENAME( @folder_name,'''') +'
            AND e.environment_name = ' +QUOTENAME(@environment_name,'''') + '
            AND p.name = '  + QUOTENAME(@project_name,'''') +'
        )
    BEGIN
    PRINT ''An environment reference for project '  + @project_name +' is being created.''

        EXEC [SSISDB].[catalog].[create_environment_reference]
            @environment_name=' + QUOTENAME(@environment_name,'''') +
			', @reference_id=@reference_id OUTPUT,
            @project_name= ' +QUOTENAME(@project_name,'''') + 
			', @folder_name=' +QUOTENAME(@folder_name,'''') +
			', @reference_type=''R''
    END'
   
--> Create cursor for project parameters
DECLARE cur CURSOR FOR
    SELECT a.parameter_name, a.design_default_value, a.sensitive, a.description, a.data_type
    FROM [SSISDB].[catalog].[object_parameters] a
        INNER JOIN [SSISDB].[catalog].[projects] b
            ON a.project_id = b.project_id
        INNER JOIN [SSISDB].[catalog].[folders] c
            ON b.folder_id = c.folder_id
    WHERE c.name = @folder_name
        AND SUBSTRING(a.parameter_name, 1, 3) <> 'CM.'
    ORDER BY a.parameter_name
   
OPEN cur
FETCH NEXT FROM cur INTO @parameter_name, @design_default_value, @sensitive, @description, @data_type
   
WHILE (@@FETCH_STATUS = 0)
    BEGIN
        PRINT '--> Creating variable ''EV_' + @parameter_name + ''' if it doesn''t exist'
        PRINT	CASE @data_type
					WHEN 'string'	then 	'SET @value  = N''' + CONVERT(nvarchar(max), @design_default_value) + ''''
					WHEN 'datetime'	then 	'SET @value  = CAST(N''' + CONVERT(nvarchar(max), @design_default_value) + ''' AS datetime)'

 				ELSE
					' SET @value = ' + CONVERT(nvarchar(max), @design_default_value) + ''
				END
		PRINT 'IF NOT EXISTS (SELECT 1 
                            FROM [SSISDB].[catalog].[environments] b 
                                INNER JOIN [SSISDB].[catalog].[environment_variables] c
                                    ON b.environment_id = c.environment_id
                            WHERE b.folder_id =  @folder_id 
                                AND b.name = ' + QUOTENAME(@environment_name,'''') +
                                'AND c.name = ''EV_' + @parameter_name + ''')
                BEGIN
                    EXEC [SSISDB].[catalog].[create_environment_variable] 
                        @variable_name = ''EV_' + @parameter_name + ''', 
                        @sensitive = ' + CONVERT(varchar(2), @sensitive) + ', 
                        @description = ''' + @description + ''', 
                        @environment_name = ' + QUOTENAME(@environment_name,'''') +', 
                        @folder_name = ' + QUOTENAME(@folder_name,'''') +', 
                        @value = @value, 
                        @data_type = ''' + @data_type + '''
                    PRINT ''CREATED: Environment-variable EV_' + @parameter_name + '''
                END
            ELSE
                PRINT ''EXISTS: Environment-variable EV_' + @parameter_name + ''''
 
        PRINT '--> Creating variable reference to  ''EV_' + @parameter_name + ''''
		
		 PRINT '
            BEGIN
                    EXEC [SSISDB].[catalog].[set_object_parameter_value] 
                        @object_type = 20, 
                        @folder_name = ' + QUOTENAME(@folder_name,'''') +', 
                        @project_name = ' + QUOTENAME(@project_name,'''') +', 
                        @parameter_name = ' + QUOTENAME(@parameter_name,'''') +', 
                        @parameter_value = ''EV_' + @parameter_name + ''',  
                        @value_type = ''' + 'R' + '''
               END
          
                PRINT ''CREATED: Environment-variable reference to EV_' + @parameter_name + ''''
 
        PRINT ''
   
    FETCH NEXT FROM cur INTO @parameter_name, @design_default_value, @sensitive, @description, @data_type
    END
   
CLOSE cur
DEALLOCATE cur




