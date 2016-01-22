using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class DbLinkFileContentCreator : SqlFileCreator
    {
        public DbLinkFileContentCreator(
            DataDictionaryDbContext dbContext,
            IFileSystemHelper fileSystemHelper,
            IDependencyMatrix dependencyMatrix)
            : base(dbContext, fileSystemHelper, dependencyMatrix)
        {
            ObjectType = "database link";
        }

        public override void CreateFiles(string schemaName)
        {
            var dbLinks = DbContext.DBA_DB_LINKS
                .Where(p => (schemaName == null || p.OWNER == schemaName))
                .OrderBy(p => p.OWNER)
                .ThenBy(p => p.DB_LINK)
                .ToList();

            foreach (var dbLink in dbLinks)
            {
                var dbObject = DependencyMatrix.GetDbObject(ObjectType.ToUpperInvariant(), dbLink.OWNER, dbLink.DB_LINK);
                var fileName = dbLink.DB_LINK.ToLowerInvariant() + SqlFileExtension;
                dbObject.FilePath = FileSystemHelper.GetOutputPath(SubFolderName, dbLink.OWNER) + fileName;

                var sqlStringBuilder = new StringBuilder();

                sqlStringBuilder.AppendLine(new CommonHeaderFragmentCreator(DbContext, fileName,ObjectType,dbLink.DB_LINK).GetSqlString());
                sqlStringBuilder.AppendLine(new DbLinkSqlGenerator(DbContext, dbLink).GetSqlString());
                sqlStringBuilder.AppendLine(new CommonFileFooterFragmentCreator(DbContext, fileName).GetSqlString());

                FileSystemHelper.SaveContentToFile(dbObject.FilePath, sqlStringBuilder.ToString());
            }
        }
    }
}
