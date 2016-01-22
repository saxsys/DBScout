using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class TableSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "TableTemplate.txt";

        private readonly DBA_TABLES _table;

        public TableSqlGenerator(DataDictionaryDbContext dbContext, DBA_TABLES table) 
            : base(dbContext)
        {
            _table = table;
        }

        public override string GetSqlString()
        {
            var tablespaceName = _table.TABLESPACE_NAME ?? GetDefaultTablespace(_table.OWNER);

            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[OBJECTNAME]", _table.TABLE_NAME.ToLowerInvariant())
                .Replace("[TABLECOLUMNDEFINITION]", new TableColumnsSqlGenerator(DbContext, _table).GetSqlString())
                .Replace("[CONSTRAINTDEFINITION]", new ConstraintsSqlGenerator(DbContext, _table).GetSqlString())
                .Replace("[TABLESPACE_NAME]", tablespaceName.ToLowerInvariant());
        }
    }
}