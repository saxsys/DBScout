using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbScout.UnitTests
{
    [TestClass]
    public class ReadDataDictionaryCommandUnitTests
    {
        [TestMethod]
        public void TestInstanceCreatedSuccess()
        {
            Assert.IsNotNull(new ReadDataDictionaryCommand());
        }

        [TestMethod]
        public void TestExecuteSuccess()
        {
            var sut = new ReadDataDictionaryCommand {ConnectionStringName = "TestDb"};
            sut.Execute();

            Assert.IsNotNull(sut.DataDictionaryContent);
        }
    }
}
