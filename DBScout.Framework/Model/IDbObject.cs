namespace DBScout.Model
{
    /// <summary>
    /// Interface defining a general database object. It is used as part of the 
    /// compositum design pattern.
    /// </summary>
    public interface IDbObject
    {
        /// <summary>
        /// Parent database object (e.g. parent of field definition is table/view)
        /// </summary>
        IDbObject Parent { get; set; }

        /// <summary>
        /// Accessor to dependency collection
        /// </summary>
        DbObjectCompositum DependsOn { get; }

        /// <summary>
        /// Owner of the database object
        /// </summary>
        string Owner { get; set; }

        /// <summary>
        /// Name of the database object
        /// </summary>
        string Name { get; set; }
    }
}
