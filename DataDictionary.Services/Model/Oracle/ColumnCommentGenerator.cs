using System.Linq;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class ColumnCommentGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "ColumnCommentTemplate.txt";

        private readonly DBA_TAB_COLUMNS _tabColumn;
        private DBA_COL_COMMENTS _columnComment;

        public ColumnCommentGenerator(DataDictionaryDbContext dbContext,DBA_TAB_COLUMNS tabColumn) 
            : base(dbContext)
        {
            _tabColumn = tabColumn;
        }

        public override string GetSqlString()
        {
            LoadColumnComment();

            if (_columnComment == null || string.IsNullOrEmpty(_columnComment.COMMENTS))
            {
                return "-- " + GetSqlStatement(
                        _tabColumn.TABLE_NAME,
                        _tabColumn.COLUMN_NAME,
                        "Hier_fehlenden_Kommentar_eingeben");
            }

            return GetSqlStatement(
                _tabColumn.TABLE_NAME,
                _tabColumn.COLUMN_NAME,
                _columnComment.COMMENTS);
        }

        private static string GetSqlStatement(string tableName, string columnName, string comment)
        {
            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[TABLE_NAME]", tableName.ToLowerInvariant())
                .Replace("[COLUMN]", columnName.ToLowerInvariant())
                .Replace("[COMMENT]", comment.Trim());
        }

        private void LoadColumnComment()
        {
            _columnComment = DbContext.DBA_COL_COMMENTS
                .FirstOrDefault(
                    c =>
                        c.OWNER == _tabColumn.OWNER &&
                        c.TABLE_NAME == _tabColumn.TABLE_NAME &&
                        c.COLUMN_NAME == _tabColumn.COLUMN_NAME);
        }
    }
}
