namespace DBScout.Model
{
    /// <summary>
    /// Abstract base class for all database information objects. It is part of the
    /// implementation of the compositum pattern.
    /// </summary>
    public abstract class AbstractDbObject : IDbObject
    {
        /// <summary>
        /// Collection of database objects where the current objects depends on.
        /// </summary>
        private DbObjectCompositum _dependsOn;

        /// <summary>
        /// Default constructor, used to initialize the string members to emtpy string instead of null.
        /// </summary>
        public AbstractDbObject()
        {
            Owner = string.Empty;
            Name = string.Empty;
        }
        
        /// <summary>
        /// Parent database object (e.g. parent of field definition is table/view)
        /// </summary>
        public IDbObject Parent { get; set; }

        /// <summary>
        /// Accessor to dependency collection
        /// </summary>
        public DbObjectCompositum DependsOn { get { return _dependsOn ?? (_dependsOn = new DbObjectCompositum()); } }
        
        /// <summary>
        /// Owner of the database object
        /// </summary>
        public string Owner { get; set; }
        
        /// <summary>
        /// Name of the database object
        /// </summary>
        public string Name { get; set; }
    }
}
