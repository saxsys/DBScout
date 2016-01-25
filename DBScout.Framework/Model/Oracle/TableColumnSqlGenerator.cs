using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class TableColumnSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "TableColumnDefinitionTemplate.txt";

        private readonly DBA_TAB_COLUMNS _tableColumn;
        
        public TableColumnSqlGenerator(DataDictionaryDbContext dbContext, DBA_TAB_COLUMNS tableColumn) 
            : base(dbContext)
        {
            _tableColumn = tableColumn;
        }

        public override string GetSqlString()
        {
            var columnName = _tableColumn.COLUMN_NAME.PadRight(30).ToLowerInvariant();
            var dataType = GetColumnDataType().PadRight(20).ToLowerInvariant();
            var defaultValue = string.IsNullOrEmpty(_tableColumn.DATA_DEFAULT)
                ? string.Empty
                : "default " + _tableColumn.DATA_DEFAULT;
            var notNullConstraint = _tableColumn.NULLABLE == "N" ? "not null" : string.Empty;
            defaultValue = defaultValue.PadRight(15);

            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[COLUMNNAME]", columnName)
                .Replace("[DATATYPE]", " " + dataType)
                .Replace("[DEFAULTVALUE]", " " + defaultValue)
                .Replace("[NULLABLESPEC]", " " + notNullConstraint)
                .TrimEnd();
        }

        private string GetColumnDataType()
        {
            var sqlStringStringBuilder = new StringBuilder();
            sqlStringStringBuilder.Append(_tableColumn.DATA_TYPE);
            switch (_tableColumn.DATA_TYPE)
            {
                case "NVARCHAR2":
                case "NVARCHAR":
                case "VARCHAR2":
                case "VARCHAR":
                case "CHAR":
                    sqlStringStringBuilder.Append("(");
                    sqlStringStringBuilder.Append(_tableColumn.CHAR_LENGTH);
                    sqlStringStringBuilder.Append(_tableColumn.CHAR_USED == "B" ? " byte" : _tableColumn.CHAR_USED == "C" ? " char" : string.Empty);
                    sqlStringStringBuilder.Append(")");
                    break;
                case "RAW":
                    sqlStringStringBuilder.Append("(");
                    sqlStringStringBuilder.Append(_tableColumn.DATA_LENGTH);
                    sqlStringStringBuilder.Append(")");
                    break;
                case "FLOAT":
                case "NUMBER":
                    sqlStringStringBuilder.Append("(");
                    sqlStringStringBuilder.Append(_tableColumn.DATA_PRECISION);
                    if (_tableColumn.DATA_SCALE != null && _tableColumn.DATA_SCALE != 0)
                    {
                        sqlStringStringBuilder.Append(",");
                        sqlStringStringBuilder.Append(_tableColumn.DATA_SCALE);
                    }
                    sqlStringStringBuilder.Append(")");
                    break;
            }
            return sqlStringStringBuilder.ToString();
        }
    }
}
