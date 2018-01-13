namespace Foundation.ServiceFabric
{
    using System.Collections.Generic;
    using System.Fabric.Description;
    using System.Linq;
    using System.Reflection;
    using Foundation.Utilities;

    public static class ConfigurationBinder
    {
        public static T Bind<T>(ConfigurationSection config, T instance)
        {
            var configParams = config.Parameters;

            foreach (var property in GetAllProperties(typeof(T).GetTypeInfo()).Where(p => configParams.Contains(p.Name)))
            {
                var value = configParams[property.Name].Value;
                property.SetValue(
                    instance,
                    property.PropertyType.IsAssignableFrom(typeof(string))
                        ? value
                        : TypeHelpers.ConvertValue(property.PropertyType, value));
            }

            return instance;
        }

        private static IEnumerable<PropertyInfo> GetAllProperties(TypeInfo type)
        {
            return ExplodeType(type).SelectMany(t => t.DeclaredProperties).Where(p => p.CanRead && p.CanWrite);
        }

        private static IEnumerable<TypeInfo> ExplodeType(TypeInfo type)
        {
            var stopType = typeof(object).GetTypeInfo();
            while (type != stopType)
            {
                yield return type;
                type = type.BaseType.GetTypeInfo();
            }
        }
    }
}
