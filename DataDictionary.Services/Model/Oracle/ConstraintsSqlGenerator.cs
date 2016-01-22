using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class ConstraintsSqlGenerator : SqlScriptCreator
    {
        private readonly DBA_TABLES _table;

        private ICollection<DBA_CONSTRAINTS> _primaryKeysCollection;
        private ICollection<DBA_CONSTRAINTS> _otherConstraintsCollection;

        public ConstraintsSqlGenerator(DataDictionaryDbContext dbContext, DBA_TABLES table) 
            : base(dbContext)
        {
            _table = table;
        }

        public override string GetSqlString()
        {
            LoadPrimaryKeysCollection();
            LoadOtherConstraintsCollection();

            const string separatorString = ",";
            const bool newLineBetweenCollectionItems = true;

            var collectionToStringTool = new CollectionToString(
                null, 
                separatorString, 
                newLineBetweenCollectionItems);

            var primaryKeyString = collectionToStringTool.GetAsString(
                _primaryKeysCollection
                    .Select(c => new PrimaryKeyConstraintSqlGenerator(DbContext, c).GetSqlString())
                    .ToList());

            var otherConstraintsString = collectionToStringTool.GetAsString(
                _otherConstraintsCollection
                    .Select(c => new OtherConstraintSqlGenerator(DbContext, c).GetSqlString())
                    .ToList());

            var sqlStringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(primaryKeyString))
            {
                sqlStringBuilder.AppendLine(separatorString);
                sqlStringBuilder.AppendLine();
                sqlStringBuilder.Append(primaryKeyString);
            }

            if (!string.IsNullOrEmpty(otherConstraintsString))
            {
                sqlStringBuilder.AppendLine(separatorString);
                sqlStringBuilder.AppendLine();
                sqlStringBuilder.Append(otherConstraintsString);
            }

            return sqlStringBuilder.ToString();
        }

        private void LoadPrimaryKeysCollection()
        {
            _primaryKeysCollection = DbContext.DBA_CONSTRAINTS
                .Where(c => c.OWNER == _table.OWNER && c.TABLE_NAME == _table.TABLE_NAME && c.CONSTRAINT_TYPE == "P")
                .OrderBy(c => c.CONSTRAINT_NAME)
                .ToList();
        }

        private void LoadOtherConstraintsCollection()
        {
            _otherConstraintsCollection = DbContext.DBA_CONSTRAINTS
                .Where(
                    c => 
                        c.OWNER == _table.OWNER &&
                        c.TABLE_NAME == _table.TABLE_NAME && 
                        c.CONSTRAINT_TYPE != "P" && 
                        c.CONSTRAINT_TYPE != "R" && 
                        c.GENERATED != "GENERATED NAME")
                .OrderBy(c => c.CONSTRAINT_NAME)
                .ToList();
        }
    }
}
