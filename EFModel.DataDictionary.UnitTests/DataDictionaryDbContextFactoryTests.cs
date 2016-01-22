using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFModel.DataDictionary.UnitTests
{
    [TestClass]
    public class DataDictionaryDbContextFactoryTests
    {
        private IDataDictionaryDbContextFactory _cut;

        [TestInitialize]
        public void Initialize()
        {
            _cut = new DataDictionaryDbContextFactory(TestDefinitions.OracleConnectionString);
        }

        [TestMethod]
        public void TestInstanceCreatedSuccess()
        {
            Assert.IsNotNull(_cut);
        }

        [TestMethod]
        public void TestCreateSuccess()
        {
            Assert.IsNotNull(_cut);
            var testObject = _cut.Create();
            Assert.IsNotNull(testObject);
            Assert.IsInstanceOfType(testObject,typeof(DataDictionaryDbContext));
        }
    }
}
