using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class TableFileContentGenerator : SqlFileCreator
    {
        public TableFileContentGenerator(
            DataDictionaryDbContext dbContext,
            IFileSystemHelper fileSystemHelper,
            IDependencyMatrix dependencyMatrix) 
            : base(dbContext,fileSystemHelper,dependencyMatrix)
        {
            ObjectType = "table";
        }

        public override void CreateFiles(string schemaName)
        {
            var tables = DbContext.DBA_TABLES
                .Where(t => schemaName == null || t.OWNER == schemaName)
                .OrderBy(t => t.OWNER)
                .ThenBy(t => t.TABLE_NAME)
                .ToList();

            foreach (var table in tables)
            {
                var dbObject = DependencyMatrix.GetDbObject(ObjectType.ToUpperInvariant(), table.OWNER, table.TABLE_NAME);

                var fileName = table.TABLE_NAME.ToLowerInvariant() + SqlFileExtension;
                dbObject.FilePath = FileSystemHelper.GetOutputPath(SubFolderName, table.OWNER) + fileName;

                var sqlStringBuilder = new StringBuilder();

                sqlStringBuilder.AppendLine(new CommonHeaderFragmentCreator(DbContext, fileName, ObjectType, table.TABLE_NAME).GetSqlString());
                sqlStringBuilder.AppendLine(new TableSqlGenerator(DbContext, table).GetSqlString());
                sqlStringBuilder.Append(new IndexesSqlGenerator(DbContext, table).GetSqlString());
                sqlStringBuilder.AppendLine(new TableCommentGenerator(DbContext, table).GetSqlString());
                sqlStringBuilder.AppendLine(new ColumnCommentsGenerator(DbContext, table).GetSqlString());
                sqlStringBuilder.AppendLine(new CommonFileFooterFragmentCreator(DbContext, fileName).GetSqlString());

                FileSystemHelper.SaveContentToFile(dbObject.FilePath, sqlStringBuilder.ToString());
            }
        }
    }
}
