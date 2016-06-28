using System;
using DbScout.Commands;
using DbScout.Contracts;
using DbScout.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbScout.UnitTests.Services
{
    [TestClass]
    public class CommandFactoryTests
    {
        [TestMethod]
        public void TestInstanceCreatedSuccess()
        {
            var sut = new CommandFactory();
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut,typeof(ICommandFactory));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestCreateCommandThrowsExceptionForNotCommandType()
        {
            var assemblyQualifiedName = typeof(string).AssemblyQualifiedName;
            new CommandFactory().CreateCommand(assemblyQualifiedName);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestGetCommandInstanceThrowsExceptionForInvalidTypeName()
        {
            new CommandFactory().CreateCommand("InnvalidTypeName");
        }


        [TestMethod]
        public void TestGetCommandInstanceSuccessForExistingTypeOfCommandInterface()
        {
            var assemblyQualifiedName = typeof(TestCommand).AssemblyQualifiedName;
            var cmdInstance = new CommandFactory().CreateCommand(assemblyQualifiedName);
            Assert.IsNotNull(cmdInstance);
            Assert.IsInstanceOfType(cmdInstance, typeof(ICommand));
        }
    }
}
