using System.Collections.Generic;

namespace DbScout.Contracts
{
    /// <summary>
    /// Provide configuration functionality. Classes implementing that interface should perform an case insensitive
    /// key comparison. 
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Returns the whole configuration parameter and value collection as IDictionary instance.
        /// </summary>
        /// <returns>IDictionary instance containing all configuration settings</returns>
        IDictionary<string, string> GetConfiguration();

        /// <summary>
        /// Returns the value of the specified configuration parameter.
        /// </summary>
        /// <param name="key">Key of the parameter</param>
        /// <param name="defaultValue">Default value, if the parameter doesn't exist.</param>
        /// <returns>Returns the value of the specified parameter or default value</returns>
        string GetConfigValue(string key, string defaultValue = null);

        /// <summary>
        /// Returns the value of the specified mandatory key. If not exist, an exception is thrown.
        /// </summary>
        /// <param name="key">Key of the parameter</param>
        /// <returns>Returns the value of the specified key.</returns>
        string GetMandatoryConfigValue(string key);

        /// <summary>
        /// Depending on the implementation, this method allows to set/update parameter values.
        /// </summary>
        /// <param name="key">Key of the parameter</param>
        /// <param name="value">New value of the parameter</param>
        void SetConfigValue(string key, string value);
    }
}