using DbScout.Contracts;

namespace DbScout.UnitTests.Mock
{
    internal class CommandMock : ICommand
    {
        public int ExecutionCount { get; private set; }
        public void Execute()
        {
            ExecutionCount++;
        }
    }
}
