using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class ProceduresFileContentCreator : SqlFileCreator
    {
        public ProceduresFileContentCreator(
            DataDictionaryDbContext dbContext,
            IFileSystemHelper fileSystemHelper,
            IDependencyMatrix dependencyMatrix)
            : base(dbContext, fileSystemHelper, dependencyMatrix)
        {
            ObjectType = "procedure";
        }

        public override void CreateFiles(string schemaName)
        {
            var objectTypeUpper = ObjectType.ToUpperInvariant();
            var procedures = DbContext.DBA_PROCEDURES
                .Where(
                    p => 
                        (schemaName == null || p.OWNER == schemaName) &&
                        p.OBJECT_TYPE == objectTypeUpper)
                .OrderBy(p => p.OWNER)
                .ThenBy(p => p.OBJECT_NAME)
                .ToList();

            foreach (var procedure in procedures)
            {
                var dbObject = DependencyMatrix.GetDbObject(objectTypeUpper, procedure.OWNER, procedure.OBJECT_NAME);
                var fileName = procedure.OBJECT_NAME.ToLowerInvariant() + SqlFileExtension;
                dbObject.FilePath = FileSystemHelper.GetOutputPath(SubFolderName, procedure.OWNER) + fileName;

                var sqlStringBuilder = new StringBuilder();

                sqlStringBuilder.AppendLine(new CommonHeaderFragmentCreator(DbContext, fileName,ObjectType,procedure.OBJECT_NAME).GetSqlString());
                sqlStringBuilder.AppendLine(new DependenciesSqlGenerator(DbContext, procedure, DependencyMatrix).GetSqlString());
                sqlStringBuilder.AppendLine(new ProcedureSqlGenerator(DbContext, procedure).GetSqlString());
                sqlStringBuilder.AppendLine(new CommonFileFooterFragmentCreator(DbContext, fileName).GetSqlString());

                FileSystemHelper.SaveContentToFile(dbObject.FilePath, sqlStringBuilder.ToString());
            }
        }
    }
}
