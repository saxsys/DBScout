using System.Linq;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class ViewSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "ViewTemplate.txt";

        private readonly DBA_VIEWS _view;

        public ViewSqlGenerator(DataDictionaryDbContext dbContext, DBA_VIEWS view) 
            : base(dbContext)
        {
            _view = view;
        }

        public override string GetSqlString()
        {
            var viewColumnsString = new CollectionToString(
                DbContext.DBA_TAB_COLUMNS
                    .Where(c => c.OWNER == _view.OWNER && c.TABLE_NAME == _view.VIEW_NAME)
                    .OrderBy(c => c.COLUMN_ID)
                    .Select(v => v.COLUMN_NAME)
                    .ToList(),
                ",\n\t").GetAsString().ToLowerInvariant();

            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[VIEW_NAME]", _view.VIEW_NAME.ToLowerInvariant())
                .Replace("[VIEW_COLUMNS]", "\n\t" + viewColumnsString)
                .Replace("[VIEW_DEF]", _view.TEXT);
        }
    }
}
