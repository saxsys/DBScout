using System;

namespace DBScout.Connectors
{
    public static class DbInfoProcessorFactory
    {
        public static IDbInfoProcessor CreateDbInfoProcessor(string qualifiedTypeName)
        {
            return CreateDbInfoProcessor(Type.GetType(qualifiedTypeName));
        }

        public static IDbInfoProcessor CreateDbInfoProcessor(Type classType)
        {
            return Activator.CreateInstance(classType) as IDbInfoProcessor;
        }
    }
}
