using System;
using System.Linq;
using DbScout.Contracts;

namespace DbScout.Console
{
    public static class Configurator
    {
        public static ICommand GetCommandInstance(string[] cmdLineArguments)
        {
            if (cmdLineArguments == null)
            {
                throw new Exception("Cannot create command instance, no arguments supplied.");
            }

            var firstCmdLineArgument = cmdLineArguments.FirstOrDefault();
            if (firstCmdLineArgument == null)
            {
                throw new Exception("Cannot create command instance, first command line argument is invalid.");
            }

            var typeOfCommand = Type.GetType(firstCmdLineArgument);
            if (typeOfCommand == null)
            {
                throw new Exception("Cannot create command instance, Type.GetType() returned invalid instance.");
            }

            var cmdInstance = Activator.CreateInstance(typeOfCommand) as ICommand;
            if (cmdInstance == null)
            {
                throw new Exception(
                    $"Cannot create command instance, specified type \"{typeOfCommand.FullName}\" does not implement ICommand interface.");
            }
            return cmdInstance;
        }
    }
}
