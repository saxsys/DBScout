namespace DBScout.Model
{
    public class Database : AbstractDbObject
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Database()
        {
            VersionInformation = string.Empty;
        }

        /// <summary>
        /// Collection of database schemas or users
        /// </summary>
        private DbObjectCompositum _databaseSchemas;

        /// <summary>
        /// Collection of roles
        /// </summary>
        private DbObjectCompositum _roles;

        /// <summary>
        /// Collection of tablespaces
        /// </summary>
        private DbObjectCompositum _tablespaces;

        /// <summary>
        /// Accessor to database schema collection
        /// </summary>
        public DbObjectCompositum DatabaseSchemas { get { return _databaseSchemas ?? (_databaseSchemas = new DbObjectCompositum()); } }

        /// <summary>
        /// Accessor to roles collection
        /// </summary>
        public DbObjectCompositum Roles { get { return _roles ?? (_roles = new DbObjectCompositum()); } }

        /// <summary>
        /// Accessor to tablespaces collection
        /// </summary>
        public DbObjectCompositum Tablespaces { get { return _tablespaces ?? (_tablespaces = new DbObjectCompositum()); } }

        /// <summary>
        /// Version information of the database
        /// </summary>
        public string VersionInformation { get; set; }
    }
}
