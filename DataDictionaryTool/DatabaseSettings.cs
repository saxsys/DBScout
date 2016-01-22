using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace DataDictionaryTool
{
    public class DatabaseSettings
    {
        public string Sid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public DatabaseSettings(IEnumerable<string> cmdLineArgs)
        {
            InitFromConfiguration();
            InitFromCommandLine(cmdLineArgs);
        }

        private void InitFromConfiguration()
        {
            var appSettingsReader = new AppSettingsReader();
            Sid = appSettingsReader.GetValue("database.sid", typeof(string)).ToString();
            Username = appSettingsReader.GetValue("database.username", typeof(string)).ToString();
            Password = appSettingsReader.GetValue("database.password", typeof(string)).ToString();
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
                    case "sid":
                        Sid = paramValue;
                        break;
                    case "username":
                        Username = paramValue;
                        break;
                    case "password":
                        Password = paramValue;
                        break;
                }
            }
        }

        public string GetOracleConnectionString()
        {
            var connectionStringBuilder = new StringBuilder();
            
            connectionStringBuilder.Append("User Id=");
            connectionStringBuilder.Append(Username);
            connectionStringBuilder.Append(";Password=");
            connectionStringBuilder.Append(Password);
            connectionStringBuilder.Append(";Data Source=");
            connectionStringBuilder.Append(Sid);
            connectionStringBuilder.Append(";");
            
            return connectionStringBuilder.ToString();
        }
    }
}
