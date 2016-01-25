using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class TableCommentGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "TableCommentTemplate.txt";
        
        private readonly DBA_TABLES _table;
        private readonly DBA_VIEWS _view;
        private readonly DBA_MVIEWS _mview;

        private DBA_TAB_COMMENTS _tabComments;
        private DBA_MVIEW_COMMENTS _mviewComments;

        public TableCommentGenerator(DataDictionaryDbContext dbContext, DBA_TABLES table) 
            : base(dbContext)
        {
            _table = table;
        }

        public TableCommentGenerator(DataDictionaryDbContext dbContext, DBA_VIEWS view)
            : base(dbContext)
        {
            _view = view;
        }

        public TableCommentGenerator(DataDictionaryDbContext dbContext, DBA_MVIEWS mview)
            : base(dbContext)
        {
            _mview = mview;
        }

        public override string GetSqlString()
        {
            LoadTabComments();

            var sqlStringBuilder = new StringBuilder();

            sqlStringBuilder.AppendLine(HeaderString("Kommentar"));

            var objectType = _table != null || _view != null
                ? "table"
                : (_mview != null
                    ? "materialized view"
                    : string.Empty);
            var objectName = (_tabComments != null
                ? _tabComments.TABLE_NAME
                : (_mviewComments != null ? _mviewComments.MVIEW_NAME : string.Empty));
            var comments = (_tabComments != null && !string.IsNullOrEmpty(_tabComments.COMMENTS)
                ? _tabComments.COMMENTS
                : (_mviewComments != null && !string.IsNullOrEmpty(_mviewComments.COMMENTS) ? _mviewComments.COMMENTS : string.Empty)).Trim();

            if (string.IsNullOrEmpty(comments))
            {
                return sqlStringBuilder
                    .Append("-- ")
                    .AppendLine(GetSqlStatement(objectName, objectType, "Hier_fehlenden_Kommentar_eingeben"))
                    .ToString();
            }

            return sqlStringBuilder.AppendLine(GetSqlStatement(objectName, objectType, comments)).ToString();
        }

        private static string GetSqlStatement(
            string objectName, 
            string objectType,
            string comments)
        {
            return TemplateStrings.GetTemplate(TemplateFileName)
                    .Replace("[OBJECTNAME]", objectName.ToLowerInvariant())
                    .Replace("[OBJECTTYPE]", objectType.ToLowerInvariant())
                    .Replace("[COMMENT]", comments.Trim());
        }

        private void LoadTabComments()
        {
            if (_table != null)
            {
                _tabComments = DbContext.DBA_TAB_COMMENTS
                    .FirstOrDefault(c => c.OWNER == _table.OWNER && c.TABLE_NAME == _table.TABLE_NAME);
            }

            if (_view != null)
            {
                _tabComments = DbContext.DBA_TAB_COMMENTS
                    .FirstOrDefault(c => c.OWNER == _view.OWNER && c.TABLE_NAME == _view.VIEW_NAME);
            }

            if (_mview != null)
            {
                _mviewComments = DbContext.DBA_MVIEW_COMMENTS
                    .FirstOrDefault(c => c.OWNER == _mview.OWNER && c.MVIEW_NAME == _mview.MVIEW_NAME);
            }
        }
    }
}
