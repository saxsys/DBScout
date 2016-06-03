using DbScout.Commands;
using DbScout.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbScout.UnitTests.Console
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für UnitTest1
    /// </summary>
    [TestClass]
    public class GenerateSqlFilesFromOracleDataDictionaryCommandUnitTests
    {
        public void TestGenerateSqlFilesFromOracleDataDictionaryCommandInstanceCreatedSuccess()
        {
            var sut = new GenerateSqlFilesFromOracleDataDictionaryCommand();
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut,typeof(ICommand));
        }

    }
}
