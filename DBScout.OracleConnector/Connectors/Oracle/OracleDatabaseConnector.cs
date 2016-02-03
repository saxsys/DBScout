using DBScout.Model;
using System;
using System.Collections.Specialized;
using System.Data.OleDb;

namespace DBScout.Connectors.Oracle
{
    public class OracleDatabaseConnector : IDatabaseConnector
    {
        private string _connectionString;
        private string _dbSchemaName;

        #region IDatabaseConnector

        /// <summary>
        /// Retrieve database model from configured database
        /// </summary>
        /// <returns>Returns database model instance</returns>
        public DBScout.Model.IDbObject GetDatabaseInformation()
        {
            ValidatePreconditions();
            using (var connection = Connect())
            {
                return ReadDataModel(connection);
            }
        }

        private void ValidatePreconditions()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("ConnectionString is invalid!");
            }
        }

        private OleDbConnection Connect()
        {
            var connection = new OleDbConnection(_connectionString);
            connection.Open();
            return connection;
        }

        private Model.IDbObject ReadDataModel(OleDbConnection connection)
        {
            var databaseInfo = new Database();
            return null;
        }

        /// <summary>
        /// Initialization method
        /// </summary>
        /// <param name="parameters">Parameters collection</param>
        public void Init(NameValueCollection parameters)
        {
            _connectionString = (null != parameters) ? parameters.Get("ConnectionString") ?? string.Empty : string.Empty;
            _dbSchemaName = (null != parameters) ? parameters.Get("SchemaName") ?? string.Empty : string.Empty;
        }

        #endregion

    }
}
