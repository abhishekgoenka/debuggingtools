
/* *****************************************************************************
Script Name	: 004.2 Restart stuck PrintFax jobs.sql
Copyright	: Allscripts Healthcare Solutions (R) 2014
Created by	: Byron Barton
Date		: 20May2014
Description	: Find any print/fax jobs in the queue that are either active or on-hold and have not been touched in 15 minutes 
			  and re-start them.


Revision History
--------------------------------------------------------------------------------------------------------------------
Change By			Date		Change
--------------------------------------------------------------------------------------------------------------------
***************************************************************************** */

BEGIN TRANSACTION            

DECLARE @JobID 		BINARY(16),
	@ReturnCode 	INT,
	@SQL		Nvarchar(4000),
	@dbname		varchar(64),    
	@Ndbname	Nvarchar(128),
	@StartTime	Int

SELECT @ReturnCode = 0,
	@dbname = db_name(),
	@Ndbname = N'' + db_name()
SELECT @SQL = N'Job Queue - Restart stuck Print/Fax jobs - ' + @dbname

IF (SELECT COUNT(*) FROM msdb.dbo.syscategories WHERE name = N'[Uncategorized (Local)]') < 1 
	EXECUTE msdb.dbo.sp_add_category @name = N'[Uncategorized (Local)]'

-- Delete the job with the same name (if it exists)
SELECT @JobID = job_id     
  FROM   msdb.dbo.sysjobs    
  WHERE name = @SQL       

IF (@JobID IS NOT NULL)    
    BEGIN  
	-- Check if the job is a multi-server job  
	IF (EXISTS (SELECT  * 
              FROM    msdb.dbo.sysjobservers 
              WHERE   (job_id = @JobID) AND (server_id <> 0))) 
	    BEGIN 
		-- There is, so abort the script 
		RAISERROR (N'Unable to import job ''Job Queue - Restart stuck Print/Fax jobs - + DBname'' since there is already a multi-server job with this name.', 16, 1) 

		GOTO QuitWithRollback  
	    END 
	ELSE 
	    BEGIN
		DECLARE @SQLString Nvarchar(4000),
				@ParDefinition Nvarchar(1000)

		SET @SQLString=''
		SET @ParDefinition=''

		SET @SQLString=N'SELECT	@Start_Time = sysschedules.active_start_time
						 FROM	msdb.dbo.sysjobschedules
						 INNER JOIN msdb.dbo.sysschedules
                         ON sysjobschedules.schedule_ID = sysschedules.schedule_ID
						 WHERE	sysjobschedules.job_id = @Job_ID'

		SET @ParDefinition=N'@Start_Time INT OUTPUT,
							 @Job_ID BINARY(16)'
		EXECUTE sp_executesql
				@SQLString,
				@ParDefinition,
				@Start_Time = @StartTime OUTPUT,
				@Job_ID = @JobID
		-- Delete the [local] job 
		EXECUTE msdb.dbo.sp_delete_job @job_name = @SQL
	    END
	SELECT @JobID = NULL
    END 

IF @StartTime is Null
	Select @StartTime = 0	-- military time in hhmmss format 

   BEGIN 

  -- Add the job
  EXECUTE @ReturnCode = msdb.dbo.sp_add_job @job_id = @JobID OUTPUT , @job_name = @SQL, @owner_login_name = N'sa', @description = N' Find any print/fax jobs in the queue that are either active or on-hold and have not been touched in 15 minutes and re-start them.', @category_name = N'[Uncategorized (Local)]', @enabled = 1, @notify_level_email = 0, @notify_level_page = 0, @notify_level_netsend = 0, @notify_level_eventlog = 2, @delete_level= 0
  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 

  -- Add the job steps
  EXECUTE @ReturnCode = msdb.dbo.sp_add_jobstep @job_id = @JobID, @step_id = 1, @step_name = N'Update', @command = N'

	SET NOCOUNT ON

	DECLARE @15MinutesAgo DATETIME
	SELECT  @15MinutesAgo = DATEADD(Minute, -15, GETDATE())

	UPDATE dbo.CSS_JOB_QUEUE SET 
			JOB_STATUS_CD = 0, 
			JOB_PROGRESS_DESC = ''Auto-Restart Inactive Job'' 
	WHERE JOB_TYPE_CD IN (''PRINT'', ''FAX'') 
	  AND JOB_STATUS_CD IN (1, 5) 
	  AND DATETIME_LAST_MOD < @15MinutesAgo 	

', @database_name = @Ndbname, @server = N'', @database_user_name = N'', @subsystem = N'TSQL', @cmdexec_success_code = 0, @flags = 0, @retry_attempts = 0, @retry_interval = 1, @output_file_name = N'', @on_success_step_id = 0, @on_success_action = 1, @on_fail_step_id = 0, @on_fail_action = 2
  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 
  EXECUTE @ReturnCode = msdb.dbo.sp_update_job @job_id = @JobID, @start_step_id = 1 

  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 

  -- Add the job schedule
  EXECUTE @ReturnCode = msdb.dbo.sp_add_jobschedule 
			@job_id = @JobID, @name = N'Every 15 minutes', 
			@enabled = 1, 
			@freq_type = 4, 
			@freq_interval = 1, 
			@freq_subday_type = 4, 
			@freq_subday_interval = 15, 
			@freq_relative_interval = 0, 
			@freq_recurrence_factor = 0, 
			@active_start_date = 20140305, @active_start_time = @StartTime, 
			@active_end_date = 99991231,   @active_end_time = 235959
  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 

  -- Add the Target Servers
  EXECUTE @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @JobID, @server_name = N'(local)' 
  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 

END
COMMIT TRANSACTION       

GOTO   EndSave              

QuitWithRollback:
  IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION 

EndSave: 

go


