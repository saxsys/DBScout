using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class DbLinkSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "DatabaseLinkTemplate.txt";
        
        private readonly DBA_DB_LINKS _dblink;

        public DbLinkSqlGenerator(DataDictionaryDbContext dbContext, DBA_DB_LINKS dblink)
            : base(dbContext)
        {
            _dblink = dblink;
        }

        public override string GetSqlString()
        {
            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[DB_LINK]",_dblink.DB_LINK.ToLowerInvariant())
                .Replace("[USERNAME]",_dblink.USERNAME.ToLowerInvariant())
                .Replace("[CONNECTSTRING]",_dblink.HOST);
        }
    }
}
