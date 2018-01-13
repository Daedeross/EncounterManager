namespace Foundation
{
    using Microsoft.ServiceFabric.Data;

    /// <summary>
    /// A simple read-only interface designed to return keyed setting values from an underlying data source.
    /// Makes use of a simple structured key value <see cref="SettingKey{T}"/> that contains the configuration, section and setting names,
    /// and typed with the expected datatype of the setting value.
    /// </summary>
    /// <see cref="SettingKey{T}"/>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Gets the specified setting, or returns <paramref name="defaultValue"/> if no setting can be found in this provider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Value with type T</returns>
        T Get<T>(SettingKey<T> key, T defaultValue = default(T));

        /// <summary>
        /// Tries to get the specified setting, returning a <see cref="ConditionalValue{TValue}"/> with the result of the attempt.
        /// </summary>
        /// <typeparam name="T">value type</typeparam>
        /// <param name="key">The setting key.</param>
        /// <returns><see cref="ConditionalValue{TValue}"/> with value <typeparamref name="T"/></returns>
        ConditionalValue<T> TryGet<T>(SettingKey<T> key);
    }
}