/*
Touchworks Fax workload analyzer
What is: Uses CSS audit records to provide hour-by-hour faxing statistics across a time range.
How use: set the start and end variables below and run. Runs slow (1-2 minutes per 24h period on a fast system) so be measured with your time ranges.
2015 Sept kth
*/

declare @dttmstart datetime
		,@dttmend datetime

declare @pages table (DTTM date, Hour int, Pages int)

set @dttmstart = '2015-09-23 03:00'
set @dttmend = '2015-09-23 23:59:59'

--Parse the job page count from each job's audit history
;WITH cte1 (DTTM, PAGES) 
AS
(
    select MAX(DATETIME_REC_CREATE) as DTTM
	       ,CAST(MAX(LTRIM(RTRIM(substring(JOB_PROGRESS_DESC,18,3)))) as int)
	from CSS_JOB_QUEUE_AUDIT jq with (nolock)
	where jq.JOB_TYPE_CD = 'FAX'
	and jq.JOB_PROGRESS_DESC like 'Sending page 1 of %'
	and DATETIME_REC_CREATE > @dttmstart and DATETIME_REC_CREATE < @dttmend
	group by JOB_ID
)
--shove page count aggregates in to the tab var
INSERT INTO @pages 
SELECT CAST(c1.DTTM as date),
       CAST(DATEPART(hour,c1.DTTM)as int) as OnHour,
       SUM(c1.PAGES)
FROM cte1 c1 with (nolock) 
GROUP BY CAST(c1.DTTM as date),
       DATEPART(hour,c1.DTTM)
       order by OnHour;
--Build the report from the audit tables + tab var to include page counts
SELECT CAST(jq.DATETIME_REC_CREATE as date) AS ForDate
       ,CAST(DATEPART(hour,jq.DATETIME_REC_CREATE)as int) AS OnHour
       ,MAX(pa.Pages) as Pages_Started
       ,COUNT (DISTINCT jq.JOB_ID) AS JobsCreated
       ,count(CASE when jq.job_status_cd = 3 THEN 1 ELSE NULL END) as JobsCompleted
       ,count(CASE when jq.JOB_PROGRESS_DESC LIKE 'Performing Auto-Retry%' THEN 1 ELSE NULL END) as Retries
       ,COUNT(CASE when jq.job_status_cd = 2 THEN 1 ELSE NULL END) as Failure_count
FROM css_job_queue_audit jq with (nolock) inner join @pages pa on pa.DTTM = CAST(jq.DATETIME_REC_CREATE as date) and pa.Hour = DATEPART(hour,jq.DATETIME_REC_CREATE) 
where jq.DATETIME_REC_CREATE > @dttmstart and DATETIME_REC_CREATE < @dttmend
and JOB_TYPE_CD = 'FAX'
GROUP BY CAST(jq.DATETIME_REC_CREATE as date), 
       DATEPART(hour,jq.DATETIME_REC_CREATE)
       order by OnHour;