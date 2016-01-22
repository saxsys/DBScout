using System.Linq;
using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class ProcedureSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "ProcedureTemplate.txt";
        
        private readonly DBA_PROCEDURES _procedure;

        public ProcedureSqlGenerator(DataDictionaryDbContext dbContext, DBA_PROCEDURES procedure) 
            : base(dbContext)
        {
            _procedure = procedure;
        }

        public override string GetSqlString()
        {
            var procedureSpecString = new CollectionToString(null, string.Empty)
                .GetAsString(
                    DbContext.DBA_SOURCE
                        .Where(
                            s =>
                                s.OWNER == _procedure.OWNER &&
                                s.NAME == _procedure.OBJECT_NAME &&
                                s.TYPE == _procedure.OBJECT_TYPE)
                        .OrderBy(s => s.LINE)
                        .Select(s => s.TEXT)
                        .ToList());

            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[PROCEDURE_SPEC]", procedureSpecString);
        }
    }
}
