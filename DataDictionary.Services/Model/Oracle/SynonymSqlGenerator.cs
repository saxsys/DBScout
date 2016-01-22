using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class SynonymSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "SynonymTemplate.txt";

        private readonly DBA_SYNONYMS _synonym;

        public SynonymSqlGenerator(DataDictionaryDbContext dbContext, DBA_SYNONYMS synonym) 
            : base(dbContext)
        {
            _synonym = synonym;
        }

        public override string GetSqlString()
        {
            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[PUBLIC]", _synonym.OWNER.Equals("PUBLIC") ? " public" : string.Empty)
                .Replace("[SYNONYM_NAME]", _synonym.SYNONYM_NAME.ToLowerInvariant())
                .Replace("[TABLE_OWNER]",
                    _synonym.TABLE_OWNER != _synonym.OWNER
                        ? _synonym.TABLE_OWNER.ToLowerInvariant() + "."
                        : string.Empty)
                .Replace("[TABLE_NAME]", _synonym.TABLE_NAME.ToLowerInvariant())
                .Replace("[DB_LINK]",
                    string.IsNullOrEmpty(_synonym.DB_LINK) 
                        ? string.Empty 
                        : "@" + _synonym.DB_LINK.ToLowerInvariant());
        }
    }
}