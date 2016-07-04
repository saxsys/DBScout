using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DbScout.Contracts;

namespace DbScout.Services
{
    public class AppSettingsConfiguration : IConfiguration
    {
        public IDictionary<string, string> GetConfiguration()
        {
            return ConfigurationManager.AppSettings.Keys
                .Cast<object>()
                .ToDictionary(key => key.ToString(), key => ConfigurationManager.AppSettings[key.ToString()]);
        }

        public string GetConfigValue(string key, string defaultValue = null)
        {
            return ConfigurationManager.AppSettings[key] ?? defaultValue;
        }

        public string GetMandatoryConfigValue(string key)
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
            {
                throw new Exception($"Cannot find mandatory configuration parameter {key} in app.config file!");
            }
            return ConfigurationManager.AppSettings[key];
        }

        public void SetConfigValue(string key, string value)
        {
            // nothing to do
        }
    }
}
