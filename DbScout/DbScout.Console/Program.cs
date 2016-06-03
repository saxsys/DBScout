using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using log4net;
using log4net.Config;

namespace DbScout.Console
{
    public class Program
    {
        /// <summary>
        /// Logger instance for this class
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            try
            {
                XmlConfigurator.Configure();

                var commandInstance = Configurator.GetCommandInstance(args.Any() ? args : new [] { ConfigurationManager.AppSettings[Constants.DefaultCommandKey]});

                Logger.InfoFormat("Executing command {0}...", commandInstance.GetType().Name);

                commandInstance.Execute();

                Logger.InfoFormat("Command {0} executed.",commandInstance.GetType().Name);
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("Error executing application: {0}",e.Message);
                Logger.ErrorFormat("Stack trace: {0}", e.StackTrace);
            }
        }
    }
}
