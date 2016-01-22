using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class DbmsSchedulerJobFileContentCreator : SqlFileCreator
    {
        public DbmsSchedulerJobFileContentCreator(
            DataDictionaryDbContext dbContext,
            IFileSystemHelper fileSystemHelper,
            IDependencyMatrix dependencyMatrix)
            : base(dbContext, fileSystemHelper, dependencyMatrix)
        {
            ObjectType = "dbms_scheduler job";
        }

        public override void CreateFiles(string schemaName)
        {
            var jobs = DbContext.DBA_SCHEDULER_JOBS
                .Where(p => (schemaName == null || p.OWNER == schemaName))
                .OrderBy(p => p.OWNER)
                .ThenBy(p => p.JOB_NAME)
                .ToList();

            foreach (var job in jobs)
            {
                var dbObject = DependencyMatrix.GetDbObject(ObjectType.ToUpperInvariant(), job.OWNER, job.JOB_NAME);
                var fileName = job.JOB_NAME.ToLowerInvariant() + SqlFileExtension;
                dbObject.FilePath = FileSystemHelper.GetOutputPath(SubFolderName, job.OWNER) + fileName;
                
                var sqlStringBuilder = new StringBuilder();

                sqlStringBuilder.AppendLine(new CommonHeaderFragmentCreator(DbContext, fileName,ObjectType,job.JOB_NAME).GetSqlString());
                sqlStringBuilder.AppendLine(new DbmsSchedulerJobSqlGenerator(DbContext, job).GetSqlString());
                sqlStringBuilder.AppendLine(new CommonFileFooterFragmentCreator(DbContext, fileName).GetSqlString());

                FileSystemHelper.SaveContentToFile(dbObject.FilePath, sqlStringBuilder.ToString());
            }
        }
    }
}
