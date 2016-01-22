using System.Linq;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class FunctionsSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "FunctionTemplate.txt";

        private readonly DBA_PROCEDURES _function;

        public FunctionsSqlGenerator(DataDictionaryDbContext dbContext,DBA_PROCEDURES function) 
            : base(dbContext)
        {
            _function = function;
        }

        public override string GetSqlString()
        {
            var functionDefString = new CollectionToString(null, string.Empty)
                .GetAsString(
                    DbContext.DBA_SOURCE
                        .Where(
                            s =>
                                s.OWNER == _function.OWNER &&
                                s.NAME == _function.OBJECT_NAME &&
                                s.TYPE == _function.OBJECT_TYPE)
                        .OrderBy(s => s.LINE)
                        .Select(s => s.TEXT)
                        .ToList());

            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[FUNCTION_DEF]", functionDefString);
        }
    }
}
