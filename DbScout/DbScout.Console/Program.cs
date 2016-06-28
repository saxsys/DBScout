using System;
using System.Reflection;
using DbScout.Contracts;
using DbScout.Services;
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

        private const string CommandKey = "Command";

        public static void Main(string[] args)
        {
            try
            {
                XmlConfigurator.Configure();

                var cfg = new CommandLineConfiguration { CmdLineArgs = args };
                var commandInstance = new CommandFactory()
                    .CreateCommand(cfg.GetMandatoryConfigValue(CommandKey));

                (commandInstance as IConfigurable)?.Configure(cfg);

                Logger.Info($"Executing command {commandInstance.GetType().Name}...");

                commandInstance.Execute();

                Logger.Info($"Command {commandInstance.GetType().Name} executed.");
            }
            catch (Exception e)
            {
                Logger.Error($"Error executing application: {e.Message}");
                var innerException = e.InnerException;
                while (null != innerException)
                {
                    Logger.Error(innerException.Message);
                    innerException = innerException.InnerException;
                }
                Logger.Error($"Stack trace: {e.StackTrace}");
            }
        }
    }
}
