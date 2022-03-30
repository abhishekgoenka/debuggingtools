/*
fast css realtime delay monitor kth Dec 2014
Triage script to help identify poorly performing jobs, spoolers, job_types, etc. 
Sept 2015 kth - Modified version specific to FAX jobs since this is particularly useful for tracking faxes/faxing delays
Will always show 'active' jobs at the top of the list, otherwise most recently updated.


*/
declare @daystart datetime
set @daystart = CAST((SELECT CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP)) + '06:00') as datetime)

--Build CTE
;WITH cte1 (JOB_ID, JOB_STATUS_CD, JOB_TYPE_CD, ItemType, Item_Count, Created, Completed, DelaySec, Spooler, Retries, ModDTTM)
AS
(	select jq.JOB_ID
		, MAX(jq.JOB_STATUS_CD) [JOB_STATUS_CD]
		, MAX(jq.job_type_cd) [JOB_TYPE_CD]
		, MAX(ji.ITEM_ID_TYPE)
		, (select count (order_ID) from CSS_JOB_QUEUE_ITEM ji where ji.JOB_ID = jq.JOB_ID) --with (nolock) inner join CSS_JOB_QUEUE jq on ji.JOB_ID = jq.JOB_ID
		, MIN(ji.DATETIME_REC_CREATE)
		, (case 
			when MIN(jq.JOB_STATUS_CD) != 3 then NULL
			else MAX(jq.DATETIME_LAST_MOD)
			end)
		, (Case
			when MIN(jq.JOB_STATUS_CD) != 3 then DateDiff(s, MIN(ji.DATETIME_REC_CREATE), GETDATE())
			else DateDiff(s, MIN(ji.DATETIME_REC_CREATE), MAX(jq.DATETIME_LAST_MOD))
			end) [DelaySec]
		, MAX (jq.SPOOLER_NM)
		, MAX(jq.TOTAL_RETRIES_COUNT)
		, MAX(jq.DATETIME_LAST_MOD)
	from dbo.css_job_queue jq with (nolock)
	inner join dbo.css_job_queue_item ji with (nolock) on jq.JOB_ID = ji.JOB_ID
	left outer join dbo.css_job_queue_audit ja with (nolock) on jq.JOB_ID = ja.JOB_ID
	where ji.DATETIME_REC_CREATE > @daystart
	and jq.PROCESS_AFTER_DT <= GETDATE()
	and jq.JOB_TYPE_CD = 'FAX'  --filter here to improve or destroy query performace
	group by jq.JOB_ID
)
--Query CTE
select JOB_ID
	, convert(char(8),Created,108) as 'Created'
	, convert(char(8),Completed,108) as 'Completed'
	, JOB_TYPE_CD
	, ItemType as 'MAX_ItemType'
	, Item_Count
	, case JOB_STATUS_CD
		when 0 then 'Idle'
		when 1 then 'Active'
		when 2 then 'Failed'
		when 3 then 'Complete'
		when 4 then 'Canceled'
		when 5 then 'On-Hold'
		when 6 then 'Posted'
		when 7 then 'Sent'
	End as 'JobStatus'
	,Retries
	,DelaySec
	,Spooler as 'SPOOLER'
	,convert(char(8),ModDTTM, 108) as 'LastActivity'
from cte1
--where Spooler = 'UHTWPRDBIF01'
order by case when JOB_STATUS_CD = 1 THEN 1 ELSE 2 END, ModDTTM DESC

/* get job_id details
declare @jobid int
set @jobid = 31257695
select * from CSS_JOB_QUEUE where JOB_ID = @jobid
select * from CSS_JOB_QUEUE_Audit where JOB_ID = @jobid order by DATETIME_LAST_MOD DESC
select * from css_job_queue_item where JOB_ID = @jobid
*/