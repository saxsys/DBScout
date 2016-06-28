using System.Collections.Generic;

namespace DbScout.Contracts
{
    /// <summary>
    /// Represents a common database object
    /// </summary>
    public interface IDatabaseObject
    {
        /// <summary>
        /// Type of the database object. It is used and referenced by database reader and renderer instances and to 
        /// define a namespace for the database object.
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// Name of the database object. Depending on the type of database object, the name must be unique in its 
        /// namespace.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Primary parent object. For instance, an index is defined on a table, therefore the table would be referenced as parent
        /// object. together with <see cref="ChildObjects"/>, it is used to build up a hierarchy of database objects. 
        /// </summary>
        IDatabaseObject ParentObject { get; set; }

        /// <summary>
        /// Collection of dependent database objects (e.g. fields defined on a table)
        /// </summary>
        ICollection<IDatabaseObject> ChildObjects { get; set; }

        /// <summary>
        /// Collection of database objects which are base dependencies of the current object. For instance, if a view is defined
        /// to use a table, the table would be referenced here.
        /// </summary>
        ICollection<IDatabaseObject> DependsOn { get; set; }

        /// <summary>
        /// Collection of properties of the current database object. Basically, the inner dictionary is a name-value-
        /// collection of a certain data dictionary table record. If the database object is described using more than one 
        /// data dictionary tables (e.g. a table is defined in Oracle Data Dictionary in USER_TABLES, USER_TAB_COMMENTS, 
        /// ...). For each data dictionary table exists a single entry in this dictionary.
        /// </summary>
        IDictionary<string, IDictionary<string,string>> Properties { get; set; }
    }
}
