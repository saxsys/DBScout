using System.Collections.Generic;
using System.Data.Common;

namespace DbScout.Contracts
{
    /// <summary>
    /// A database reader is used to obtain the database hierarchy structure depending on the implementation
    /// of this interface and the related configuration.
    /// </summary>
    public interface IDatabaseReader
    {
        /// <summary>
        /// Obtain the database structure from data dictionary. The returned object refers to the first level of 
        /// database object hierarchy (usually the database representation).
        /// </summary>
        /// <returns></returns>
        ICollection<IDatabaseObject> ReadDatabaseObjects();
    }
}
