using DataDictionary.Services.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataDictionary.Services.UnitTests
{
    [TestClass]
    public class DataDictionaryControllerTests
    {
        private DataDictionaryController _cut;
        
        [TestInitialize]
        public void Initialize()
        {
            _cut = new DataDictionaryController(TestDefinitions.OracleConnectionString);
        }

        [TestMethod]
        public void TestInstanceCreatedSuccess()
        {
            Assert.IsNotNull(_cut);
        }

        [TestMethod]
        public void TestDataDictionaryModelCreationSuccess()
        {
            Assert.IsNotNull(_cut);
            _cut.SchemaName = "EEG_PROD_2";
            _cut.RootPath = "Output";

            _cut.CreateSqlFilesFromDatabaseModel();
            Assert.IsTrue(true);
        }
    }
}
