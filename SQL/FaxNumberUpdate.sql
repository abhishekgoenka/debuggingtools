USE Works;
GO
IF OBJECT_ID ('dbo.CSS_JOB_QUEUE_TR','TR') IS NOT NULL
    DROP TRIGGER dbo.CSS_JOB_QUEUE_TR;
GO
CREATE TRIGGER CSS_JOB_QUEUE_TR
ON dbo.CSS_JOB_QUEUE
AFTER INSERT 
AS

	UPDATE dbo.CSS_JOB_QUEUE SET DESTINATION = '66885665'
	WHERE JOB_STATUS_CD = 'FAX' AND JOB_TYPE_CD = 0
GO