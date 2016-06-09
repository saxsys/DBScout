using System.Collections.Generic;
using System.Data;
using DbScout.Contracts;
using Oracle.ManagedDataAccess.Client;

namespace DbScout.Commands
{
    public class GenerateSqlFilesFromOracleDataDictionaryCommand : ICommand
    {
        private IDictionary<string, string> _cfgDictionary;
        private IDbConnection _dbConnection;

        public void Configure(IDictionary<string, string> cfgDictionary)
        {
            _cfgDictionary = cfgDictionary;
        }

        public void Execute()
        {
            ConnectToDatabase();
            CollectData();
            GenerateOutputFiles();
            Cleanup();
        }

        private void ConnectToDatabase()
        {
            _dbConnection = new OracleConnection(
                new OracleConnectionStringProvider().GetConnectionString(_cfgDictionary));
            _dbConnection.Open();
        }

        private void CollectData()
        {
        }

        private void GenerateOutputFiles()
        {
        }

        private void Cleanup()
        {
            if (null == _dbConnection)
            {
                return;
            }

            _dbConnection.Close();
            _dbConnection = null;
        }
    }
}
