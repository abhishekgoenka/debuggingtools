/*
Touchworks FAX backlog query
Shows Idle, Active jobs grouped by Destination. Will help understand if idle queue is inflated by many jobs lining up to the same fax recipient
Job_Active should always be 1 or 0 , anything else is weird, see worker thread business logic (do not process if like destination has active job)
Destinations listed as Idle with 0 Retries are new and untouched - these are technically what to consider as the backlog.
2015 Sept kth
*/

select 
	distinct(jq.Destination)
	,coalesce(SUM(CASE jq.JOB_STATUS_CD when 1 then 1 end), 0) as 'Jobs_Active'
	,count(jq.JOB_ID) as [Job_Count_Total]
	,coalesce(SUM(CASE jq.JOB_STATUS_CD when 0 then 1 end), 0) as 'Jobs_Idle'
	,SUM(jq.TOTAL_RETRIES_COUNT) as 'Retries_Total'
	,MAX(jq.DATETIME_LAST_MOD) as 'Last_Touched'
From css_job_queue jq with (nolock)
where jq.JOB_STATUS_CD in (1,0) 
and jq.JOB_TYPE_CD = 'FAX'
group by DESTINATION
order by Jobs_Active DESC, Last_Touched DESC, Jobs_Idle DESC, Retries_Total DESC