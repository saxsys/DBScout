using System.Data;
using DBScout.DatabaseSettings.ViewModels;
using DynamicSpecs.NUnit;
using NUnit.Framework;


namespace DBScout.DatabaseSettings.Test
{
 
    public class WhenAllFieldsFilledAValidConnectionstringIsProvided:Specifies<DbSettingsViewModel>
    {
        private string _result;

        public override void Given()
        {
            //SUT.DbSid = "testdb";
            //SUT.DbUser = "testuser";
            //SUT.DbPassword = "123";
            //SUT.ExportShema = "dbo";
            //SUT.ExportRootPath = "Output";
            //SUT.DbProvider = "Provider";

        }

        public override void When()
        {
            this._result = this.SUT.GetConnectionString();
        }

        [Test]
        public void ThenAConnectionStringShouldBeThere()
        {
          Assert.IsNotEmpty(_result);
        }
    }
}
