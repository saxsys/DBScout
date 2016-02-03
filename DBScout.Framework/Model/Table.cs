namespace DBScout.Model
{
    public class Table : AbstractDbObject
    {
        private DbObjectCompositum _fields;
        private DbObjectCompositum _primaryKeys;
        private DbObjectCompositum _foreignKeys;
        private DbObjectCompositum _uniqueConstraints;
        private DbObjectCompositum _checkConstraints;
        private DbObjectCompositum _indexes;
        private DbObjectCompositum _triggers;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Table()
        {
            Comment = string.Empty;
        }

        /// <summary>
        /// Fields collection
        /// </summary>
        public DbObjectCompositum Fields { get { return _fields ?? (_fields = new DbObjectCompositum()); } }

        /// <summary>
        /// Primary keys collection
        /// </summary>
        public DbObjectCompositum PrimaryKeys { get { return _primaryKeys ?? (_primaryKeys = new DbObjectCompositum()); } }

        /// <summary>
        /// Foreign keys collection
        /// </summary>
        public DbObjectCompositum ForeignKeys { get { return _foreignKeys ?? (_foreignKeys = new DbObjectCompositum()); } }

        /// <summary>
        /// Unique constraints collection
        /// </summary>
        public DbObjectCompositum UniqueConstraints { get { return _uniqueConstraints ?? (_uniqueConstraints = new DbObjectCompositum()); } }

        /// <summary>
        /// Check constraints collection
        /// </summary>
        public DbObjectCompositum CheckConstraints { get { return _checkConstraints ?? (_checkConstraints = new DbObjectCompositum()); } }

        /// <summary>
        /// References related tablespace information
        /// </summary>
        public Tablespace Tablespace { get; set; }

        /// <summary>
        /// Indexes collection
        /// </summary>
        public DbObjectCompositum Indexes { get { return _indexes ?? (_indexes = new DbObjectCompositum()); } }

        /// <summary>
        /// Indexes collection
        /// </summary>
        public DbObjectCompositum Triggers { get { return _triggers ?? (_triggers = new DbObjectCompositum()); } }

        /// <summary>
        /// True if the table is partitioned, otherwise false
        /// </summary>
        public bool IsPartitioned { get; set; }

        /// <summary>
        /// Comment of the table
        /// </summary>
        public string Comment { get; set; }
    }
}
