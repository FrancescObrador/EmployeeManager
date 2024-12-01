using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

namespace HumanResourcesManager.Utilities
{
    public static class Config
    {
        private static readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

        private static Logger logger;

        static Config()
        {
            logger = new Logger("config");
            Load();
        }

        /// <summary>
        /// Initializes default configuration values. 
        /// </summary>
        public static void InitDefaults()
        {
            logger.LogInfo("Config - Loading default configuration");
            SetDefault("lang", "es");
        }

        /// <summary>
        /// Sets a default value for a given configuration key, if it does not already exist.
        /// </summary>
        private static void SetDefault(string key, string value)
        {
            if (!_settings.ContainsKey(key))
            {
                _settings[key] = value;
            }
        }

        /// <summary>
        /// Loads the configuration settings from the application settings (AppSettings) in the configuration file.
        /// </summary>
        public static void Load()
        {
            logger.LogInfo("Config - Loading configuration");
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                _settings[key] = ConfigurationManager.AppSettings[key] ?? string.Empty;
            }

            if (_settings.Count == 0)
            {
                InitDefaults();
            }
        }

        /// <summary>
        /// Gets the value of a specific configuration key.
        /// </summary>
        public static string Get(string key)
        {
            logger.LogInfo($"Config - Getting {key}'s value from configuration");
            return _settings.ContainsKey(key) ? _settings[key] : string.Empty;
        }

        /// <summary>
        /// Sets a specific configuration key to the provided value.
        /// </summary>
        public static void Set(string key, string value)
        {
            logger.LogInfo($"Config - Setting {value} to {key} from configuration");
            _settings[key] = value;
        }

        /// <summary>
        /// Saves the current configuration settings to the configuration file.
        /// </summary>
        public static void Save()
        {
            logger.LogInfo("Config - Saving configuration");
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            foreach (var setting in _settings)
            {
                if (settings[setting.Key] == null)
                {
                    settings.Add(setting.Key, setting.Value);
                }
                else
                {
                    settings[setting.Key].Value = setting.Value;
                }
            }

            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}
