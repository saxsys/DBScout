using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class DbmsSchedulerJobSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "DbmsSchedulerJobTemplate.txt";

        private readonly DBA_SCHEDULER_JOBS _jobDef;

        public DbmsSchedulerJobSqlGenerator(DataDictionaryDbContext dbContext, DBA_SCHEDULER_JOBS jobDef) 
            : base(dbContext)
        {
            _jobDef = jobDef;
        }

        public override string GetSqlString()
        {
            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[JOB_NAME]", _jobDef.JOB_NAME.ToLowerInvariant())
                .Replace("[JOB_TYPE]", _jobDef.JOB_TYPE)
                .Replace("[JOB_ACTION]", _jobDef.JOB_ACTION)
                .Replace("[START_DATE]", _jobDef.START_DATE.HasValue
                    ? "to_date('" + _jobDef.START_DATE.Value.ToString("dd.MM.yyyy HH:mm:ss") + "','dd.mm.yyyy hh24:mi:ss')"
                    : "null")
                .Replace("[REPEAT_INTERVAL]", string.IsNullOrEmpty(_jobDef.REPEAT_INTERVAL)
                    ? "null"
                    : "'" + _jobDef.REPEAT_INTERVAL.Trim() + "'")
                .Replace("[END_DATE]", _jobDef.END_DATE.HasValue
                    ? "to_date('" + _jobDef.END_DATE.Value.ToString("dd.MM.yyyy HH:mm:ss") + "','dd.mm.yyyy hh24:mi:ss')"
                    : "null" )
                .Replace("[JOB_CLASS]", _jobDef.JOB_CLASS.Trim())
                .Replace("[ENABLED]", _jobDef.ENABLED.ToLowerInvariant())
                .Replace("[AUTO_DROP]", _jobDef.AUTO_DROP.ToLowerInvariant())
                .Replace("[COMMENTS]", string.IsNullOrEmpty(_jobDef.COMMENTS)
                    ? "null" 
                    : "'" + _jobDef.COMMENTS + "'");
        }
    }
}
