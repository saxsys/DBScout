using System.Collections.Generic;
using DbScout.Contracts;
using DbScout.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OracleConnector;

namespace DbScout.UnitTests
{
    [TestClass]
    public class OracleDatabaseReaderTests
    {
        [TestMethod]
        public void TestReadDatabaseObjectSuccess()
        {
            var sut = new OracleDatabaseReader();
            sut.Configure(new AppSettingsConfiguration());

            var resultObject = sut.ReadDatabaseObjects();

            Assert.IsNotNull(resultObject);
            Assert.IsInstanceOfType(resultObject,typeof(ICollection<IDatabaseObject>));
        }
    }
}
