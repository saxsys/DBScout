namespace DbScout.Contracts
{
    /// <summary>
    /// Definition of interface for command factory implementation.
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>
        /// Create a new ICommand instance for specified command class type name.
        /// </summary>
        /// <param name="cmdTypeName">Type name of the command to be created.</param>
        /// <returns>Returns a new instance of ICommand, related to the specified type name.</returns>
        ICommand CreateCommand(string cmdTypeName);
    }
}
