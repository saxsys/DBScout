using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class ForeignKeyConstraintSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "ForeignKeyDefinitionTemplate.txt";
        private readonly DBA_CONSTRAINTS _foreignKeyConstraint;
        private DBA_CONSTRAINTS _refConstraint;

        public ForeignKeyConstraintSqlGenerator(DataDictionaryDbContext dbContext, DBA_CONSTRAINTS foreignKeyConstraint) 
            : base(dbContext)
        {
            _foreignKeyConstraint = foreignKeyConstraint;
        }

        public override string GetSqlString()
        {
            LoadReferencedConstraint();

            var constraintColumns = LoadConstraintColumns(_foreignKeyConstraint);
            var refConstraintColumns = LoadConstraintColumns(_refConstraint);

            var collectionToStringTool = new CollectionToString();

            var constraintColumnsString = collectionToStringTool.GetAsString(
                constraintColumns
                    .Select(c => c.COLUMN_NAME.ToLowerInvariant())
                    .ToList());

            var refConstraintColumnsString = collectionToStringTool.GetAsString(
                refConstraintColumns
                    .Select(c => c.COLUMN_NAME.ToLowerInvariant())
                    .ToList());

            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[TABLENAME]",_foreignKeyConstraint.TABLE_NAME.ToLowerInvariant())
                .Replace("[CONSTRAINTNAME]",_foreignKeyConstraint.CONSTRAINT_NAME.ToLowerInvariant())
                .Replace("[CONSTRAINTCOLUMNS]",constraintColumnsString)
                .Replace("[REFTABLENAME]",_refConstraint.TABLE_NAME.ToLowerInvariant())
                .Replace("[REFCONSTRAINTCOLUMNS]",refConstraintColumnsString)
                .Replace("[CONSTRAINTSTATE]",_foreignKeyConstraint.STATUS.Equals("DISABLED") ? " disable" : string.Empty);
        }

        private void LoadReferencedConstraint()
        {
            _refConstraint = DbContext.DBA_CONSTRAINTS
                .First(
                    c =>
                        c.OWNER == _foreignKeyConstraint.R_OWNER &&
                        c.CONSTRAINT_NAME == _foreignKeyConstraint.R_CONSTRAINT_NAME);
        }

        private IEnumerable<DBA_CONS_COLUMNS> LoadConstraintColumns(DBA_CONSTRAINTS constraint)
        {
            return DbContext.DBA_CONS_COLUMNS
                .Where(c => c.OWNER == constraint.OWNER && c.CONSTRAINT_NAME == constraint.CONSTRAINT_NAME)
                .OrderBy(c => c.POSITION)
                .ToList();
        }
    }
}
