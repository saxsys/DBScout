
using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace EFModel.DataDictionary
{
    public class DataDictionaryDbContextFactory : IDataDictionaryDbContextFactory
    {
        private string OracleConnectionString { get; set; }

        public DataDictionaryDbContextFactory(string oracleConnectionString)
        {
            OracleConnectionString = oracleConnectionString;
        }

        public DataDictionaryDbContext Create()
        {
            // DbContext erzeugen, je nach Einstellung basierend auf einem Oracle Connection String oder mit Default
            var dbContext = new DataDictionaryDbContext(TransformOracleConnectionString(OracleConnectionString));
            return Customize(dbContext);
        }

        private static string TransformOracleConnectionString(string oracleConnectionString)
        {
            var cfgConnectionStringSettings = ConfigurationManager.ConnectionStrings["DataDictionary"];
            if (cfgConnectionStringSettings == null)
            {
                throw new Exception("Für den Schlüssel \"DataDictionary\" ist in der app.config kein Eintrag vorhanden!");
            }

            var devartConnectionString = cfgConnectionStringSettings.ConnectionString;
            const RegexOptions opt = RegexOptions.IgnoreCase;

            // Oracle connection string must follow the pattern: "Password=XXX;User ID=XXX;Data Source=XXX"
            // Extract the Oracle parts
            var oraclePwd = Regex.Match(oracleConnectionString, "(Password=.+?)(;|$)", opt).Groups[1].Value;
            var oracleUsr = Regex.Match(oracleConnectionString, "(User ID=.+?)(;|$)", opt).Groups[1].Value;
            var oracleSvr = Regex.Match(oracleConnectionString, "(Data Source=.+?)(;|$)", opt).Groups[1].Value;

            // DevArt connectionString must follow the pattern ="metadata=******;provider connection string=&quot;User Id=EEG_TEST_2;Password=erneuerbar;Server=vera05.world;Persist Security Info=True&quot;" 
            // Extract the DevArt parts
            var devartPwd = Regex.Match(devartConnectionString, "(Password=.+?)(;|$)", opt).Groups[1].Value;
            var devartUsr = Regex.Match(devartConnectionString, "(User ID=.+?)(;|$)", opt).Groups[1].Value;
            var devartSvr = Regex.Match(devartConnectionString, "(Server=.+?)(;|$)", opt).Groups[1].Value;

            // Convert Oracle param "Data Source" => DevArt param "Server"
            oracleSvr = oracleSvr.Replace("Data Source", "Server");

            // Replace parts in the DevArt connection string
            return devartConnectionString
                .Replace(devartPwd, oraclePwd)
                .Replace(devartUsr, oracleUsr)
                .Replace(devartSvr, oracleSvr);
        }

        /// <summary>
        /// Macht einige nette Einstellungen an dem übergebenen DbConext.
        /// </summary>
        private static DataDictionaryDbContext Customize(DataDictionaryDbContext dbContext)
        {
            // Anweisung an der DevArt Treiber, den im Model konfigurierten Default-Wert zu verwenden, falls wir per Code keinen Wert gesetzt haben
            // Ist Voraussetzung für die Zuweisung der PKs via Oracle Sequence
            // Siehe http://blog.devart.com/set-identity-and-computed-properties-in-entity-framework-without-triggers.html
            var oracleConfig = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
            oracleConfig.DmlOptions.InsertNullBehaviour = Devart.Data.Oracle.Entity.Configuration.InsertNullBehaviour.InsertDefaultOrOmit;

            // DevArt (kann) den Schema-Name mit abspeichern und in den Queries verwenden.
            // Damit kann man dann nicht mehr gegen andere Datenbanken arbeiten, deswegen IgnoreSchemaName
            // Siehe http://blog.devart.com/new-features-of-entity-framework-support-in-dotconnect-providers.html#Workarounds
            oracleConfig.Workarounds.IgnoreSchemaName = true;

            return dbContext;
        }
    }
}
