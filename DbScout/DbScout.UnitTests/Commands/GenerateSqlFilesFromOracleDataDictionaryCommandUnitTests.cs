using System;
using DbScout.Commands;
using DbScout.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbScout.UnitTests.Commands
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für UnitTest1
    /// </summary>
    [TestClass]
    public class GenerateSqlFilesFromOracleDataDictionaryCommandUnitTests
    {
        [TestMethod]
        public void TestGenerateSqlFilesFromOracleDataDictionaryCommandInstanceCreatedSuccess()
        {
            var sut = new GenerateSqlFilesFromOracleDataDictionaryCommand();
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(ICommand));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestCommandExecutionThrowsExceptionWhenInvalidCfgInstance()
        {
            var sut = new GenerateSqlFilesFromOracleDataDictionaryCommand();
            sut.Configure(null);
            sut.Execute();
        }
    }
}
