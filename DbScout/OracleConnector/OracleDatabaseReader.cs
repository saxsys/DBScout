using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using DbScout.Contracts;
using Oracle.ManagedDataAccess.Client;

namespace OracleConnector
{
    public class OracleDatabaseReader : IDatabaseReader, IConfigurable
    {
        private string _connectionString;

        public string ConnectionString
        {
            get
            {
                return string.IsNullOrEmpty(_connectionString)
                    ? (_connectionString = ConfigurationManager.AppSettings[AppSettingsKeys.ConnectionString])
                    : _connectionString;
            }
            set { _connectionString = value; }
        }

        private DbConnection _dbConnection;

        public DbConnection DbConnection
        {
            get { return _dbConnection ?? (_dbConnection = new OracleConnection(ConnectionString)); }
            set { _dbConnection = value; }
        }

        public ICollection<IDatabaseObject> ReadDatabaseObject()
        {
            if (null == _cfg)
            {
                throw new Exception("Configuration instance is not assigned!");
            }

            var rootObjectType = _cfg.GetMandatoryConfigValue("RootObject");
            if (string.IsNullOrEmpty(rootObjectType))
            {
                throw new Exception("There is no root object type name specified. Cannot obtain database structure without root object!");
            }

            return new [] { ReadDatabaseObject(rootObjectType) };
        }

        private IDatabaseObject ReadDatabaseObject(string objectTypeName, IDatabaseObject parentObject = null)
        {
            if (null == _cfg)
            {
                throw new Exception("Configuration instance is not assigned!");
            }

            var databaseObject = new DatabaseObject {Type = objectTypeName,ParentObject = parentObject};
            var dataDictionaryTablesString = _cfg.GetMandatoryConfigValue($"{objectTypeName}.DataDictionaryTables");
            if (string.IsNullOrEmpty(dataDictionaryTablesString))
            {
                throw new Exception($"{objectTypeName}.DataDictionaryTables configuration parameter missed!");
            }

            var dataDictionaryTables = dataDictionaryTablesString.Split(",;".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);
            foreach (var dataDictionaryTable in dataDictionaryTables)
            {
                var criteria = _cfg.GetConfigValue($"{objectTypeName}.{dataDictionaryTable}.Criteria");
                databaseObject.Properties.Add(dataDictionaryTable,GetRecord(dataDictionaryTable,criteria));
            }
            return databaseObject;
        }

        private IDictionary<string, string> GetRecord(string dataDictionaryTable, string criteria)
        {
            var retVal = new Dictionary<string, string>();
            using (var cmd = new OracleCommand())
            {
                cmd.Connection = DbConnection as OracleConnection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.IsNullOrEmpty(criteria)
                    ? $"select * from {dataDictionaryTable}"
                    : $"select * from {dataDictionaryTable} where {criteria}";

                var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    return retVal;
                }

                for (var fieldIndex = 0; fieldIndex < reader.FieldCount; fieldIndex++)
                {
                    retVal.Add(reader.GetName(fieldIndex),reader.GetString(fieldIndex));
                }
            }

            return retVal;
        }

        private IConfiguration _cfg;

        public void Configure(IConfiguration cfg)
        {
            _cfg = cfg;
        }
    }
}
