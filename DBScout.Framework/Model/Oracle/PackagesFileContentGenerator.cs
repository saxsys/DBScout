using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class PackagesFileContentGenerator : SqlFileCreator
    {
        public PackagesFileContentGenerator(
            DataDictionaryDbContext dbContext,
            IFileSystemHelper fileSystemHelper,
            IDependencyMatrix dependencyMatrix)
            : base(dbContext, fileSystemHelper, dependencyMatrix)
        {
            ObjectType = "package";
        }

        public override void CreateFiles(string schemaName)
        {
            var objectTypeUpper = ObjectType.ToUpperInvariant();
            var packages = DbContext.DBA_PROCEDURES
                .Where(
                    p =>
                        (schemaName == null || p.OWNER == schemaName) &&
                        (p.OBJECT_TYPE == objectTypeUpper && p.SUBPROGRAM_ID == 0))
                .OrderBy(p => p.OBJECT_TYPE)
                .ThenBy(p => p.OWNER)
                .ThenBy(p => p.OBJECT_NAME)
                .ToList();

            foreach (var package in packages)
            {
                var dbObject = DependencyMatrix.GetDbObject(objectTypeUpper, package.OWNER, package.OBJECT_NAME);

                var fileName = package.OBJECT_NAME.ToLowerInvariant() + SqlFileExtension;
                dbObject.FilePath = FileSystemHelper.GetOutputPath(SubFolderName, package.OWNER) + fileName;
                
                var sqlStringBuilder = new StringBuilder();

                sqlStringBuilder.AppendLine(new CommonHeaderFragmentCreator(DbContext, fileName,ObjectType,package.OBJECT_NAME).GetSqlString());
                sqlStringBuilder.AppendLine(new DependenciesSqlGenerator(DbContext, package, DependencyMatrix).GetSqlString());
                sqlStringBuilder.AppendLine(new PackageSqlGenerator(DbContext, package).GetSqlString());
                sqlStringBuilder.AppendLine(new CommonFileFooterFragmentCreator(DbContext, fileName).GetSqlString());

                FileSystemHelper.SaveContentToFile(dbObject.FilePath, sqlStringBuilder.ToString());
            }
        }
    }
}
