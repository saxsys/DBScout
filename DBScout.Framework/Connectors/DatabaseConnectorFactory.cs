using System;

namespace DBScout.Connectors
{
    public static class DatabaseConnectorFactory
    {
        public static IDatabaseConnector CreateDatabaseConnector(string qualifiedTypeName, string providerName = "", string userName = "", string password = "")
        {
            return CreateDatabaseConnector(Type.GetType(qualifiedTypeName),providerName,userName,password);
        }

        public static IDatabaseConnector CreateDatabaseConnector(Type classType, string providerName = "", string userName = "", string password = "")
        {
            var dbConnectorInstance = Activator.CreateInstance(classType) as IDatabaseConnector;

            // TODO: Assign connection string to NHibernate configuration

            return dbConnectorInstance;
        }
    }
}
