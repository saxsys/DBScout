using System.Linq;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class PackageSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "PackageTemplate.txt";

        private readonly DBA_PROCEDURES _package;

        public PackageSqlGenerator(DataDictionaryDbContext dbContext, DBA_PROCEDURES package) 
            : base(dbContext)
        {
            _package = package;
        }

        public override string GetSqlString()
        {
            var collectionToStringTool = new CollectionToString(null, string.Empty);

            var packageHeaderString = collectionToStringTool.GetAsString(
                DbContext.DBA_SOURCE
                    .Where(
                        s =>
                            s.OWNER == _package.OWNER &&
                            s.NAME == _package.OBJECT_NAME &&
                            s.TYPE == _package.OBJECT_TYPE)
                    .OrderBy(s => s.LINE)
                    .Select(s => s.TEXT)
                    .ToList());

            var packageBodyString = collectionToStringTool.GetAsString(
                DbContext.DBA_SOURCE
                    .Where(
                        s =>
                            s.TYPE == "PACKAGE BODY" &&
                            s.OWNER == _package.OWNER &&
                            s.NAME == _package.OBJECT_NAME)
                    .OrderBy(s => s.LINE)
                    .Select(s => s.TEXT)
                    .ToList());

            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[PACKAGE_HEADER]", packageHeaderString)
                .Replace("[PACKAGE_BODY]", packageBodyString);
        }
    }
}
