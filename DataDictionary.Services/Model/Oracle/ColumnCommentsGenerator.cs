using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class ColumnCommentsGenerator : SqlScriptCreator
    {
        private readonly DBA_TABLES _table;
        private readonly DBA_VIEWS _view;
        private readonly DBA_MVIEWS _mview;

        private ICollection<DBA_TAB_COLUMNS> _tabColumnCollection;

        public ColumnCommentsGenerator(DataDictionaryDbContext dbContext,DBA_TABLES table) 
            : base(dbContext)
        {
            _table = table;
        }

        public ColumnCommentsGenerator(DataDictionaryDbContext dbContext, DBA_VIEWS view)
            : base(dbContext)
        {
            _view = view;
        }

        public ColumnCommentsGenerator(DataDictionaryDbContext dbContext, DBA_MVIEWS mview)
            : base(dbContext)
        {
            _mview = mview;
        }

        public override string GetSqlString()
        {
            LoadTableColumnCollection();

            var sqlStringBuilder = new StringBuilder();

            sqlStringBuilder.AppendLine(HeaderString("Spaltenkommentare"));

            sqlStringBuilder.AppendLine(
                new CollectionToString(
                    _tabColumnCollection
                        .Select(c => new ColumnCommentGenerator(DbContext,c).GetSqlString())
                        .ToList(),string.Empty,true).GetAsString());

            return sqlStringBuilder.ToString();
        }

        private void LoadTableColumnCollection()
        {
            var ownerName = _table == null ? (_view == null ? (_mview != null ? _mview.OWNER : string.Empty) : _view.OWNER) : _table.OWNER;
            var tabName = _table == null ? (_view == null ? (_mview != null ? _mview.MVIEW_NAME : string.Empty) : _view.VIEW_NAME) : _table.TABLE_NAME;

            _tabColumnCollection = DbContext.DBA_TAB_COLUMNS
                .Where(c => c.OWNER == ownerName && c.TABLE_NAME == tabName)
                .OrderBy(c => c.COLUMN_ID)
                .ToList();
        }
    }
}
