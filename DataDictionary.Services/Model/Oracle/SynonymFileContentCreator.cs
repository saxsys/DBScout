using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class SynonymFileContentCreator : SqlFileCreator
    {
        public SynonymFileContentCreator(
            DataDictionaryDbContext dbContext,
            IFileSystemHelper fileSystemHelper,
            IDependencyMatrix dependencyMatrix)
            : base(dbContext, fileSystemHelper, dependencyMatrix)
        {
            ObjectType = "synonym";
        }

        public override void CreateFiles(string schemaName)
        {
            var synonyms = DbContext.DBA_SYNONYMS
                .Where(p => schemaName == null || p.OWNER == schemaName)
                .OrderBy(p => p.OWNER)
                .ThenBy(p => p.SYNONYM_NAME)
                .ToList();

            foreach (var synonym in synonyms)
            {
                var dbObject = DependencyMatrix.GetDbObject(ObjectType.ToUpperInvariant(), synonym.OWNER, synonym.SYNONYM_NAME);
                var fileName = synonym.SYNONYM_NAME.ToLowerInvariant() + SqlFileExtension;
                dbObject.FilePath = FileSystemHelper.GetOutputPath(SubFolderName, synonym.OWNER) + fileName;
                
                var sqlStringBuilder = new StringBuilder();

                sqlStringBuilder.AppendLine(new CommonHeaderFragmentCreator(DbContext, fileName,ObjectType,synonym.SYNONYM_NAME).GetSqlString());
                sqlStringBuilder.AppendLine(new DependenciesSqlGenerator(DbContext, synonym, DependencyMatrix).GetSqlString());
                sqlStringBuilder.AppendLine(new SynonymSqlGenerator(DbContext, synonym).GetSqlString());
                sqlStringBuilder.AppendLine(new CommonFileFooterFragmentCreator(DbContext, fileName).GetSqlString());

                FileSystemHelper.SaveContentToFile(dbObject.FilePath, sqlStringBuilder.ToString());
            }
        }
    }
}
