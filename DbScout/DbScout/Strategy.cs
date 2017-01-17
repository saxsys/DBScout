using DbScout.Contracts;

namespace DbScout
{
    public class Strategy : IStrategy
    {
        private delegate void RunStrategyDelegate();

        private RunStrategyDelegate _runStrategy;

        public void Run()
        {
            _runStrategy?.Invoke();
        }

        public void AddCommand(ICommand command)
        {
            _runStrategy += command.Execute;
        }
    }
}
