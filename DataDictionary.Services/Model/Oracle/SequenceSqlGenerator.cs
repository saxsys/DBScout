using DataDictionary.Services.Tools;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model.Oracle
{
    public class SequenceSqlGenerator : SqlScriptCreator
    {
        private const string TemplateFileName = "SequenceTemplate.txt";

        private readonly DBA_SEQUENCES _sequence;

        public SequenceSqlGenerator(DataDictionaryDbContext dbContext, DBA_SEQUENCES sequence) 
            : base(dbContext)
        {
            _sequence = sequence;
        }

        public override string GetSqlString()
        {
            return TemplateStrings.GetTemplate(TemplateFileName)
                .Replace("[SEQUENCE_NAME]",_sequence.SEQUENCE_NAME.ToLowerInvariant())
                .Replace("[START_WITH]", _sequence.LAST_NUMBER != 1 ? "\n\tstart with " + _sequence.LAST_NUMBER + 1 : string.Empty)
                .Replace("[INCREMENT_BY]",_sequence.INCREMENT_BY != 1 ? "\n\tincrement by " + _sequence.INCREMENT_BY : string.Empty)
                .Replace("[MIN_VALUE]",_sequence.MIN_VALUE != 1 ? "\n\tminvalue " + _sequence.MIN_VALUE : string.Empty)
                .Replace("[MAX_VALUE]",_sequence.MIN_VALUE != 1 ? "\n\tmaxvalue " + _sequence.MAX_VALUE : string.Empty)
                .Replace("[CYCLE_FLAG]",_sequence.CYCLE_FLAG.Equals("Y") ? "\n\tcycle" : string.Empty)
                .Replace("[ORDER_FLAG]", _sequence.ORDER_FLAG.Equals("Y") ? "\n\torder" : string.Empty);
        }
    }
}
