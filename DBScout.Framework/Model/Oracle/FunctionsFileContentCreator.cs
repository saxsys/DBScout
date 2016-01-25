using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class FunctionsFileContentCreator : SqlFileCreator
    {
        public FunctionsFileContentCreator(
            DataDictionaryDbContext dbContext,
            IFileSystemHelper fileSystemHelper,
            IDependencyMatrix dependencyMatrix)
            : base(dbContext, fileSystemHelper, dependencyMatrix)
        {
            ObjectType = "function";
        }

        public override void CreateFiles(string schemaName)
        {
            var objectTypeUpper = ObjectType.ToUpperInvariant();
            var functions = DbContext.DBA_PROCEDURES
                .Where(
                    p => 
                        (schemaName == null || p.OWNER == schemaName) &&
                        p.OBJECT_TYPE == objectTypeUpper)
                .OrderBy(p => p.OWNER)
                .ThenBy(p => p.OBJECT_NAME)
                .ToList();

            foreach (var function in functions)
            {
                var dbObject = DependencyMatrix.GetDbObject(objectTypeUpper, function.OWNER, function.OBJECT_NAME);
                var fileName = function.OBJECT_NAME.ToLowerInvariant() + SqlFileExtension;
                dbObject.FilePath = FileSystemHelper.GetOutputPath(SubFolderName, function.OWNER) + fileName;
                
                var sqlStringBuilder = new StringBuilder();

                sqlStringBuilder.AppendLine(new CommonHeaderFragmentCreator(DbContext, fileName,ObjectType,function.OBJECT_NAME).GetSqlString());
                sqlStringBuilder.AppendLine(new DependenciesSqlGenerator(DbContext, function, DependencyMatrix).GetSqlString());
                sqlStringBuilder.AppendLine(new FunctionsSqlGenerator(DbContext, function).GetSqlString());
                sqlStringBuilder.AppendLine(new CommonFileFooterFragmentCreator(DbContext, fileName).GetSqlString());

                FileSystemHelper.SaveContentToFile(dbObject.FilePath, sqlStringBuilder.ToString());
            }
        }
    }
}
