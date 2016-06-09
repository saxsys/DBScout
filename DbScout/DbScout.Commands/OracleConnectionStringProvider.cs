using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oracle.ManagedDataAccess.Client;

namespace DbScout.Commands
{
    public class OracleConnectionStringProvider
    {
        private OracleConnectionStringBuilder _connectionStringBuilder;

        public string GetConnectionString(IDictionary<string, string> cfgParameters)
        {
            VerifyConfigurationDictionary(cfgParameters);

            AssignConfigurationParameters(cfgParameters);

            return _connectionStringBuilder.ToString();
        }

        private static void VerifyConfigurationDictionary(IDictionary<string, string> cfgParameters)
        {
            if (null == cfgParameters)
            {
                throw new ArgumentNullException(nameof(cfgParameters), "Parameter cfgParameters is invalid!");
            }
        }

        private void AssignConfigurationParameters(IDictionary<string, string> cfgParameters)
        {
            var connectionStringBuilderType = typeof(OracleConnectionStringBuilder);
            _connectionStringBuilder = new OracleConnectionStringBuilder();

            foreach (
                var property in
                    typeof(OracleConnectionStringBuilder).GetProperties().Where(p => cfgParameters.ContainsKey(p.Name))
                )
            {
                connectionStringBuilderType.InvokeMember(
                    property.Name,
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public,
                    null,
                    _connectionStringBuilder,
                    new object[] { cfgParameters[property.Name] } );
            }
        }
    }
}
