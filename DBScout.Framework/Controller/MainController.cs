using DBScout.Connectors;
using DBScout.Model;
using System;

namespace DBScout.Controller
{
    public class MainController
    {
        public IDbObject AcquireDatabaseSchemaInformation(IDatabaseConnector dbConnector)
        {
            if (null == dbConnector)
            {
                throw new ArgumentNullException("dbConnector","Invalid IDatabaseConnector instance!");
            }
            return dbConnector.GetDatabaseInformation();
        }

        public void ProcessDatabaseSchema(IDbObject dbInfo, IDbInfoProcessor dbInfoProcessor)
        {
            if (null == dbInfo)
            {
                throw new ArgumentNullException("dbInfo", "Invalid IDbObject instance!");
            }
            if (null == dbInfoProcessor)
            {
                throw new ArgumentNullException("dbInfoProcessor", "Invalid IDbInfoProcessor instance!");
            }

            dbInfoProcessor.Process(dbInfo);
        }
    }
}
