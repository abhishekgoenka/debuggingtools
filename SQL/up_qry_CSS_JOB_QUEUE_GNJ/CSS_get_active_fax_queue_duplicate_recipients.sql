--Shows Idle, Active jobs grouped by Destination. Will help understand if idle queue is inflated by many jobs lining up to the same fax recipient

select 
	distinct(jq.DESTINATION)
	,count(jq.JOB_ID) as [Job_Count]
	,coalesce(SUM(CASE jq.JOB_STATUS_CD when 0 then 1 end), 0) as 'Idle'
	,coalesce(SUM(CASE jq.JOB_STATUS_CD when 1 then 1 end), 0) as 'Active'
	,SUM(jq.TOTAL_RETRIES_COUNT) as 'Total_Retries'
From css_job_queue jq 
where jq.JOB_STATUS_CD in (1,0) 
and jq.JOB_TYPE_CD = 'FAX'
group by DESTINATION
order by Idle DESC, Total_Retries DESC
