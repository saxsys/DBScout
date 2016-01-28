using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBScout.Console
{
    public class ConnectorSettings
    {
        public string DbConnectorClassName { get; private set; }
        public string DbInfoProcessorClassName { get; private set; }

        public ConnectorSettings(string[] commandLineArgs)
        {
            DbConnectorClassName = string.Empty;
            DbInfoProcessorClassName = string.Empty;
        }
    }
}
