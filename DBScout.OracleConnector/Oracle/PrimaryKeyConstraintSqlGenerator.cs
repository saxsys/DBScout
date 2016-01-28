using System.Linq;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class PrimaryKeyConstraintSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "PrimaryKeyDefinitionTemplate.txt";

        private readonly DBA_CONSTRAINTS _primaryKeyConstraint;

        public PrimaryKeyConstraintSqlGenerator(DataDictionaryDbContext dbContext,DBA_CONSTRAINTS primaryKeyConstraint) 
            : base(dbContext)
        {
            _primaryKeyConstraint = primaryKeyConstraint;
        }

        public override string GetSqlString()
        {
            var constraintColumnsString = new CollectionToString(
                DbContext.DBA_CONS_COLUMNS
                    .Where(
                        c =>
                            c.OWNER == _primaryKeyConstraint.OWNER &&
                            c.CONSTRAINT_NAME == _primaryKeyConstraint.CONSTRAINT_NAME)
                    .OrderBy(c => c.POSITION)
                    .Select(c => c.COLUMN_NAME)
                    .ToList()).GetAsString().ToLowerInvariant();

            var tablespaceName = DbContext.DBA_INDEXES
                .First(
                    i =>
                        i.OWNER == _primaryKeyConstraint.INDEX_OWNER &&
                        i.INDEX_NAME == _primaryKeyConstraint.INDEX_NAME)
                .TABLESPACE_NAME;

            if (string.IsNullOrEmpty(tablespaceName))
            {
                tablespaceName = GetDefaultTablespace(_primaryKeyConstraint.INDEX_OWNER);
            }

            return "\t" + (TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[CONSTRAINTNAME]", _primaryKeyConstraint.CONSTRAINT_NAME.ToLowerInvariant())
                .Replace("[CONSTRAINTCOLUMNS]", constraintColumnsString)
                .Replace("[TABLESPACE_NAME]", tablespaceName.ToLowerInvariant())
                .Replace("[CONSTRAINTSTATE]", _primaryKeyConstraint.STATUS.Equals("DISABLED") ? " disable" : string.Empty)
                .Replace("\t","\t\t"));
        }
    }
}
