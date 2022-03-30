How to use these procedures

The procedures can be deployed on the master database with the included rakefile (or just manually open them in SQL Server Management Studio and run them on master)

Once they are deployed you can call the procedure sp_select from any database. The procedure sp_select accepts the following parameters:

    @table_name: This is the fully qualified table name to display the contents from. (for example msdb.dbo.MSdbms)
    @spid: this optional parameter can be used to specify a spid on which the temp table is created. (useful on busy servers)
    @max_pages: this optional parameter is used to limit the amount of data returned. (default 1000)

To get the rowcount of the table you can run the procedures sp_select_get_rowcount from any database. The procedure sp_select_get_rowcount accepts the following parameters

    @table_name: This is the fully qualified table name to display the contents from. (for example msdb.dbo.MSdbms)
    @spid: this optional parameter can be used to specify a spid on which the temp table is created. (useful on busy servers)
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	
	
Examples
CREATE TABLE #temp (id int, name varchar(200))
INSERT INTO #temp VALUES (1, 'Filip')
INSERT INTO #temp VALUES (2, 'Sam')
	
exec sp_select 'tempdb..#temp'
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	
	
When you want to see the rowcount you run
exec sp_select_get_rowcount 'tempdb..#temp'
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Reference
https://github.com/FilipDeVos/sp_select
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------