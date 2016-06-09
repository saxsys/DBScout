using System;
using System.Collections.Generic;
using DbScout.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbScout.UnitTests
{
    [TestClass]
    public class OracleConnectionStringProviderTests
    {
        [TestMethod]
        public void TestInstanceCreatedSuccess()
        {
            var sut = new OracleConnectionStringProvider();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), AllowDerivedTypes = false)]
        public void TestGetConnectionStringThrowsExceptionWhenCfgInstanceIsInvalid()
        {
            var sut = new OracleConnectionStringProvider();
            sut.GetConnectionString(null);
        }

        [TestMethod]
        public void TestGetConnectionStringSuccess()
        {
            var sut = new OracleConnectionStringProvider();
            const string expectedConnectionString = "USER ID=UserID;DATA SOURCE=DataSource;PASSWORD=Password";

            var connectionString = sut.GetConnectionString(
                new Dictionary<string, string>
                {
                    {"DataSource", "DataSource"},
                    {"UserID", "UserID"},
                    {"Password", "Password"}
                });
            Assert.IsNotNull(connectionString);
            Assert.AreEqual(expectedConnectionString.ToLowerInvariant(), connectionString.ToLowerInvariant());
        }
    }
}
