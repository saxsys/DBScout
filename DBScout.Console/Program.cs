using DBScout.Connectors;
using DBScout.Controller;
using System;

namespace DBScout.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var dbSettings = new DatabaseSettings(args);
                var exportSettings = new ExportSettings(args);
                var connectorDefinitions = new ConnectorSettings(args);

                var dbConnector = DatabaseConnectorFactory.CreateDatabaseConnector(connectorDefinitions.DbConnectorClassName,dbSettings.Provider,dbSettings.Username,dbSettings.Password);
                if (null == dbConnector)
                {
                    throw new Exception(string.Format("Couldn't create instance of \"{0}\"",connectorDefinitions.DbConnectorClassName));
                }
                dbConnector.Init(dbSettings.GetInitializationParameters());

                var dbInfoProcessor = DbInfoProcessorFactory.CreateDbInfoProcessor(connectorDefinitions.DbInfoProcessorClassName);
                if (null == dbInfoProcessor)
                {
                    throw new Exception(string.Format("Couldn't create instance of \"{0}\"", connectorDefinitions.DbInfoProcessorClassName));
                }

                var controller = new MainController();
                var dbInfos = controller.AcquireDatabaseSchemaInformation(dbConnector);
                controller.ProcessDatabaseSchema(dbInfos,dbInfoProcessor);
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.Message);
            }
        }
    }
}
