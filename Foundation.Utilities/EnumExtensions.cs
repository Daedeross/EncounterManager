namespace Foundation.Utilities
{
    using System;

    public static class EnumExtensions
    {
        public static TEnum ParseEnum<TEnum>(this string value, bool ignoreCase = true)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof(TEnum).IsEnum) { throw new ArgumentException("TEnum must be an enumerated type"); }
            TEnum result;
            if (string.IsNullOrEmpty(value) || !Enum.TryParse(value, ignoreCase, out result))
            {
                throw new InvalidOperationException($"Value '{value}' could not be found on enum {typeof(TEnum).Name}");
            }
            return result;
        }
    }
}