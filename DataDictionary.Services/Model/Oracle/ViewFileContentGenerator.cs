using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class ViewFileContentGenerator : SqlFileCreator
    {
        public ViewFileContentGenerator(
            DataDictionaryDbContext dbContext,
            IFileSystemHelper fileSystemHelper,
            IDependencyMatrix dependencyMatrix)
            : base(dbContext, fileSystemHelper, dependencyMatrix)
        {
            ObjectType = "view";
        }

        public override void CreateFiles(string schemaName)
        {
            var views = DbContext.DBA_VIEWS
                .Where(t => schemaName == null || t.OWNER == schemaName)
                .OrderBy(t => t.OWNER)
                .ThenBy(t => t.VIEW_NAME)
                .ToList();

            foreach (var view in views)
            {
                var dbObject = DependencyMatrix.GetDbObject(ObjectType.ToUpperInvariant(), view.OWNER, view.VIEW_NAME);

                var fileName = view.VIEW_NAME.ToLowerInvariant() + SqlFileExtension;
                dbObject.FilePath = FileSystemHelper.GetOutputPath(SubFolderName, view.OWNER) + fileName;

                var sqlStringBuilder = new StringBuilder();

                sqlStringBuilder.AppendLine(new CommonHeaderFragmentCreator(DbContext, fileName, ObjectType, view.VIEW_NAME).GetSqlString());
                sqlStringBuilder.AppendLine(new DependenciesSqlGenerator(DbContext, view, DependencyMatrix).GetSqlString());
                sqlStringBuilder.AppendLine(new ViewSqlGenerator(DbContext, view).GetSqlString());
                sqlStringBuilder.AppendLine(new TableCommentGenerator(DbContext, view).GetSqlString());
                sqlStringBuilder.AppendLine(new ColumnCommentsGenerator(DbContext, view).GetSqlString());
                sqlStringBuilder.AppendLine(new CommonFileFooterFragmentCreator(DbContext, fileName).GetSqlString());

                FileSystemHelper.SaveContentToFile(dbObject.FilePath, sqlStringBuilder.ToString());
            }
        }
    }
}
