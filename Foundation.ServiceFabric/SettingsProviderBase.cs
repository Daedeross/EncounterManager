namespace Foundation.ServiceFabric
{
    using Foundation.Utilities;
    using Microsoft.ServiceFabric.Data;

    public abstract class SettingsProviderBase : ISettingsProvider
    {
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

        protected abstract bool TryGetSetting(string config, string section, string parameter, out string value);
    }
}