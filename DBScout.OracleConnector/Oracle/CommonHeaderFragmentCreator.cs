using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class CommonHeaderFragmentCreator : SqlScriptCreator
    {
        private const string TemplateFileName = "CommonFileHeaderTemplate.txt";
        
        private readonly string _fileName;
        private readonly string _objectType;
        private readonly string _objectName;

        public CommonHeaderFragmentCreator(
            DataDictionaryDbContext dbContext, 
            string fileName,
            string objectType,
            string objectName) 
            : base(dbContext)
        {
            _fileName = fileName;
            _objectName = objectName;
            _objectType = objectType;
        }

        public override string GetSqlString()
        {
            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[FILENAME]", _fileName.ToLowerInvariant())
                .Replace("[OBJECTTYPE]", _objectType.ToLowerInvariant())
                .Replace("[OBJECTNAME]", _objectName.ToLowerInvariant());
        }
    }
}
