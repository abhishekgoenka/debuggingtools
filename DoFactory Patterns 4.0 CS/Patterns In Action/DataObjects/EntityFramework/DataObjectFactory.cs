using System.Configuration;

namespace DataObjects.EntityFramework
{
    /// <summary>
    /// DataObjectFactory caches the connectionstring so that the context can be created quickly.
    /// </summary>
    public static class DataObjectFactory
    {
        private static readonly string _connectionString;

        /// <summary>
        /// Static constructor. Reads the connectionstring from web.config just once.
        /// </summary>
        static DataObjectFactory()
        {
            string connectionStringName = ConfigurationManager.AppSettings.Get("ConnectionStringName");
            _connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        /// <summary>
        /// Creates the Context using the current connectionstring.
        /// </summary>
        /// <remarks>
        /// Gof pattern: Factory method. 
        /// </remarks>
        /// <returns>Action Entities context.</returns>
        public static ActionEntities CreateContext()
        {
            return new ActionEntities(_connectionString);
        }
    }
}
