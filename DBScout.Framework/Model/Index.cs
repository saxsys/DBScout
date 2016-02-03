namespace DBScout.Model
{
    public class Index : AbstractDbObject
    {
        /// <summary>
        /// Collection of index fields
        /// </summary>
        private DbObjectCompositum _fields;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Index()
        {
            IndexType = string.Empty;
            Status = string.Empty;
        }

        /// <summary>
        /// Defines the index type
        /// </summary>
        public string IndexType { get; set; }

        /// <summary>
        /// True if the index is unique, otherwise false
        /// </summary>
        public bool IsUnique { get; set; }

        /// <summary>
        /// True if index compression is enabled, otherwise false
        /// </summary>
        public bool IsCompressionEnabled { get; set; }

        /// <summary>
        /// References related tablespace information
        /// </summary>
        public Tablespace Tablespace { get; set; }

        /// <summary>
        /// Defines the status of the index
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// True if the index is partitioned, otherwise false
        /// </summary>
        public bool IsPartitioned { get; set; }

        /// <summary>
        /// Index fields collection including the sort direction
        /// </summary>
        public DbObjectCompositum Fields { get { return _fields ?? (_fields = new DbObjectCompositum()); } }
    }
}
