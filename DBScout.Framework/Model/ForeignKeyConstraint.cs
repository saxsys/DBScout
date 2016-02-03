namespace DBScout.Model
{
    public class ForeignKeyConstraint : AbstractConstraint
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ForeignKeyConstraint()
        {
            DeleteRule = string.Empty;
        }

        /// <summary>
        /// Referenced costraint
        /// </summary>
        public AbstractConstraint ReferencedConstraint { get; set; }

        /// <summary>
        /// Cascading rule
        /// </summary>
        public string DeleteRule { get; set; }
    }
}
