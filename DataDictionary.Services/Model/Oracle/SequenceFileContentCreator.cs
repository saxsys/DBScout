using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class SequenceFileContentCreator : SqlFileCreator
    {
        public SequenceFileContentCreator(
            DataDictionaryDbContext dbContext,
            IFileSystemHelper fileSystemHelper,
            IDependencyMatrix dependencyMatrix)
            : base(dbContext, fileSystemHelper, dependencyMatrix)
        {
            ObjectType = "sequence";
        }

        public override void CreateFiles(string schemaName)
        {
            var sequences = DbContext.DBA_SEQUENCES
                .Where(p => (schemaName == null || p.SEQUENCE_OWNER == schemaName))
                .OrderBy(p => p.SEQUENCE_OWNER)
                .ThenBy(p => p.SEQUENCE_NAME)
                .ToList();

            foreach (var seq in sequences)
            {
                var dbObject = DependencyMatrix.GetDbObject(ObjectType.ToUpperInvariant(), seq.SEQUENCE_OWNER, seq.SEQUENCE_NAME);
                var fileName = seq.SEQUENCE_NAME.ToLowerInvariant() + SqlFileExtension;
                dbObject.FilePath = FileSystemHelper.GetOutputPath(SubFolderName, seq.SEQUENCE_OWNER) + fileName;
                
                var sqlStringBuilder = new StringBuilder();

                sqlStringBuilder.AppendLine(new CommonHeaderFragmentCreator(DbContext, fileName,ObjectType,seq.SEQUENCE_NAME).GetSqlString());
                sqlStringBuilder.AppendLine(new SequenceSqlGenerator(DbContext, seq).GetSqlString());
                sqlStringBuilder.AppendLine(new CommonFileFooterFragmentCreator(DbContext, fileName).GetSqlString());

                FileSystemHelper.SaveContentToFile(dbObject.FilePath, sqlStringBuilder.ToString());
            }
        }
    }
}
