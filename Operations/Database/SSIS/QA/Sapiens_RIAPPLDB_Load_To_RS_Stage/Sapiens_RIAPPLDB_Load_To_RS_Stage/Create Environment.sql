DECLARE 
    @folder_id bigint,
    @value sql_variant,
    @folder_name nvarchar(200)      = 'Sapiens_RIAPPLDB_Load_To_RS_Stage',
    @environment_name nvarchar(200) = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage'
 
--> Get folder_id for 'Sapiens_RIAPPLDB_Load_To_RS_Stage' 
    SET @folder_id = (SELECT folder_id FROM [SSISDB].[catalog].[folders] WHERE name = @folder_name)
 
--> Create ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage environment in 'Sapiens_RIAPPLDB_Load_To_RS_Stage'-folder if it does not exist
    IF NOT EXISTS (SELECT 1 FROM [SSISDB].[catalog].[environments] WHERE folder_id = @folder_id AND name = @environment_name)
        BEGIN
            EXEC [SSISDB].[catalog].[create_environment] @environment_name = @environment_name, @folder_name = @folder_name
            PRINT 'CREATED: Environment ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage in folder Sapiens_RIAPPLDB_Load_To_RS_Stage'
        END
    ELSE
        PRINT 'EXISTS: Environment ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage exists'
 
DECLARE @message nvarchar(255)
DECLARE @reference_id BIGINT
 
---) Add environment reference to project. 


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
            f.name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage'
            AND e.environment_name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage'
            AND p.name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage'
        )
    BEGIN
    PRINT 'An environment reference for project Sapiens_RIAPPLDB_Load_To_RS_Stage is being created.'

        EXEC [SSISDB].[catalog].[create_environment_reference]
            @environment_name='ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage', @reference_id=@reference_id OUTPUT,
            @project_name= 'Sapiens_RIAPPLDB_Load_To_RS_Stage', @folder_name='Sapiens_RIAPPLDB_Load_To_RS_Stage', @reference_type='R'
    END
--> Creating variable 'EV_EmailDistributionList' if it doesn't exist
SET @value  = N'jroch@trg.com'
IF NOT EXISTS (SELECT 1 
                            FROM [SSISDB].[catalog].[environments] b 
                                INNER JOIN [SSISDB].[catalog].[environment_variables] c
                                    ON b.environment_id = c.environment_id
                            WHERE b.folder_id =  @folder_id 
                                AND b.name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage'AND c.name = 'EV_EmailDistributionList')
                BEGIN
                    EXEC [SSISDB].[catalog].[create_environment_variable] 
                        @variable_name = 'EV_EmailDistributionList', 
                        @sensitive = 0, 
                        @description = '', 
                        @environment_name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @value = @value, 
                        @data_type = 'String'
                    PRINT 'CREATED: Environment-variable EV_EmailDistributionList'
                END
            ELSE
                PRINT 'EXISTS: Environment-variable EV_EmailDistributionList'
--> Creating variable reference to  'EV_EmailDistributionList'

            BEGIN
                    EXEC [SSISDB].[catalog].[set_object_parameter_value] 
                        @object_type = 20, 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @project_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @parameter_name = 'EmailDistributionList', 
                        @parameter_value = 'EV_EmailDistributionList',  
                        @value_type = 'R'
               END
          
                PRINT 'CREATED: Environment-variable reference to EV_EmailDistributionList'
 
--> Creating variable 'EV_RIAPPLDBCon_InitialCatalog' if it doesn't exist
SET @value  = N'PROD_RIAPPLDB'
IF NOT EXISTS (SELECT 1 
                            FROM [SSISDB].[catalog].[environments] b 
                                INNER JOIN [SSISDB].[catalog].[environment_variables] c
                                    ON b.environment_id = c.environment_id
                            WHERE b.folder_id =  @folder_id 
                                AND b.name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage'AND c.name = 'EV_RIAPPLDBCon_InitialCatalog')
                BEGIN
                    EXEC [SSISDB].[catalog].[create_environment_variable] 
                        @variable_name = 'EV_RIAPPLDBCon_InitialCatalog', 
                        @sensitive = 0, 
                        @description = '', 
                        @environment_name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @value = @value, 
                        @data_type = 'String'
                    PRINT 'CREATED: Environment-variable EV_RIAPPLDBCon_InitialCatalog'
                END
            ELSE
                PRINT 'EXISTS: Environment-variable EV_RIAPPLDBCon_InitialCatalog'
--> Creating variable reference to  'EV_RIAPPLDBCon_InitialCatalog'

            BEGIN
                    EXEC [SSISDB].[catalog].[set_object_parameter_value] 
                        @object_type = 20, 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @project_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @parameter_name = 'RIAPPLDBCon_InitialCatalog', 
                        @parameter_value = 'EV_RIAPPLDBCon_InitialCatalog',  
                        @value_type = 'R'
               END
          
                PRINT 'CREATED: Environment-variable reference to EV_RIAPPLDBCon_InitialCatalog'
 
--> Creating variable 'EV_RIAPPLDBCon_ServerName' if it doesn't exist
SET @value  = N'BIASRC01'
IF NOT EXISTS (SELECT 1 
                            FROM [SSISDB].[catalog].[environments] b 
                                INNER JOIN [SSISDB].[catalog].[environment_variables] c
                                    ON b.environment_id = c.environment_id
                            WHERE b.folder_id =  @folder_id 
                                AND b.name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage'AND c.name = 'EV_RIAPPLDBCon_ServerName')
                BEGIN
                    EXEC [SSISDB].[catalog].[create_environment_variable] 
                        @variable_name = 'EV_RIAPPLDBCon_ServerName', 
                        @sensitive = 0, 
                        @description = '', 
                        @environment_name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @value = @value, 
                        @data_type = 'String'
                    PRINT 'CREATED: Environment-variable EV_RIAPPLDBCon_ServerName'
                END
            ELSE
                PRINT 'EXISTS: Environment-variable EV_RIAPPLDBCon_ServerName'
--> Creating variable reference to  'EV_RIAPPLDBCon_ServerName'

            BEGIN
                    EXEC [SSISDB].[catalog].[set_object_parameter_value] 
                        @object_type = 20, 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @project_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @parameter_name = 'RIAPPLDBCon_ServerName', 
                        @parameter_value = 'EV_RIAPPLDBCon_ServerName',  
                        @value_type = 'R'
               END
          
                PRINT 'CREATED: Environment-variable reference to EV_RIAPPLDBCon_ServerName'
 
--> Creating variable 'EV_RS_Stage_Full_InitialCatalog' if it doesn't exist
SET @value  = N'RS_Stage_Full'
IF NOT EXISTS (SELECT 1 
                            FROM [SSISDB].[catalog].[environments] b 
                                INNER JOIN [SSISDB].[catalog].[environment_variables] c
                                    ON b.environment_id = c.environment_id
                            WHERE b.folder_id =  @folder_id 
                                AND b.name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage'AND c.name = 'EV_RS_Stage_Full_InitialCatalog')
                BEGIN
                    EXEC [SSISDB].[catalog].[create_environment_variable] 
                        @variable_name = 'EV_RS_Stage_Full_InitialCatalog', 
                        @sensitive = 0, 
                        @description = '', 
                        @environment_name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @value = @value, 
                        @data_type = 'String'
                    PRINT 'CREATED: Environment-variable EV_RS_Stage_Full_InitialCatalog'
                END
            ELSE
                PRINT 'EXISTS: Environment-variable EV_RS_Stage_Full_InitialCatalog'
--> Creating variable reference to  'EV_RS_Stage_Full_InitialCatalog'

            BEGIN
                    EXEC [SSISDB].[catalog].[set_object_parameter_value] 
                        @object_type = 20, 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @project_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @parameter_name = 'RS_Stage_Full_InitialCatalog', 
                        @parameter_value = 'EV_RS_Stage_Full_InitialCatalog',  
                        @value_type = 'R'
               END
          
                PRINT 'CREATED: Environment-variable reference to EV_RS_Stage_Full_InitialCatalog'
 
--> Creating variable 'EV_RS_Stage_Full_ServerName' if it doesn't exist
SET @value  = N'DWDEV'
IF NOT EXISTS (SELECT 1 
                            FROM [SSISDB].[catalog].[environments] b 
                                INNER JOIN [SSISDB].[catalog].[environment_variables] c
                                    ON b.environment_id = c.environment_id
                            WHERE b.folder_id =  @folder_id 
                                AND b.name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage'AND c.name = 'EV_RS_Stage_Full_ServerName')
                BEGIN
                    EXEC [SSISDB].[catalog].[create_environment_variable] 
                        @variable_name = 'EV_RS_Stage_Full_ServerName', 
                        @sensitive = 0, 
                        @description = '', 
                        @environment_name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @value = @value, 
                        @data_type = 'String'
                    PRINT 'CREATED: Environment-variable EV_RS_Stage_Full_ServerName'
                END
            ELSE
                PRINT 'EXISTS: Environment-variable EV_RS_Stage_Full_ServerName'
--> Creating variable reference to  'EV_RS_Stage_Full_ServerName'

            BEGIN
                    EXEC [SSISDB].[catalog].[set_object_parameter_value] 
                        @object_type = 20, 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @project_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @parameter_name = 'RS_Stage_Full_ServerName', 
                        @parameter_value = 'EV_RS_Stage_Full_ServerName',  
                        @value_type = 'R'
               END
          
                PRINT 'CREATED: Environment-variable reference to EV_RS_Stage_Full_ServerName'
 
--> Creating variable 'EV_RS_Stage_FullCon_InitialCatalog' if it doesn't exist
SET @value  = N'RS_Stage_Full'
IF NOT EXISTS (SELECT 1 
                            FROM [SSISDB].[catalog].[environments] b 
                                INNER JOIN [SSISDB].[catalog].[environment_variables] c
                                    ON b.environment_id = c.environment_id
                            WHERE b.folder_id =  @folder_id 
                                AND b.name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage'AND c.name = 'EV_RS_Stage_FullCon_InitialCatalog')
                BEGIN
                    EXEC [SSISDB].[catalog].[create_environment_variable] 
                        @variable_name = 'EV_RS_Stage_FullCon_InitialCatalog', 
                        @sensitive = 0, 
                        @description = '', 
                        @environment_name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @value = @value, 
                        @data_type = 'String'
                    PRINT 'CREATED: Environment-variable EV_RS_Stage_FullCon_InitialCatalog'
                END
            ELSE
                PRINT 'EXISTS: Environment-variable EV_RS_Stage_FullCon_InitialCatalog'
--> Creating variable reference to  'EV_RS_Stage_FullCon_InitialCatalog'

            BEGIN
                    EXEC [SSISDB].[catalog].[set_object_parameter_value] 
                        @object_type = 20, 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @project_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @parameter_name = 'RS_Stage_FullCon_InitialCatalog', 
                        @parameter_value = 'EV_RS_Stage_FullCon_InitialCatalog',  
                        @value_type = 'R'
               END
          
                PRINT 'CREATED: Environment-variable reference to EV_RS_Stage_FullCon_InitialCatalog'
 
--> Creating variable 'EV_RS_Stage_FullCon_ServerName' if it doesn't exist
SET @value  = N'DWDEV'
IF NOT EXISTS (SELECT 1 
                            FROM [SSISDB].[catalog].[environments] b 
                                INNER JOIN [SSISDB].[catalog].[environment_variables] c
                                    ON b.environment_id = c.environment_id
                            WHERE b.folder_id =  @folder_id 
                                AND b.name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage'AND c.name = 'EV_RS_Stage_FullCon_ServerName')
                BEGIN
                    EXEC [SSISDB].[catalog].[create_environment_variable] 
                        @variable_name = 'EV_RS_Stage_FullCon_ServerName', 
                        @sensitive = 0, 
                        @description = '', 
                        @environment_name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @value = @value, 
                        @data_type = 'String'
                    PRINT 'CREATED: Environment-variable EV_RS_Stage_FullCon_ServerName'
                END
            ELSE
                PRINT 'EXISTS: Environment-variable EV_RS_Stage_FullCon_ServerName'
--> Creating variable reference to  'EV_RS_Stage_FullCon_ServerName'

            BEGIN
                    EXEC [SSISDB].[catalog].[set_object_parameter_value] 
                        @object_type = 20, 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @project_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @parameter_name = 'RS_Stage_FullCon_ServerName', 
                        @parameter_value = 'EV_RS_Stage_FullCon_ServerName',  
                        @value_type = 'R'
               END
          
                PRINT 'CREATED: Environment-variable reference to EV_RS_Stage_FullCon_ServerName'
 
--> Creating variable 'EV_RS_StageCon_InitialCatalog' if it doesn't exist
SET @value  = N'RS_Stage'
IF NOT EXISTS (SELECT 1 
                            FROM [SSISDB].[catalog].[environments] b 
                                INNER JOIN [SSISDB].[catalog].[environment_variables] c
                                    ON b.environment_id = c.environment_id
                            WHERE b.folder_id =  @folder_id 
                                AND b.name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage'AND c.name = 'EV_RS_StageCon_InitialCatalog')
                BEGIN
                    EXEC [SSISDB].[catalog].[create_environment_variable] 
                        @variable_name = 'EV_RS_StageCon_InitialCatalog', 
                        @sensitive = 0, 
                        @description = '', 
                        @environment_name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @value = @value, 
                        @data_type = 'String'
                    PRINT 'CREATED: Environment-variable EV_RS_StageCon_InitialCatalog'
                END
            ELSE
                PRINT 'EXISTS: Environment-variable EV_RS_StageCon_InitialCatalog'
--> Creating variable reference to  'EV_RS_StageCon_InitialCatalog'

            BEGIN
                    EXEC [SSISDB].[catalog].[set_object_parameter_value] 
                        @object_type = 20, 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @project_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @parameter_name = 'RS_StageCon_InitialCatalog', 
                        @parameter_value = 'EV_RS_StageCon_InitialCatalog',  
                        @value_type = 'R'
               END
          
                PRINT 'CREATED: Environment-variable reference to EV_RS_StageCon_InitialCatalog'
 
--> Creating variable 'EV_RS_StageCon_ServerName' if it doesn't exist
SET @value  = N'DWDEV'
IF NOT EXISTS (SELECT 1 
                            FROM [SSISDB].[catalog].[environments] b 
                                INNER JOIN [SSISDB].[catalog].[environment_variables] c
                                    ON b.environment_id = c.environment_id
                            WHERE b.folder_id =  @folder_id 
                                AND b.name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage'AND c.name = 'EV_RS_StageCon_ServerName')
                BEGIN
                    EXEC [SSISDB].[catalog].[create_environment_variable] 
                        @variable_name = 'EV_RS_StageCon_ServerName', 
                        @sensitive = 0, 
                        @description = '', 
                        @environment_name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @value = @value, 
                        @data_type = 'String'
                    PRINT 'CREATED: Environment-variable EV_RS_StageCon_ServerName'
                END
            ELSE
                PRINT 'EXISTS: Environment-variable EV_RS_StageCon_ServerName'
--> Creating variable reference to  'EV_RS_StageCon_ServerName'

            BEGIN
                    EXEC [SSISDB].[catalog].[set_object_parameter_value] 
                        @object_type = 20, 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @project_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @parameter_name = 'RS_StageCon_ServerName', 
                        @parameter_value = 'EV_RS_StageCon_ServerName',  
                        @value_type = 'R'
               END
          
                PRINT 'CREATED: Environment-variable reference to EV_RS_StageCon_ServerName'
 
--> Creating variable 'EV_SMTPCon_SmtpServer' if it doesn't exist
SET @value  = N'manrelay01.trg.com'
IF NOT EXISTS (SELECT 1 
                            FROM [SSISDB].[catalog].[environments] b 
                                INNER JOIN [SSISDB].[catalog].[environment_variables] c
                                    ON b.environment_id = c.environment_id
                            WHERE b.folder_id =  @folder_id 
                                AND b.name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage'AND c.name = 'EV_SMTPCon_SmtpServer')
                BEGIN
                    EXEC [SSISDB].[catalog].[create_environment_variable] 
                        @variable_name = 'EV_SMTPCon_SmtpServer', 
                        @sensitive = 0, 
                        @description = '', 
                        @environment_name = 'ENV_Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @value = @value, 
                        @data_type = 'String'
                    PRINT 'CREATED: Environment-variable EV_SMTPCon_SmtpServer'
                END
            ELSE
                PRINT 'EXISTS: Environment-variable EV_SMTPCon_SmtpServer'
--> Creating variable reference to  'EV_SMTPCon_SmtpServer'

            BEGIN
                    EXEC [SSISDB].[catalog].[set_object_parameter_value] 
                        @object_type = 20, 
                        @folder_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @project_name = 'Sapiens_RIAPPLDB_Load_To_RS_Stage', 
                        @parameter_name = 'SMTPCon_SmtpServer', 
                        @parameter_value = 'EV_SMTPCon_SmtpServer',  
                        @value_type = 'R'
               END
          
                PRINT 'CREATED: Environment-variable reference to EV_SMTPCon_SmtpServer'
 
