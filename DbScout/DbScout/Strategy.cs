using DbScout.Contracts;

namespace DbScout
{
    /// <summary>
    /// This class provides the common infrastructure for a strategy pattern implementation.
    /// The strategy simply defines a <see cref="Run"/> method, which is executed by the client. To setup
    /// the strategy, all necessary ICommand instances must be registered using <see cref="AddCommand"/>.
    /// The order of the <see cref="ICommand"/> registrations define the order of execution.
    /// </summary>
    public class Strategy : IStrategy
    {
        /// <summary>
        /// Definition of a delegate signature to be used in <see cref="Run"/> method.
        /// </summary>
        private delegate void RunStrategyDelegate();

        /// <summary>
        /// Delegate instance to receive the <see cref="ICommand.Execute"/> references.
        /// </summary>
        private RunStrategyDelegate _runStrategy;

        /// <summary>
        /// Strategy execution method. The multicast delegate instance <see cref="_runStrategy"/> is 
        /// executed sequentially.
        /// </summary>
        public void Run()
        {
            _runStrategy?.Invoke();
        }

        /// <summary>
        /// Register a new <see cref="ICommand"/> instance for command execution. The <see cref="ICommand.Execute"/> method is 
        /// appended to the multicast delegate <see cref="_runStrategy"/>. The order of appending commands to the strategy
        /// defines the order of later execution.
        /// </summary>
        /// <param name="command"><see cref="ICommand"/> implementation to be appended to the execution queue</param>
        public void AddCommand(ICommand command)
        {
            _runStrategy += command.Execute;
        }
    }
}
