using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using DbScout.Contracts;
using Oracle.ManagedDataAccess.Client;

namespace OracleConnector
{
    public class OracleDatabaseReader : IDatabaseReader, IConfigurable
    {
        private IConfiguration _cfg;

        public void Configure(IConfiguration cfg)
        {
            _cfg = cfg;
        }

        public ICollection<IDatabaseObject> ReadDatabaseObjects()
        {
            var databaseObject = new DatabaseObject {Type = _cfg.GetMandatoryConfigValue("RootObject") };
            LoadObject(databaseObject);

            return new List<IDatabaseObject> { databaseObject };
        }

        private void LoadObject(IDatabaseObject dbObject)
        {
            if (null == dbObject)
            {
                return;
            }

            var objectType = dbObject.Type;
            var dataDictionaryTablesString = _cfg.GetMandatoryConfigValue($"{objectType}.DataDictionaryTables");
            var dataDictionaryTables = dataDictionaryTablesString.Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var dataDictionaryTable in dataDictionaryTables)
            {
                if (dbObject.Properties.ContainsKey(dataDictionaryTable))
                {
                    continue;
                }

                var sqlCmd = _cfg.GetConfigValue($"{objectType}.{dataDictionaryTable}.SQL");
                sqlCmd = ApplyContentToCriteria(dbObject, sqlCmd);

                var propertiesDictionary = new Dictionary<string, object>();
                using (var cmd = new OracleCommand())
                {
                    cmd.Connection = GetDbConnection();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlCmd;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        for (var fieldIndex = 0; fieldIndex < reader.FieldCount; fieldIndex++)
                        {
                            propertiesDictionary.Add(reader.GetName(fieldIndex).ToLowerInvariant(), reader.GetValue(fieldIndex));
                        }
                    }
                }

                dbObject.Properties.Add(dataDictionaryTable.ToLowerInvariant(), propertiesDictionary);
            }

            // ermitteln der child objects
            LoadChildObjects(dbObject);
        }

        private void LoadChildObjects(IDatabaseObject dbObject)
        {
            if (null == dbObject)
            {
                return;
            }

            var objectType = dbObject.Type;
            var childObjectsString = _cfg.GetConfigValue($"{objectType}.ChildObjectTypes");
            if (string.IsNullOrEmpty(childObjectsString))
            {
                return;
            }

            var childObjectTypes = childObjectsString.Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var childObjectType in childObjectTypes)
            {
                var dataDictionaryTablesString = _cfg.GetMandatoryConfigValue($"{childObjectType}.DataDictionaryTables");
                var dataDictionaryTables = dataDictionaryTablesString.Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var primaryDataDictionaryTable = dataDictionaryTables.First();

                var sqlCmd = _cfg.GetConfigValue($"{childObjectType}.{primaryDataDictionaryTable}.SQL");
                sqlCmd = ApplyContentToCriteria(dbObject, sqlCmd);

                using (var cmd = new OracleCommand())
                {
                    cmd.Connection = GetDbConnection();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlCmd;

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var childObject = new DatabaseObject { Type = childObjectType, ParentObject = dbObject };
                        var propertiesDictionary = new Dictionary<string, object>();

                        for (var fieldIndex = 0; fieldIndex < reader.FieldCount; fieldIndex++)
                        {
                            propertiesDictionary.Add(reader.GetName(fieldIndex).ToLowerInvariant(), reader.GetValue(fieldIndex));
                        }
                        childObject.Properties.Add(primaryDataDictionaryTable.ToLowerInvariant(),propertiesDictionary);
                        LoadObject(childObject);
                        dbObject.ChildObjects.Add(childObject);
                    }
                }
            }
        }

        private static string ApplyContentToCriteria(IDatabaseObject databaseObject, string criteria)
        {
            var regex = new Regex("{[^{]*\\.[^{]*}");
            var processedCriteria = criteria;

            while (regex.IsMatch(processedCriteria))
            {
                var currentMatch = regex.Match(processedCriteria);
                foreach (var capture in currentMatch.Captures)
                {
                    var replacementString = capture.ToString();
                    replacementString = replacementString.Replace("{", string.Empty).Replace("}", string.Empty);
                    var replacementStringParts = replacementString.Split(".".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);

                    var tableName = replacementStringParts.Length > 0 ? replacementStringParts[0] : string.Empty;
                    if (string.IsNullOrEmpty(tableName))
                    {
                        continue;
                    }

                    var properties = databaseObject.Properties;
                    while (null != properties)
                    {
                        if (properties.ContainsKey(tableName))
                        {
                            break;
                        }
                        properties = databaseObject.ParentObject?.Properties;
                    }

                    if (null == properties)
                    {
                        continue;
                    }

                    var fieldName = replacementStringParts.Length > 1 ? replacementStringParts[1] : string.Empty;
                    if (string.IsNullOrEmpty(fieldName))
                    {
                        continue;
                    }

                    if (!properties[tableName].ContainsKey(fieldName))
                    {
                        continue;
                    }

                    var fieldValue = properties[tableName][fieldName].ToString();
                    processedCriteria = processedCriteria.Replace(capture.ToString(), fieldValue);
                }
            }
            return processedCriteria;
        }

        private OracleConnection _dbConnection;

        private OracleConnection GetDbConnection()
        {
            if (null != _dbConnection)
            {
                return _dbConnection;
            }

            _dbConnection = new OracleConnection(_cfg.GetMandatoryConfigValue("ConnectionString"));
            _dbConnection.Open();

            return _dbConnection;
        }
    }
}
