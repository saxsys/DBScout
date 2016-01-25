using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class TriggerFileContentCreator : SqlFileCreator
    {
        public TriggerFileContentCreator(
            DataDictionaryDbContext dbContext,
            IFileSystemHelper fileSystemHelper,
            IDependencyMatrix dependencyMatrix)
            : base(dbContext, fileSystemHelper, dependencyMatrix)
        {
            ObjectType = "trigger";
        }

        public override void CreateFiles(string schemaName)
        {
            var objectTypeUpper = ObjectType.ToUpperInvariant();
            var triggers = DbContext.DBA_PROCEDURES
                .Where(
                    p => 
                        (schemaName == null || p.OWNER == schemaName) &&
                        p.OBJECT_TYPE == objectTypeUpper)
                .OrderBy(p => p.OWNER)
                .ThenBy(p => p.OBJECT_NAME)
                .ToList();

            foreach (var trigger in triggers)
            {
                var triggerData = DbContext.DBA_TRIGGERS
                    .FirstOrDefault(t => t.OWNER == trigger.OWNER && t.TRIGGER_NAME == trigger.OBJECT_NAME);

                if (triggerData == null)
                {
                    continue;
                }

                var dbObject = DependencyMatrix.GetDbObject(objectTypeUpper, trigger.OWNER, trigger.OBJECT_NAME);
                var fileName = trigger.OBJECT_NAME.ToLowerInvariant() + SqlFileExtension;
                dbObject.FilePath = FileSystemHelper.GetOutputPath(SubFolderName, trigger.OWNER) + fileName;

                var sqlStringBuilder = new StringBuilder();

                sqlStringBuilder.AppendLine(new CommonHeaderFragmentCreator(DbContext, fileName,ObjectType,trigger.OBJECT_NAME).GetSqlString());
                sqlStringBuilder.AppendLine(new DependenciesSqlGenerator(DbContext, trigger, DependencyMatrix).GetSqlString());
                sqlStringBuilder.AppendLine(new TriggerSqlGenerator(DbContext, triggerData).GetSqlString());
                sqlStringBuilder.AppendLine(new CommonFileFooterFragmentCreator(DbContext, fileName).GetSqlString());

                FileSystemHelper.SaveContentToFile(dbObject.FilePath, sqlStringBuilder.ToString());
            }
        }
    }
}
