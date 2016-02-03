using DBScout.Model;
using System.Collections.Specialized;

namespace DBScout.Connectors
{
    public interface IDatabaseConnector
    {
        /// <summary>
        /// Retrieve database information from data dictionary of connected database.
        /// Precondition is a valid database connection.
        /// </summary>
        /// <returns>Returns </returns>
        IDbObject GetDatabaseInformation();

        /// <summary>
        /// Initialization method
        /// </summary>
        /// <param name="parameters">Parameters collection</param>
        void Init(NameValueCollection parameters);
    }
}
