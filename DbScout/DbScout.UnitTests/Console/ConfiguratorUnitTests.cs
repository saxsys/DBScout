using System;
using DbScout.Commands;
using DbScout.Console;
using DbScout.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbScout.UnitTests.Console
{
    [TestClass]
    public class ConfiguratorUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception),AllowDerivedTypes = true)]
        public void TestGetCommandInstanceThrowsExceptionWhenCalledWithoutConfiguration()
        {
            Configurator.GetCommandInstance(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestGetCommandInstanceThrowsExceptionForNotCommandType()
        {
            var assemblyQualifiedName = typeof(String).AssemblyQualifiedName;
            Configurator.GetCommandInstance(new[] { assemblyQualifiedName });
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestGetCommandInstanceThrowsExceptionForInvalidTypeName()
        {
            var assemblyQualifiedName = "InnvalidTypeName";
            Configurator.GetCommandInstance(new[] { assemblyQualifiedName });
        }


        [TestMethod]
        public void TestGetCommandInstanceSuccessForExistingTypeOfCommandInterface()
        {
            var assemblyQualifiedName = typeof(TestCommand).AssemblyQualifiedName;
            var cmdInstance = Configurator.GetCommandInstance(new[] { assemblyQualifiedName });
            Assert.IsNotNull(cmdInstance);
            Assert.IsInstanceOfType(cmdInstance, typeof(ICommand));
        }
    }
}
