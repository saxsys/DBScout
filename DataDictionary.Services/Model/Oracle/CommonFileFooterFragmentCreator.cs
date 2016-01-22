using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class CommonFileFooterFragmentCreator : SqlScriptCreator
    {
        private const string TemplateFileName = "CommonFileFooterTemplate.txt";

        private readonly string _fileName;
        
        public CommonFileFooterFragmentCreator(DataDictionaryDbContext dbContext,string fileName) 
            : base(dbContext)
        {
            _fileName = fileName;
        }

        public override string GetSqlString()
        {
            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[FILENAME]", _fileName.ToLowerInvariant());
        }
    }
}
