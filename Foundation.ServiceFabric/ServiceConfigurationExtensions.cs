namespace Foundation.ServiceFabric
{
    using System;
    using System.Fabric;
    using Foundation.Utilities;

    public static class ServiceConfigurationExtensions
    {
        public static T BindConfiguration<T>(this ICodePackageActivationContext context, string sectionName)
            where T : class, new()
        {
            var instance = new T();
            BindConfiguration<T>(context, sectionName, instance);
            return instance;
        }

        public static void BindConfiguration<T>(this ICodePackageActivationContext context, string sectionName, T instance)
            where T : class
        {
            ConfigurationBinder.Bind(context.GetConfigurationPackageObject("Config").Settings.Sections[sectionName], instance);
        }

        public static T GetSetting<T>(this ICodePackageActivationContext context, string sectionName, string settingName, T defaultValue = default(T))
        {
            string stringValue;
            if (TryGetSetting(context, "Config", sectionName, settingName, out stringValue))
            {
                return TypeHelpers.ConvertValue<T>(stringValue);
            }

            return defaultValue;
        }

        public static T GetSetting<T>(this ICodePackageActivationContext context, SettingKey<T> key, T defaultValue = default(T))
        {
            string stringValue;
            if (TryGetSetting(context, key.Configuration, key.Section, key.Parameter, out stringValue))
            {
                return TypeHelpers.ConvertValue<T>(stringValue);
            }

            return defaultValue;
        }

        public static string GetSetting(this ICodePackageActivationContext context, string sectionName, string settingName, string defaultValue = null)
        {
            string stringValue;
            if (TryGetSetting(context, "Config", sectionName, settingName, out stringValue))
            {
                return stringValue;
            }

            return defaultValue;
        }

        public static bool TryGetSetting(this ICodePackageActivationContext context, string config, string section, string parameter, out string value)
        {
            value = null;
            try
            {
                var sections = context.GetConfigurationPackageObject(config)?.Settings.Sections;
                if (sections == null || !sections.Contains(section)) return false;

                var parameters = sections[section].Parameters;
                if (parameters == null || !parameters.Contains(parameter)) return false;

                value = parameters[parameter].Value;
                return true;
            }
            catch (Exception)
            {
                // ignored -- in the case of settings retrieval we want to ignore all failure possibilities and skip straight to return the default value
                return false;
            }
        }
    }
}
