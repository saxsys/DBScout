using DBScout.Model;
using System.Collections.Specialized;

namespace DBScout.Connectors
{
    public interface IDbInfoProcessor
    {
        /// <summary>
        /// Defines implementation of output creation
        /// </summary>
        /// <param name="dbObject">Database object used to create related output</param>
        void Process(IDbObject dbObject);

        /// <summary>
        /// Initialization method
        /// </summary>
        /// <param name="parameters">Parameters collection</param>
        void Init(NameValueCollection parameters);
    }
}
