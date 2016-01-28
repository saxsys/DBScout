using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class TriggerSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "TriggerTemplate.txt";

        private readonly DBA_TRIGGERS _triggerData;

        public TriggerSqlGenerator(DataDictionaryDbContext dbContext, DBA_TRIGGERS triggerData) 
            : base(dbContext)
        {
            _triggerData = triggerData;
        }

        public override string GetSqlString()
        {
            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[DESCRIPTION]", _triggerData.DESCRIPTION)
                .Replace("[TRIGGER_BODY]", _triggerData.TRIGGER_BODY)
                .Replace("[TRIGGER_NAME]", _triggerData.TRIGGER_NAME.ToLowerInvariant())
                .Replace("[ENABLE_DISABLE]", _triggerData.STATUS.Equals("DISABLED") ? "disable" : "enable");
        }
    }
}
