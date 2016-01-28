using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class MaterializedViewFileContentGenerator : SqlFileCreator
    {
        public MaterializedViewFileContentGenerator(
            DataDictionaryDbContext dbContext,
            IFileSystemHelper fileSystemHelper,
            IDependencyMatrix dependencyMatrix)
            : base(dbContext, fileSystemHelper, dependencyMatrix)
        {
            ObjectType = "materialized view";
        }

        public override void CreateFiles(string schemaName)
        {
            var mviews = DbContext.DBA_MVIEWS
                .Where(t => schemaName == null || t.OWNER == schemaName)
                .OrderBy(t => t.OWNER)
                .ThenBy(t => t.MVIEW_NAME)
                .ToList();

            foreach (var mview in mviews)
            {
                var dbObject = DependencyMatrix.GetDbObject(ObjectType.ToUpperInvariant(), mview.OWNER, mview.MVIEW_NAME);

                var fileName = mview.MVIEW_NAME.ToLowerInvariant() + SqlFileExtension;
                dbObject.FilePath = FileSystemHelper.GetOutputPath(SubFolderName, mview.OWNER) + fileName;
                
                var sqlStringBuilder = new StringBuilder();

                sqlStringBuilder.AppendLine(new CommonHeaderFragmentCreator(DbContext, fileName,ObjectType,mview.MVIEW_NAME).GetSqlString());
                sqlStringBuilder.AppendLine(new DependenciesSqlGenerator(DbContext, mview, DependencyMatrix).GetSqlString());
                sqlStringBuilder.AppendLine(new MaterializedViewSqlGenerator(DbContext, mview).GetSqlString());
                sqlStringBuilder.AppendLine(new TableCommentGenerator(DbContext, mview).GetSqlString());
                sqlStringBuilder.AppendLine(new ColumnCommentsGenerator(DbContext, mview).GetSqlString());
                sqlStringBuilder.AppendLine(new CommonFileFooterFragmentCreator(DbContext, fileName).GetSqlString());

                FileSystemHelper.SaveContentToFile(dbObject.FilePath, sqlStringBuilder.ToString());
            }
        }
    }
}
