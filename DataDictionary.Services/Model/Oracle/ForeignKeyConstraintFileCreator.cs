using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class ForeignKeyConstraintFileCreator : SqlFileCreator
    {
        public ForeignKeyConstraintFileCreator(
            DataDictionaryDbContext dbContext, 
            IFileSystemHelper fileSystemHelper, 
            IDependencyMatrix dependencyMatrix) 
            : base(dbContext, fileSystemHelper, dependencyMatrix)
        {
            ObjectType = "foreign key";
        }

        public override void CreateFiles(string schemaName)
        {
            var foreignKeys = DbContext.DBA_CONSTRAINTS
                .Where(c => (schemaName == null || c.OWNER == schemaName) && c.CONSTRAINT_TYPE == "R")
                .OrderBy(c => c.OWNER)
                .ThenBy(c => c.CONSTRAINT_NAME)
                .ToList();

            foreach (var foreignKey in foreignKeys)
            {
                var dbObject = DependencyMatrix.GetDbObject(ObjectType.ToUpperInvariant(), foreignKey.OWNER, foreignKey.CONSTRAINT_NAME);

                var fileName = foreignKey.CONSTRAINT_NAME.ToLowerInvariant() + SqlFileExtension;
                dbObject.FilePath = FileSystemHelper.GetOutputPath(SubFolderName, foreignKey.OWNER) + fileName;

                var sqlStringBuilder = new StringBuilder();

                sqlStringBuilder.AppendLine(new CommonHeaderFragmentCreator(DbContext, fileName, ObjectType, foreignKey.CONSTRAINT_NAME).GetSqlString());
                sqlStringBuilder.AppendLine(new DependenciesSqlGenerator(DbContext, foreignKey, DependencyMatrix).GetSqlString());
                sqlStringBuilder.AppendLine(new ForeignKeyConstraintSqlGenerator(DbContext, foreignKey).GetSqlString());
                sqlStringBuilder.AppendLine(new CommonFileFooterFragmentCreator(DbContext, fileName).GetSqlString());

                FileSystemHelper.SaveContentToFile(dbObject.FilePath, sqlStringBuilder.ToString());
            }
            
        }
    }
}
