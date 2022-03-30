SELECT * FROM @PrintDocuments
DECLARE @v XML = (SELECT * FROM @PrintDocuments FOR XML AUTO)