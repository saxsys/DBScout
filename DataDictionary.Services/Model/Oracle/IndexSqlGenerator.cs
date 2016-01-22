using System.Linq;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class IndexSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "IndexTemplate.txt";

        private readonly DBA_INDEXES _index;

        public IndexSqlGenerator(DataDictionaryDbContext dbContext, DBA_INDEXES index) 
            : base(dbContext)
        {
            _index = index;
        }

        public override string GetSqlString()
        {
            var indexColumnList = new CollectionToString(
                DbContext.DBA_IND_COLUMNS
                    .Where(c => c.INDEX_OWNER == _index.OWNER && c.INDEX_NAME == _index.INDEX_NAME)
                    .OrderBy(c => c.COLUMN_POSITION)
                    .Select(
                        c =>
                            c.COLUMN_NAME +
                            (c.DESCEND.Equals("DESC") ? " desc" : string.Empty))
                    .ToList()).GetAsString().ToLowerInvariant();
            
            var tablespaceName = _index.TABLESPACE_NAME ?? GetDefaultTablespace(_index.OWNER);

            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[BITMAP_UNIQUE]", _index.INDEX_TYPE.Equals("BITMAP")
                    ? " bitmap"
                    : _index.INDEX_TYPE.Equals("UNIQUE")
                        ? " unique"
                        : string.Empty)
                .Replace("[INDEX_NAME]", _index.INDEX_NAME.ToLowerInvariant())
                .Replace("[COLUMN_LIST]", indexColumnList)
                .Replace("[TABLE_NAME]", _index.TABLE_NAME.ToLowerInvariant())
                .Replace("[TABLESPACE_NAME]", tablespaceName.ToLowerInvariant());
        }
    }
}
