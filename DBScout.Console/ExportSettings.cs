using System.Collections.Generic;
using System.Configuration;

namespace DBScout.Console
{
    public class ExportSettings
    {
        public string SchemaName { get; set; }
        public string RootPath { get; set; }

        public ExportSettings(IEnumerable<string> cmdLineArgs)
        {
            InitFromConfiguration();
            InitFromCommandLine(cmdLineArgs);
        }

        private void InitFromConfiguration()
        {
            var appSettingsReader = new AppSettingsReader();
            SchemaName = appSettingsReader.GetValue("export.schema", typeof(string)).ToString();
            RootPath = appSettingsReader.GetValue("export.root_path", typeof(string)).ToString();
        }

        private void InitFromCommandLine(IEnumerable<string> cmdLineArgs)
        {
            foreach (var arg in cmdLineArgs)
            {
                var paramAndKey = arg.Split("=".ToCharArray());
                if (paramAndKey.Length < 2)
                {
                    continue;
                }

                var paramName = paramAndKey[0].Trim().ToLowerInvariant();
                var paramValue = paramAndKey[1];

                switch (paramName)
                {
                    case "schema":
                        SchemaName = paramValue;
                        break;
                    case "rootpath":
                        RootPath = paramValue;
                        break;
                }
            }
        }
    }
}
