using DataDictionary.Services.Controller;
using System;

namespace DataDictionaryTool
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var dbSettings = new DatabaseSettings(args);
                var exportSettings = new ExportSettings(args);

                var controller = new DataDictionaryController(dbSettings.GetOracleConnectionString())
                {
                    RootPath = exportSettings.RootPath,
                    SchemaName = exportSettings.SchemaName
                };

                controller.CreateSqlFilesFromDatabaseModel();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}
