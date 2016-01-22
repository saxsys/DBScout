using System.Collections.Generic;
using System.Linq;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class TableColumnsSqlGenerator : SqlScriptCreator
    {
        private readonly DBA_TABLES _table;
        private ICollection<DBA_TAB_COLUMNS> _tableColumns;

        public TableColumnsSqlGenerator(DataDictionaryDbContext dbContext, DBA_TABLES table) 
            : base(dbContext)
        {
            _table = table;
        }

        public override string GetSqlString()
        {
            LoadColumnsCollection();

            return new CollectionToString(
                _tableColumns
                    .Select(c => new TableColumnSqlGenerator(DbContext, c).GetSqlString())
                    .ToList(), 
                ",\n").GetAsString();
        }

        private void LoadColumnsCollection()
        {
            _tableColumns = DbContext.DBA_TAB_COLUMNS
                .Where(c => c.OWNER == _table.OWNER && c.TABLE_NAME == _table.TABLE_NAME)
                .OrderBy(c => c.COLUMN_ID)
                .ToList();
        }
    }
}
