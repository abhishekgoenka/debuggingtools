IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_qry_CSS_JOB_QUEUE_GNJ]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[up_qry_CSS_JOB_QUEUE_GNJ] 
GO

CREATE PROCEDURE [dbo].[up_qry_CSS_JOB_QUEUE_GNJ]
	@cookie_cntr				numeric(16) = 0,	-- no longer used, always 0
	@spooler_nm 				varchar(64),
	@ip_address 				varchar(64),
	@any_site					tinyint = 1,	-- no longer used, always 1
	@avail_modem				tinyint,
	@can_print					tinyint,
	@can_fax					tinyint,
	@can_news					tinyint,
	@can_elig					tinyint,
	@posting_cntr				tinyint = 0,	-- no longer used, always 0
	@wellpoint_client			tinyint = 0,	-- no longer used, always 0
	@lock_row					tinyint = 1,	-- no longer used, always 1
	@can_process_ced			tinyint = 0,
	@can_process_inbound		tinyint,
	@can_create_outbound		tinyint,
	@can_publish_outbound		tinyint,
	@can_atp_sync				tinyint,
	@custom_filter				varchar(4000),
	@can_script_inbound			tinyint,
	@can_script_outbound_create tinyint,
	@can_script_outbound_publish tinyint,
	@can_e_referral				tinyint = 0,
	@can_e_referral_response	tinyint = 0,

	@QueueInitialGrabCount		int = 100,		-- How many rows to initially grab from CSS_JOB_QUEUE.  Too low and we might end up without any job to process,
												--		but too high and we might end up slowing the proc down if the table is large.
	@QueueCacheGrabCount		int = 5			-- How many rows to attempt to act on, after filtering out non-actionable jobs from the initial grab.

AS
/******************************************************************************************************************************

Copyright: Allscripts Healthcare Solutions (R)  2004 

Object Name : up_qry_CSS_JOB_QUEUE_GNJ
Author : ????
Date Created : ????
Module Name :

Purpose : Get the next job for the job spooler, which runs as an OS service (CSS) on the Message Center server(s).  This processes
		  all the print, fax, hub transmission, etc., jobs.  It is called repeatedly, every second per process, until no more jobs found, then pauses for 30 seconds.

Usage : 
		exec up_qry_CSS_JOB_QUEUE_GNJ @Spooler_nm='ACNDEV17', @ip_address='10.106.18.54',@avail_modem=0,@can_print=1,@can_fax=0,@can_news=0,@can_elig=1,
									  @can_process_ced=0,@can_process_inbound=0, @can_create_outbound=0, @can_publish_outbound=0, @can_atp_sync=0,
									  @custom_filter='',
									  @can_script_inbound=0,@can_script_outbound_create=0,@can_script_outbound_publish=0,@can_e_referral=0,@can_e_referral_response=0

		exec up_qry_CSS_JOB_QUEUE_GNJ @Spooler_nm='ACNDEV17', @ip_address='10.106.18.54',@avail_modem=0,@can_print=1,@can_fax=0,@can_news=0,@can_elig=1,
									  @can_process_ced=0,@can_process_inbound=0, @can_create_outbound=0, @can_publish_outbound=0, @can_atp_sync=0,
									  @custom_filter='RETRIES=5',
									  @can_script_inbound=0,@can_script_outbound_create=0,@can_script_outbound_publish=0,@can_e_referral=0,@can_e_referral_response=0

Dependent Objects :  up_ins_CSS_JOB_QUEUE_SINGLE 

						Job Status Codes:	0=Idle
											1=Active
											2=Failed
											3=Complete
											4=Canceled
											5=On-Hold
											6=Posted


Change History : 
ChangedBy	Date		Description
jts			1/5/2005	VI# 29435 Added comment section, added nolock to CSS_JOB_QUEUE select queries.
ymichi		2/21/2007	VI# 31247 Moved temp table creation to the top
wjia		1/26/2012	Added ability to filter out the CED jobs based upon print server configuration. 
jchinnap	3/15/2012	TFS#423276 - Don't pickup the CED jobs if the spooler is not configured to process CED jobs
bbarton		05/06/2014	TFS#801198 - Try to speed up overall execution time and reduce contention on CSS_JOB_QUEUE.
******************************************************************************************************************************/

DECLARE @org_id					int,
		@site_id				int,
		@job_id					numeric(16),
		@job_type_cd			char(12),
		@job_status_cd			smallint,
		@device_type			varchar(32),
		@job_priority			int,
		@contains_crystal_rpt	tinyint,
		@print_server_ipaddress	varchar(255),
		@server_name			varchar(50);

DECLARE @item_count		int,
		@executable_name varchar(64),
		@item_timeout	smallint,
		@serial_same_dest int,
		@IDLE_JOB		tinyint = 0,
		@POSTED_JOB		tinyint = 6,
		@ModemExists	bit = 0;

DECLARE @nsql			nvarchar(4000),
		@Filter			nvarchar(4000),
		@RowCount		int,
		@True			tinyint = 1;

DECLARE @JobID TABLE  (
		RowNumber		integer IDENTITY(1,1),
		JobID			numeric(16) PRIMARY KEY CLUSTERED
		);

DECLARE @MyJobID TABLE  (
		RowNumber		integer IDENTITY(1,1),
		JobID			numeric(16) PRIMARY KEY CLUSTERED
		);


CREATE TABLE #RequestedJobTypes
	(
		JOB_TYPE_CD 	char(12)	PRIMARY KEY CLUSTERED
	)


CREATE TABLE #JOB_QUEUE
	(
		JOB_ID 						numeric(16,0), -- Considered PRIMARY KEY CLUSTERED here, which would help joins, but would impede initial load (unless we grab the whole queue and defer ORDER BY priority, date)
		ORG_ID 						int,
		SITE_ID 					int,
		JOB_STATUS_CD				smallint,
		JOB_TYPE_CD 				char(12),
		DEVICE_TYPE	 				varchar(32) default(''),
		JOB_PRIORITY 				int,
		DATETIME_LAST_MOD			datetime,
		CONTAINS_CRYSTAL_RPT		tinyint default(0),
		ITEM_COUNT					int default(0),
		DESTINATION					varchar(255) default(''),
		PRINT_SERVER_DISPLAY_NAME	varchar(255) default(''),
		PRINT_SERVER_NAME 			varchar(255) default(''),
		PRINTER_NAME varchar(255)	default(''),
		PRINT_SERVER_IPADDRESS		varchar(255) default(''),
		PRINT_SERVER_ID				numeric(16,0),
		RowNumber					int IDENTITY(1,1),
		UNIQUE (JOB_ID)				-- We are forcing the creation of a unique index here because if we were to create it separately after the data is loaded, 
	)								-- the optimizer would ignore it within the same batch or procedure.


SET NOCOUNT ON





-- Lookup our preference to see if we should queue up jobs with same destination
SELECT @serial_same_dest = DEFAULT_TYPE_ENUM
FROM dbo.CSS_SITE_PREF  WITH (NOLOCK)
WHERE ORG_ID = 0 
  and SITE_ID = 0 
  and DEFAULT_TYPE = 'PRINT_QUEUE_CONFIG' 
  and DEFAULT_TYPE_CD = 'SERIAL_SAME_DEST' 
SELECT @serial_same_dest = ISNULL(@serial_same_dest,0)
-- If no configuration found, default to 10 days
IF (@serial_same_dest = 0)
BEGIN
	SELECT @serial_same_dest = 1 
	exec Set_CSS_SITE_PREF 0, 0, 0, 'PRINT_QUEUE_CONFIG', 'SERIAL_SAME_DEST', 'USE_ENUM', @serial_same_dest  -- TODO: is this supposed to be 10, not 1?
END



-- Determine the types of jobs the spooler can handle
IF @can_atp_sync = @True
	INSERT INTO #RequestedJobTypes VALUES ('ATP_SYNC')
IF @can_news = @True
	INSERT INTO #RequestedJobTypes VALUES ('CBRIEF')
IF @can_process_ced = @True
	INSERT INTO #RequestedJobTypes VALUES ('CED')
IF @can_elig = @True
	INSERT INTO #RequestedJobTypes VALUES ('ELIG')
IF @can_e_referral_response = @True
	INSERT INTO #RequestedJobTypes VALUES ('ERef_Resp')
IF @can_e_referral = @True
	INSERT INTO #RequestedJobTypes VALUES ('eREFERRAL')
IF @can_fax = @True
	INSERT INTO #RequestedJobTypes VALUES ('FAX')
IF @can_process_inbound = @True
	INSERT INTO #RequestedJobTypes VALUES ('INB_MSG')
IF @can_script_inbound = @True
	INSERT INTO #RequestedJobTypes VALUES ('INB_SCR')
IF @can_create_outbound = @True
	INSERT INTO #RequestedJobTypes VALUES ('OUT_MSG_CRT')
IF @can_publish_outbound = @True
	INSERT INTO #RequestedJobTypes VALUES ('OUT_MSG_PUB')
IF @can_script_outbound_create = @True
	INSERT INTO #RequestedJobTypes VALUES ('OUT_SCR_CRT')
IF @can_script_outbound_publish = @True
	INSERT INTO #RequestedJobTypes VALUES ('OUT_SCR_PUB')
IF @can_print = @True
	INSERT INTO #RequestedJobTypes VALUES ('PRINT')



-- Obtain a starting set of candidate jobs

-- If a custom filter was NOT provided, use static SQL
if (@custom_filter = '')
begin
	INSERT INTO #JOB_QUEUE 
		  (ORG_ID, SITE_ID, JOB_ID, JOB_STATUS_CD,   JOB_TYPE_CD, JOB_PRIORITY, DATETIME_LAST_MOD, PRINT_SERVER_DISPLAY_NAME, PRINTER_NAME, DESTINATION)
	SELECT TOP (@QueueInitialGrabCount)
		   ORG_ID, SITE_ID, JOB_ID, JOB_STATUS_CD, jq.JOB_TYPE_CD, JOB_PRIORITY, DATETIME_LAST_MOD, SERVER_NAME,			  PRINTER_NAME, DESTINATION
	FROM dbo.CSS_JOB_QUEUE JQ  WITH (NOLOCK) 
	JOIN #RequestedJobTypes jt ON jt.JOB_TYPE_CD = jq.JOB_TYPE_CD
	WHERE (JOB_STATUS_CD = 0 AND PROCESS_AFTER_DT <= GETDATE())
	ORDER BY JOB_PRIORITY ASC, DATETIME_LAST_MOD ASC  -- Get the highest priority job that hasn't been touched for the longest time
	SELECT @RowCount = @@ROWCOUNT;

end
else
begin	-- If a custom filter was provided, use dynamic SQL
	SET @Filter = RTRIM(@Custom_Filter)
	select @nsql = N'
	INSERT INTO #JOB_QUEUE 
		  (ORG_ID, SITE_ID, JOB_ID, JOB_STATUS_CD,    JOB_TYPE_CD, JOB_PRIORITY, DATETIME_LAST_MOD, PRINT_SERVER_DISPLAY_NAME, PRINTER_NAME, DESTINATION)
	SELECT TOP (' + CAST(@QueueInitialGrabCount AS VARCHAR(15)) + N')
		   ORG_ID, SITE_ID, JOB_ID, JOB_STATUS_CD, jq.JOB_TYPE_CD, JOB_PRIORITY, DATETIME_LAST_MOD, SERVER_NAME,			   PRINTER_NAME, DESTINATION
	FROM dbo.CSS_JOB_QUEUE JQ  WITH (NOLOCK) 
	JOIN #RequestedJobTypes jt ON jt.JOB_TYPE_CD = jq.JOB_TYPE_CD
	WHERE (JOB_STATUS_CD = 0 AND PROCESS_AFTER_DT <= GETDATE())
	  AND (' + @Filter + N') 
	ORDER BY JOB_PRIORITY ASC, DATETIME_LAST_MOD ASC 
	SELECT @RowCount = @@ROWCOUNT;'
	--SELECT @nsql 'Dynamic SQL'
	EXEC sp_executesql @nsql, N'@RowCount int output', @RowCount OUTPUT
end



-- Remove jobs that are not actionable by this spooler
IF @RowCount > 0
begin

	-- We have to treat jobs with crystal reports differently for posting centers
	UPDATE #JOB_QUEUE
		SET CONTAINS_CRYSTAL_RPT = 1
	FROM dbo.CSS_JOB_QUEUE_ITEM JI  WITH (NOLOCK) 
	WHERE #JOB_QUEUE.JOB_ID = JI.JOB_ID AND JI.PROCESSING_TYPE = 'CRYSTAL'


	-- Figure out our Print Server info
	UPDATE #JOB_QUEUE SET 
		PRINT_SERVER_NAME = ps.Name, 
		PRINT_SERVER_IPADDRESS = ps.IPAddress, 
		PRINT_SERVER_ID = ps.ID 
	FROM dbo.Printing_Server ps  WITH (NOLOCK) 
	WHERE #JOB_QUEUE.PRINT_SERVER_DISPLAY_NAME = ps.DisplayName


	-- Don't grab any jobs that weren't meant for me that don't need to be posted
	DELETE #JOB_QUEUE
	WHERE CONTAINS_CRYSTAL_RPT = 0 AND PRINT_SERVER_DISPLAY_NAME <> '' /*SPECIFIC DESTINATION*/ AND PRINT_SERVER_IPADDRESS <> '' /*REMOTE PRINTING*/


	-- Are we supposed to serialize jobs with the same destination (same printer or same fax number)
	if (@serial_same_dest > 0)
	begin
		-- Determine our item count
		UPDATE #JOB_QUEUE
		SET ITEM_COUNT = (SELECT COUNT(1)
						  FROM dbo.CSS_JOB_QUEUE_ITEM JI  WITH (NOLOCK) 
						  WHERE #JOB_QUEUE.JOB_ID = JI.JOB_ID)

		-- TABLE SCAN !!!!
		-- Remove any print jobs with multiple items and there is an active job to the same printer
		DELETE tjq 
		FROM  #JOB_QUEUE tjq
		JOIN dbo.CSS_JOB_QUEUE jq  WITH (NOLOCK) ON jq.SERVER_NAME = tjq.PRINT_SERVER_DISPLAY_NAME AND jq.PRINTER_NAME = tjq.PRINTER_NAME
		WHERE tjq.ITEM_COUNT > 0 
		  AND jq.JOB_STATUS_CD = 1 AND jq.JOB_TYPE_CD = 'PRINT'


		-- TABLE SCAN !!!!
		-- Remove any fax jobs when there is an active job to the same phone number
		DELETE tjq
		FROM #JOB_QUEUE tjq 
		JOIN dbo.CSS_JOB_QUEUE jq  WITH (NOLOCK) ON jq.DESTINATION = tjq.DESTINATION 
		WHERE jq.JOB_STATUS_CD = 1 AND JQ.JOB_TYPE_CD = 'FAX'

	end

end


-- Are there any jobs left to choose from?  Grab the top n candidate Job ID's.
INSERT INTO @JobID
		(JobID)
SELECT TOP (@QueueCacheGrabCount)
		 JOB_ID
FROM #JOB_QUEUE
ORDER BY RowNumber -- preserve the original insertion order, so we're relying on that being what we want.
SELECT @RowCount = @@ROWCOUNT;

	
-- We have some candidate jobs to attempt to claim.
IF @RowCount > 0
BEGIN

	-- Mark a row as taken
	UPDATE TOP (1) jq
		SET JOB_STATUS_CD = 1, 
			JOB_PROGRESS_DESC = 'Job Initiation', 
			SPOOLER_NM = @spooler_nm, 
			USER_ID_LAST_MOD = @cookie_cntr, 
			DATETIME_LAST_MOD = getdate()
		OUTPUT inserted.JOB_ID INTO @MyJobID (JobID)
		FROM dbo.CSS_JOB_QUEUE jq WITH (READPAST)  -- This query hint prevents locking issues that can come up if another application is already reading the first qualifying record in the table.
		JOIN @JobID tj ON tj.JobID = jq.JOB_ID
		WHERE JOB_STATUS_CD = @IDLE_JOB 

	SELECT @RowCount = @@ROWCOUNT;
	
	IF @RowCount > 0
		BEGIN
			SELECT TOP 1 @job_id = JobID FROM @MyJobID

			SELECT
				@org_id = ORG_ID,
				@site_id = SITE_ID,
				@job_id = JOB_ID, 
				@job_type_cd = JOB_TYPE_CD,
				@job_status_cd = JOB_STATUS_CD,
				@device_type = DEVICE_TYPE,
				@job_priority = JOB_PRIORITY,
				@contains_crystal_rpt = CONTAINS_CRYSTAL_RPT,
				@print_server_ipaddress = PRINT_SERVER_IPADDRESS,
				@server_name = PRINT_SERVER_NAME,
				@item_count = ITEM_COUNT
			FROM #JOB_QUEUE  WITH (NOLOCK)
			WHERE JOB_ID = @job_id
		END

END /* RowCount */



--IF Job selected, calculate additional values for the result set.
IF @job_id > 0
BEGIN

	SELECT @device_type = CASE 
							WHEN @job_type_cd = 'FAX' AND @job_status_cd = 0 AND @contains_crystal_rpt = 1 AND @print_server_ipaddress <> '' 	
								THEN 'NETWORK'
							WHEN @job_type_cd IN ('FAX', 'SCRIPT', 'ELIG')
								THEN 'MODEM'
							WHEN @job_type_cd = 'PRINT'
								THEN 'PRINTER'
						  END 
	
	-- If all of our pharmacy script switches are network connections, we don't need a modem, otherwise we probably will.
	SELECT @ModemExists = 1 
	FROM dbo.COMM_PROTOCOL			    cp  WITH (NOLOCK)
	JOIN dbo.COMM_PROTOCOL_SCRIPT_SW  cpss  WITH (NOLOCK)  ON cpss.COMM_PROTOCOL_ID = cp.COMM_PROTOCOL_ID
	JOIN dbo.SCRIPT_SWITCH			    ss  WITH (NOLOCK)  ON ss.SCRIPT_SW_ID = cpss.SCRIPT_SW_ID
	WHERE 	ss.ACTV_IND = 'Y'
		AND	ss.SCRIPT_SW_TYPE = 'P'
		AND	cp.PROTOCOL_TYP = 'RAS' 
	IF ISNULL(@ModemExists,0) = 0 AND @JOB_TYPE_CD IN ('SCRIPT', 'ELIG') 
		SELECT @device_type = 'NETWORK'


	-- Only calculate the item count if we didn't already do so above.
	if (@serial_same_dest = 0)
		SELECT @item_count = count(1) 
		FROM dbo.CSS_JOB_QUEUE_ITEM  WITH (NOLOCK) 
		WHERE JOB_ID = @job_id


	SELECT @executable_name = EXECUTABLE_NAME, 
		   @item_timeout = JOB_EXECUTION_TIMEOUT 
	FROM dbo.CSS_JOB_TYPES  WITH (NOLOCK) 
	WHERE JOB_TYPE_CD = @job_type_cd

	IF ISNULL(@item_timeout,0) = 0
		SET @item_timeout = 600

END /* IF Job Selected */


-- Return all the same data to the proc as the up_get does, plus the device type and the item count
-- and the executable name and timeout values
select
	ORG_ID,
	SITE_ID,
	JOB_ID,
	ACTV_IND,                      
	JOB_TYPE_CD ,                  
	JOB_TYPE_PROP,
	JOB_STATUS_CD ,                
	JOB_PROGRESS_DESC,             
	SPOOLER_NM,
	SPOOLER_DEVICE_NM,
	USER_ID_REC_CREATE,
	-- DESTINATION,
	SERVER_NAME,
	-- PRINTER_NAME,
	CLIENT_STD_TZ,                             
	JOB_PRIORITY,                  
	RETRIES,                       
	PAT_ID,                       
	PHARM_ID,                      
	RESUBMITTED,                   
	DEPENDENCY_JOB_ID,             
	ORIG_JOB_ID ,          
	PBM_ID,
	PROCESS_AFTER_DT,
	DATETIME_REC_CREATE,
	USER_ID_LAST_MOD,
	DATETIME_LAST_MOD,
	-- TOTAL_RETRIES_COUNT,
	@device_type AS DEVICE_TYPE,
	@item_count AS ITEM_COUNT,
	@executable_name AS EXECUTABLE_NAME, 
	@item_timeout AS JOB_EXECUTION_TIMEOUT
FROM dbo.CSS_JOB_QUEUE  WITH (NOLOCK) 
WHERE JOB_ID = @job_id 


DROP TABLE #JOB_QUEUE
DROP TABLE #RequestedJobTypes

SET NOCOUNT OFF
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[up_qry_CSS_JOB_QUEUE_GNJ]  TO [idxusers]
GO


