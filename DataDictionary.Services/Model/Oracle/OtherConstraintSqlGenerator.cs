using System.Linq;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class OtherConstraintSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "ConstraintDefinitionTemplate.txt";
        
        private readonly DBA_CONSTRAINTS _constraint;
        
        public OtherConstraintSqlGenerator(DataDictionaryDbContext dbContext,DBA_CONSTRAINTS constraint) 
            : base(dbContext)
        {
            _constraint = constraint;
        }

        public override string GetSqlString()
        {
            var constraintColumnsList = new CollectionToString(
                DbContext.DBA_CONS_COLUMNS
                    .Where(c => c.OWNER == _constraint.OWNER && c.CONSTRAINT_NAME == _constraint.CONSTRAINT_NAME)
                    .OrderBy(c => c.POSITION)
                    .Select(c => c.COLUMN_NAME)
                    .ToList(),
                ",\n\t\t").GetAsString().ToLowerInvariant();

            return "\t" + (TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[CONSTRAINTNAME]", _constraint.CONSTRAINT_NAME.ToLowerInvariant())
                .Replace("[CONSTRAINTTYPE]", _constraint.CONSTRAINT_TYPE.Equals("C")
                    ? "\n\tcheck"
                    : _constraint.CONSTRAINT_TYPE.Equals("U")
                        ? " unique"
                        : string.Empty)
                .Replace("[CONSTRAINTCOLUMNS]", _constraint.CONSTRAINT_TYPE.Equals("C")
                    ? _constraint.SEARCH_CONDITION.ToLowerInvariant().Trim()
                    : constraintColumnsList)
                    .Replace("[CONSTRAINTSTATE]", _constraint.STATUS.Equals("DISABLED") ? " disable" : string.Empty)
                .Replace("\t","\t\t"));
        }
    }
}
