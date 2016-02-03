namespace DBScout.Model
{
    public abstract class AbstractConstraint : AbstractDbObject
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AbstractConstraint()
        {
            Status = string.Empty;
            Generated = string.Empty;
        }

        /// <summary>
        /// Collection of constraint fields
        /// </summary>
        public DbObjectCompositum Fields { get; set; }
        
        /// <summary>
        /// Defines the status of the constraint (enabled, disabled)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Defines the generator of the constraint
        /// </summary>
        public string Generated { get; set; }
    }
}
