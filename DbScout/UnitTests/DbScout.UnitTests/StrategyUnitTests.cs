using DbScout.Contracts;
using DbScout.UnitTests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbScout.UnitTests
{
    [TestClass]
    public class StrategyUnitTests
    {
        [TestMethod]
        public void TestInstanceCreatedSuccess()
        {
            Assert.IsNotNull(new Strategy());
        }

        [TestMethod]
        public void TestInstanceIsTypeOfInterface()
        {
            Assert.IsInstanceOfType(new Strategy(),typeof(IStrategy));
        }

        [TestMethod]
        public void TestAddCommandSuccess()
        {
            var sut = new Strategy();
            sut.AddCommand(new CommandMock());
        }

        [TestMethod]
        public void TestExecuteSuccess()
        {
            // create 3 mock command instances
            var cmd1 = new CommandMock();
            var cmd2 = new CommandMock();
            var cmd3 = new CommandMock();

            // execution count == 0 for all commands
            const int expectedExecutionCountBeforeRun = 0;
            Assert.AreEqual(expectedExecutionCountBeforeRun, cmd1.ExecutionCount);
            Assert.AreEqual(expectedExecutionCountBeforeRun, cmd2.ExecutionCount);
            Assert.AreEqual(expectedExecutionCountBeforeRun, cmd3.ExecutionCount);

            // add all 3 mock command instances to strategy
            var sut = new Strategy();
            sut.AddCommand(cmd1);
            sut.AddCommand(cmd2);
            sut.AddCommand(cmd3);

            // strategy execution:
            sut.Run();

            // each command is executed once:
            const int expectedExecutionCountAfterRun = 1;
            Assert.AreEqual(expectedExecutionCountAfterRun, cmd1.ExecutionCount);
            Assert.AreEqual(expectedExecutionCountAfterRun, cmd2.ExecutionCount);
            Assert.AreEqual(expectedExecutionCountAfterRun, cmd3.ExecutionCount);
        }
    }
}
