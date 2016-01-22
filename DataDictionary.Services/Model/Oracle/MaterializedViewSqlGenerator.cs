using System.Linq;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class MaterializedViewSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "MaterializedViewTemplate.txt";

        private readonly DBA_MVIEWS _mview;

        public MaterializedViewSqlGenerator(DataDictionaryDbContext dbContext, DBA_MVIEWS mview) 
            : base(dbContext)
        {
            _mview = mview;
        }

        public override string GetSqlString()
        {
            var columnsList = new CollectionToString(
                DbContext.DBA_TAB_COLUMNS
                    .Where(c => c.OWNER == _mview.OWNER && c.TABLE_NAME == _mview.MVIEW_NAME)
                    .OrderBy(c => c.COLUMN_ID)
                    .Select(v => v.COLUMN_NAME)
                    .ToList(),",\n\t").GetAsString().ToLowerInvariant();

            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[MVIEW_NAME]", _mview.MVIEW_NAME.ToLowerInvariant())
                .Replace("[COLUMNS_LIST]", "\n\t" + columnsList)
                .Replace("[QUERY]", _mview.QUERY)
                .Replace("[BUILD_MODE]", string.IsNullOrEmpty(_mview.BUILD_MODE) ? string.Empty : "build " + _mview.BUILD_MODE.ToLowerInvariant() + "\n")
                .Replace("[REFRESH_METHOD]", string.IsNullOrEmpty(_mview.REFRESH_METHOD) ? string.Empty : "refresh " + _mview.REFRESH_METHOD.ToLowerInvariant() + "\n")
                .Replace("[REFRESH_MODE]", string.IsNullOrEmpty(_mview.REFRESH_MODE) ? string.Empty : "on " + _mview.REFRESH_MODE.ToLowerInvariant() + "\n")
                .Replace("[QUERY_REWRITE]", (_mview.REWRITE_ENABLED.Equals("Y") ? "enable" : "disable") + " query rewrite\n");
        }
    }
}
