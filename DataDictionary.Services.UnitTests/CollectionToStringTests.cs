using System;
using System.Collections.Generic;
using System.Text;
using DataDictionary.Services.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataDictionary.Services.UnitTests
{
    [TestClass]
    public class CollectionToStringTests
    {
        private CollectionToString _cut;
        
        [TestInitialize]
        public void Initialize()
        {
            _cut = new CollectionToString();
        }

        [TestMethod]
        public void InstanceCreatedSuccess()
        {
            Assert.IsNotNull(_cut);
        }

        [TestMethod]
        public void InstanceCreatedDefaultPropertiesSuccess()
        {
            const string expectedSeparatorChar = ",";
            const int expectedTabSize = 2;
            const int expectedTabCount = 0;
            var expectedTabString = new string(' ', expectedTabSize);

            Assert.IsNotNull(_cut);

            Assert.IsNull(_cut.Collection);
            Assert.AreEqual(expectedSeparatorChar,_cut.SeparatorString);
            Assert.IsFalse(_cut.InsertNewLine);
            Assert.AreEqual(expectedTabSize,_cut.TabSize);
            Assert.AreEqual(expectedTabCount,_cut.TabCountForNewLine);
            Assert.IsFalse(_cut.StartWithNewLine);
            Assert.AreEqual(expectedTabString,_cut.TabString);
        }

        [TestMethod]
        public void TestGetAsStringWithoutNewLinesSuccess()
        {
            var collection = new List<string>
            {
                "item1",
                "item2",
                "item3"
            };

            const string expectedResult = "item1,item2,item3";

            Assert.IsNotNull(_cut);
            var currentResult = _cut.GetAsString(collection);

            Assert.AreEqual(expectedResult,currentResult);
        }

        [TestMethod]
        public void TestGetAsStringWithNewLinesSuccess()
        {
            var collection = new List<string>
            {
                "item1",
                "item2",
                "item3"
            };

            var sb = new StringBuilder();
            sb.AppendLine("item1,");
            sb.AppendLine("item2,");
            sb.Append("item3");
            var expectedResult = sb.ToString();

            Assert.IsNotNull(_cut);
            _cut.InsertNewLine = true;
            var currentResult = _cut.GetAsString(collection);

            Assert.AreEqual(expectedResult, currentResult);
        }
    }
}
