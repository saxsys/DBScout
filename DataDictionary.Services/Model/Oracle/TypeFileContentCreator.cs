using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class TypeFileContentCreator : SqlFileCreator
    {
        public TypeFileContentCreator(
            DataDictionaryDbContext dbContext,
            IFileSystemHelper fileSystemHelper,
            IDependencyMatrix dependencyMatrix)
            : base(dbContext, fileSystemHelper,dependencyMatrix)
        {
            ObjectType = "type";
        }

        public override void CreateFiles(string schemaName)
        {
            var types = DbContext.DBA_TYPES
                .Where(p => (schemaName == null || p.OWNER == schemaName))
                .OrderBy(p => p.OWNER)
                .ThenBy(p => p.TYPE_NAME)
                .ToList();

            foreach (var type in types)
            {
                var dbObject = DependencyMatrix.GetDbObject(ObjectType.ToUpperInvariant(), type.OWNER, type.TYPE_NAME);
                var fileName = type.TYPE_NAME.ToLowerInvariant() + SqlFileExtension;
                dbObject.FilePath = FileSystemHelper.GetOutputPath(SubFolderName, type.OWNER) + fileName;

                var sqlStringBuilder = new StringBuilder();

                sqlStringBuilder.AppendLine(new CommonHeaderFragmentCreator(DbContext, fileName,ObjectType,type.TYPE_NAME).GetSqlString());
                sqlStringBuilder.AppendLine(new DependenciesSqlGenerator(DbContext, type,DependencyMatrix).GetSqlString());
                sqlStringBuilder.AppendLine(new TypeSqlGenerator(DbContext, type).GetSqlString());
                sqlStringBuilder.AppendLine(new CommonFileFooterFragmentCreator(DbContext, fileName).GetSqlString());

                FileSystemHelper.SaveContentToFile(dbObject.FilePath, sqlStringBuilder.ToString());
            }
        }
    }
}
