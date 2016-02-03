namespace DBScout.Model
{
    public class CheckConstraint : AbstractConstraint
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CheckConstraint()
        {
            RuleDefinition = string.Empty;
        }

        /// <summary>
        /// Definition of check rule
        /// </summary>
        public string RuleDefinition { get; set; }
    }
}
