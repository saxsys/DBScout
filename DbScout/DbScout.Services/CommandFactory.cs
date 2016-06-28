using System;
using DbScout.Contracts;

namespace DbScout.Services
{
    public class CommandFactory : ICommandFactory
    {
        public ICommand CreateCommand(string cmdTypeName)
        {
            var typeOfCommand = Type.GetType(cmdTypeName);
            if (typeOfCommand == null)
            {
                throw new Exception(
                    $"Cannot create command instance, Type.GetType(\"{cmdTypeName}\") returned invalid instance.");
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
