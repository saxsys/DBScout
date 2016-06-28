using System;
using System.Collections.Generic;
using System.Linq;
using DbScout.Commands;
using DbScout.Contracts;
using DbScout.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbScout.UnitTests.Services
{
    [TestClass]
    public class ConfiguratorUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception),AllowDerivedTypes = true)]
        public void TestGetConfigurationThrowsExceptionWhenCalledWithoutConfiguration()
        {
            new CommandLineConfiguration().GetConfiguration();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestGetConfigurationThrowsExceptionWhenCalledWithAllMalformedConfiguration()
        {
            new CommandLineConfiguration {CmdLineArgs = new [] {"Param1", "Param2"}}.GetConfiguration();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestGetConfigurationThrowsExceptionWhenCalledWithOneMalformedConfiguration()
        {
            new CommandLineConfiguration { CmdLineArgs = new[] { "Param1", "Param2=value2" } }.GetConfiguration();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestGetConfigurationThrowsExceptionWhenCalledWithAnotherMalformedConfiguration()
        {
            new CommandLineConfiguration { CmdLineArgs = new[] { "Param1=value1=value11", "Param2=value2" } }.GetConfiguration();
        }

        [TestMethod]
        public void TestGetConfigurationSuccessWithValidConfiguration()
        {
            var cfgDict = new CommandLineConfiguration { CmdLineArgs = new[] { "Param1=value1", "Param2=value2" } }.GetConfiguration();
            Assert.IsNotNull(cfgDict);
            Assert.IsInstanceOfType(cfgDict, typeof(IDictionary<string,string>));
            Assert.AreEqual(2,cfgDict.Count);
            Assert.AreEqual("param1",cfgDict.First().Key);
            Assert.AreEqual("param2", cfgDict.Last().Key);
            Assert.AreEqual("value1", cfgDict.First().Value);
            Assert.AreEqual("value2", cfgDict.Last().Value);
        }
    }
}
