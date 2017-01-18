using System.Collections.Generic;
using System.Configuration;
using DbScout.Contracts;
using Oracle.ManagedDataAccess.Client;

namespace DbScout
{
    public class ReadDataDictionaryCommand : ICommand
    {
        public IDictionary<string,ICollection<IDictionary<string,object>>> DataDictionaryContent { get; set; }

        public string ConnectionStringName { get; set; }

        public void Execute()
        {
            DataDictionaryContent = new Dictionary<string, ICollection<IDictionary<string, object>>>();

            using (var dbConnection = new OracleConnection(GetConnectionString()))
            {
                dbConnection.Open();

                using (var selectCommand = new OracleCommand("select table_name,tablespace_name from user_tables order by table_name",dbConnection))
                {
                    var resultSet = selectCommand.ExecuteReader();
                    DataDictionaryContent["USER_TABLES"] = new List<IDictionary<string, object>>();
                    while (resultSet.Read())
                    {
                        var fieldValues = new Dictionary<string,object>();
                        for (var fieldIndex = 0; fieldIndex < resultSet.FieldCount; fieldIndex++)
                        {
                            fieldValues[resultSet.GetName(fieldIndex)] = resultSet.GetValue(fieldIndex);
                        }

                        DataDictionaryContent["USER_TABLES"].Add(fieldValues);
                    }
                }
                dbConnection.Close();
            }
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
        }
    }
}
