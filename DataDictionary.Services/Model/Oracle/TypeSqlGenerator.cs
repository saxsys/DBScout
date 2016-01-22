using System.Linq;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class TypeSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "TypeTemplate.txt";

        private readonly DBA_TYPES _type;
        
        public TypeSqlGenerator(DataDictionaryDbContext dbContext, DBA_TYPES type) 
            : base(dbContext)
        {
            _type = type;
        }

        public override string GetSqlString()
        {
            var typeDefString = new CollectionToString(
                DbContext.DBA_SOURCE
                    .Where(
                        s =>
                            s.OWNER == _type.OWNER &&
                            s.NAME == _type.TYPE_NAME &&
                            s.TYPE.Equals("TYPE"))
                    .OrderBy(s => s.LINE)
                    .Select(s => s.TEXT)
                    .ToList(),
                string.Empty).GetAsString();

            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[TYPE_DEF]", typeDefString);
        }
    }
}
