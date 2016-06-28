using System;
using System.Collections.Generic;
using DbScout.Contracts;

namespace DbScout.Services
{
    public class CommandLineConfiguration : IConfiguration
    {
        private const string ParamSeparators = "=:";

        public ICollection<string> CmdLineArgs { get; set; }

        private IDictionary<string, string> _paramDictionary;

        public IDictionary<string, string> GetConfiguration()
        {
            if (null != _paramDictionary)
            {
                return _paramDictionary;
            }
            _paramDictionary = new Dictionary<string, string>();

            if (CmdLineArgs == null)
            {
                throw new Exception("Configuration error: Invalid command line instance specified!");
            }

            foreach (var param in CmdLineArgs)
            {
                var keyValue = param.Split(ParamSeparators.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (2 != keyValue.Length)
                {
                    throw new Exception(
                        "Invalid command line format, parameters must be provided in format \"key=value\"!");
                }
                _paramDictionary.Add(keyValue[0].ToLowerInvariant(), keyValue[1]);
            }

            return _paramDictionary;
        }

        public string GetConfigValue(string key, string defaultValue)
        {
            return GetConfiguration()
                .ContainsKey(key.ToLowerInvariant()) 
                    ? GetConfiguration()[key.ToLowerInvariant()] 
                    : defaultValue;
        }

        public string GetMandatoryConfigValue(string key)
        {
            if (!GetConfiguration().ContainsKey(key.ToLowerInvariant()))
            {
                throw new Exception($"Configuration error: Missed mandatory parameter {key} in command line!");
            }

            return GetConfiguration()[key.ToLowerInvariant()];
        }

        public void SetConfigValue(string key, string value)
        {
            // SetConfigValue is not applicable for command line configuration
        }
    }
}
