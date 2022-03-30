using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace DataObjects.ADO.NET
{
    //
    // Here is some skeleton code demonstrating how you could manange multiple database connections.
    // Note: this code is not actually used in Patterns in Action.
    //

    /// <summary>
    ///  Connects with Database 1
    /// </summary>
    public static class DbAdapter1
    {
        private static readonly string connectionStringName = ConfigurationManager.AppSettings.Get("ConnectionStringName1");
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

        public static T Read<T>(string sql, Func<IDataReader, T> make, object[] parms = null)
        {
            return DbCore.Read(sql, make, connectionString, parms);
        }

        // etc., etc.
    }

    /// <summary>
    /// Connects with Database 2
    /// </summary>
    public static class DbAdapter2
    {
        private static readonly string connectionStringName = ConfigurationManager.AppSettings.Get("ConnectionStringName2");
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

        public static T Read<T>(string sql, Func<IDataReader, T> make, object[] parms = null)
        {
            return DbCore.Read(sql, make, connectionString, parms);
        }

        // etc., etc.
    }

    public static class DbCore
    {
        private static readonly string dataProvider = ConfigurationManager.AppSettings.Get("DataProvider");
        private static readonly DbProviderFactory factory = DbProviderFactories.GetFactory(dataProvider);

        public static T Read<T>(string sql, Func<IDataReader, T> make, string connectionString, object[] parms = null)
        {
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    command.SetParameters(parms);  // Extension method

                    connection.Open();

                    T t = default(T);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                        t = make(reader);

                    return t;
                }
            }
        }

        // etc., etc.

        /// <summary>
        /// Extention method. Adds query parameters to command object.
        /// </summary>
        /// <param name="command">Command object.</param>
        /// <param name="parms">Array of name-value query parameters.</param>
        private static void SetParameters(this DbCommand command, object[] parms)
        {
            if (parms != null && parms.Length > 0)
            {
                // NOTE: Processes a name/value pair at each iteration
                for (int i = 0; i < parms.Length; i += 2)
                {
                    string name = parms[i].ToString();

                    // No empty strings to the database
                    if (parms[i + 1] is string && (string)parms[i + 1] == "")
                        parms[i + 1] = null;

                    // If null, set to DbNull
                    object value = parms[i + 1] ?? DBNull.Value;

                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = name;
                    dbParameter.Value = value;

                    command.Parameters.Add(dbParameter);
                }
            }
        }
    }
}
