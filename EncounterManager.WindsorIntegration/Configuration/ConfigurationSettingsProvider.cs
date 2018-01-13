namespace EncounterManager.Configuration
{
    using System;
    using Foundation;
    using Foundation.Utilities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.ServiceFabric.Data;

    /// <summary>
    /// Wraps a Microsoft Configuration instance with the <see cref="ISettingsProvider"/> interface
    /// </summary>
    public class ConfigurationSettingsProvider : ISettingsProvider
    {
        private readonly IConfiguration _configuration;

        public ConfigurationSettingsProvider(IConfiguration configuration)
        {
            Args.NotNull(configuration, nameof(configuration));
            _configuration = configuration;
        }

        public T Get<T>(SettingKey<T> key, T defaultValue)
        {
            string stringValue;
            return TryGetSetting(key.Configuration, key.Section, key.Parameter, out stringValue) ? TypeHelpers.ConvertValue<T>(stringValue) : defaultValue;
        }

        public ConditionalValue<T> TryGet<T>(SettingKey<T> key)
        {
            string stringValue;
            if (TryGetSetting(key.Configuration, key.Section, key.Parameter, out stringValue))
            {
                return new ConditionalValue<T>(true, TypeHelpers.ConvertValue<T>(stringValue));
            }

            return new ConditionalValue<T>();
        }

        private bool TryGetSetting(string config, string section, string parameter, out string value)
        {
            value = null;
            try
            {
                var configSection = _configuration.GetSection(section);
                if (configSection == null) return false;

                value = configSection[parameter];
                return !string.IsNullOrEmpty(value);
            }
            catch (Exception)
            {
                // ignored -- in the case of settings retrieval we want to ignore all failure possibilities and skip straight to return the default value
                return false;
            }
        }
    }
}