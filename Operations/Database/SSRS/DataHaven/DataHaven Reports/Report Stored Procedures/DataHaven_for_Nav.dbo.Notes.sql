USE [DataHaven_for_Nav]
GO

/****** Object:  StoredProcedure [dbo].[rpt_Notes]    Script Date: 02/03/2016 07:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.rpt_Notes','P') IS NOT NULL
    DROP PROCEDURE dbo.rpt_Notes;
go

CREATE PROCEDURE [dbo].[rpt_Notes] 

@PPINumber VARCHAR (25)

AS

-- ================================================
 /*STORED PROCEDURE                                                                                  
  *  ==============                                                                                  
  *                                                                                                    
  *    ---Datahaven Notes Report---                                                                *
  *                                                                                                 
  *                                                                                               
  *  DESCRIPTION                                                                                       
  *  ===========                                                                                       
  *                                                                                                    
  * ---This stored procedure is used to produce the Notes Report. It compiles information from Datahaven and Navision.                              
  *                                                                                                    
  *                                                                                                    
  *                                                                                                    
  *  USAGE                                                                                             
  *  =====                                                                                             
  *                                                                                                    
  *  This is used to produce an SSRS report that provides Datahaven notes information                     
  *                                                                                                    
  *                                                                                                     
  *  PARAMETERS                                                                                        
  *  ==========                                                                                        
  *  Start Date
  *  End  Date 
  *  Note Type                                                                                          
  *                                        
  *                                                                                                    
  *  VARIABLES                                                                                         
  *  =========                                                                                         
  *     NA                                                                                               
  *                                                                 
  *                                                                                                    
  *                                                                                                    
  *  RETURN VALUE                                                                                      
  *  ============                                                                                      
  *                                                                                                    
  *     0                  No Errors                                                                   
  *    -1                  Description of cause of non-zero return value                               
  *                                                                                                    
  *                                                                                                    
  *  PROGRAMMING NOTES                                                                                 
  *  ================= 
                                                                                  
                                                                                                                                                                                                      *
  *  CHANGE HISTORY                                                                                    
  *  ==============                                                                                    
  *                                                                                                    
  *     Date        Version          Author	        Description                                    
  *     ====        =======          ======             ===========                                    
  *                                                                                                    
  *     8/23/2013   1.0			 Steve Carignan      Origianl Version                               
	   02/03/2016				 Pburke			 Modified query to remove unused and unnecessary Temp tables                                                                                              

*/
BEGIN
SET NOCOUNT ON;

    SELECT a.source_number as [PPI Number],
	   n.note_data as NoteData,
	   CAST(n.note_date as DATE) as NoteDate,
	   n.note_type as NoteType
    FROM	Ud_dt_dynamics_doc a
	   JOIN manage_object m on m.obj_id = a.object_fk
	   JOIN edc_Notes n ON n.document_fk = m.obj_id_fk
				     AND n.note_type = 'U'
    WHERE a.source_number = @PPINumber
    ORDER BY CAST(n.note_date as DATE);

END;


GO


