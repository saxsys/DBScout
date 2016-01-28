using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class IndexesSqlGenerator : SqlScriptCreator
    {
        private readonly DBA_TABLES _table;
        private ICollection<DBA_INDEXES> _indexCollection;

        public IndexesSqlGenerator(DataDictionaryDbContext dbContext, DBA_TABLES table) 
            : base(dbContext)
        {
            _table = table;
        }

        public override string GetSqlString()
        {
            LoadIndexCollection();

            var separatorString = string.Empty;
            const bool newLineBetweenCollectionItems = true;
            var collectionInstance = _indexCollection
                .Select(i => new IndexSqlGenerator(DbContext, i).GetSqlString())
                .ToList();

            var sqlStringBuilder = new StringBuilder();

            sqlStringBuilder.AppendLine(HeaderString("Indexe"));

            sqlStringBuilder.AppendLine(
                new CollectionToString(
                    collectionInstance,
                    separatorString,
                    newLineBetweenCollectionItems).GetAsString());

            return sqlStringBuilder.ToString();
        }

        private void LoadIndexCollection()
        {
            _indexCollection = DbContext.DBA_INDEXES
                .Where(i => i.TABLE_OWNER == _table.OWNER && i.TABLE_NAME == _table.TABLE_NAME)
                .OrderBy(i => i.INDEX_NAME)
                .ToList();
        }
    }
}
