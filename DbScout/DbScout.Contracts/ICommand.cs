namespace DbScout.Contracts
{
    /// <summary>
    /// Interface definition for command implementations.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Command execution method.
        /// </summary>
        void Execute();
    }
}
