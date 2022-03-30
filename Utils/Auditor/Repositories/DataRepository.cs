#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Auditor.Models;

#endregion

namespace Dapper
{
    public class DataRepository
    {
        private readonly String SQL_CONNECTION_STRING = GetConnectionString();

        public List<Activity> GetActivities()
        {
            using (SQLiteConnection connection = new SQLiteConnection(SQL_CONNECTION_STRING))
            {
                //Execute 
                return connection.Query<Activity>("SELECT * FROM ACTIVITIES", commandType: CommandType.Text).ToList();
            }
        }

        public List<Activity> GetActivities(String filter)
        {
            using (SQLiteConnection connection = new SQLiteConnection(SQL_CONNECTION_STRING))
            {
                String sql = "SELECT * FROM ACTIVITIES where " + filter;

                //Execute 
                return connection.Query<Activity>(sql, commandType: CommandType.Text).ToList();
            }
        }

        public void SaveActivity(Activity activity)
        {
            using (SQLiteConnection connection = new SQLiteConnection(SQL_CONNECTION_STRING))
            {
                connection.Open();
                const string sql = "Update Activities set TimeSpend=@timespend, IsUploaded=0, ObjectName=@objectname, ToolName=@toolname, ProjectID=@projectid, OldActionType=@oldactiontype, Version=@version, ExeName=@exename, ArtifactType=@artifacttype, AppID=@appid, ObjID=@objid, ActionType=@actiontype where ID = @id";
                IDbTransaction transaction = connection.BeginTransaction();

                try
                {
                    //Execute 
                    connection.Query<Activity>(sql, new { timespend = activity.TimeSpend, id = activity.ID, objectname = activity.ObjectName, toolname = activity.ToolName, projectid = activity.ProjectID, oldactiontype = activity.OldActionType, version = activity.Version, exename = activity.ExeName, artifacttype = activity.ArtifactType, appid = activity.AppID, objid = activity.ObjID, actiontype = activity.ActionType }, transaction,
                        commandType: CommandType.Text);
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception("Error while updating activities", exception);
                }
                
            }
        }

        private static String GetConnectionString()
        {
            String path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //path = @"Data Source=C:\temp\SAPIENCE DATA\SCDBOld.DB;New=False;Version=3;Password=sap$1inCon;";

            path = String.Format(@"Data Source={0}\Sapience\SCDB.DB;New=False;Version=3;Password=sap$1inCon;", path);
            return path;
        }
    }
}