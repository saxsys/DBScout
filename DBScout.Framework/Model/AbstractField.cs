namespace DBScout.Model
{
    public abstract class AbstractField : AbstractDbObject
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AbstractField()
        {
            Comment = string.Empty;
        }

        /// <summary>
        /// Position of the field in table/view
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Comment of the field
        /// </summary>
        public string Comment { get; set; }
    }
}
