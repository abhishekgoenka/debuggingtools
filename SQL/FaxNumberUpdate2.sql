USE Works

SELECT * FROM dbo.CSS_JOB_QUEUE
SELECT * FROM dbo.CSS_JOB_QUEUE_AUDIT
SELECT * FROM dbo.CSS_JOB_QUEUE_ITEM

UPDATE dbo.CSS_JOB_QUEUE SET DESTINATION = '66885665' WHERE JOB_ID=2

TRUNCATE TABLE dbo.CSS_JOB_QUEUE
TRUNCATE TABLE dbo.CSS_JOB_QUEUE_AUDIT
TRUNCATE TABLE dbo.CSS_JOB_QUEUE_ITEM


SELECT TOP 10 * FROM dbo.Result
ORDER BY NEWID()