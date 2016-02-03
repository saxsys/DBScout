using System;

namespace DBScout.Connectors
{
    public static class DatabaseConnectorFactory
    {
        public static IDatabaseConnector CreateDatabaseConnector(string qualifiedTypeName)
        {
            return CreateDatabaseConnector(Type.GetType(qualifiedTypeName));
        }

        public static IDatabaseConnector CreateDatabaseConnector(Type classType)
        {
            return Activator.CreateInstance(classType) as IDatabaseConnector;
        }
    }
}
