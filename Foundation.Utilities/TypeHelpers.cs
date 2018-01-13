namespace Foundation.Utilities
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public static class TypeHelpers
    {
        public static T ConvertValue<T>(string value)
        {
            return (T) ConvertValue(typeof(T), value);
        }

        public static object ConvertValue(Type type, string value)
        {
            var convertType = type;
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                convertType = Nullable.GetUnderlyingType(type);
            }

            object result;
            try
            {
                result = TypeDescriptor.GetConverter(convertType).ConvertFromInvariantString(value);
            }
            catch (Exception innerException)
            {
                throw new InvalidOperationException($"Could not convert value '{value}' to type {type}", innerException);
            }
            return result;
        }
    }
}
