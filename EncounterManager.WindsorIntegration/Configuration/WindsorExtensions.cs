namespace EncounterManager.Configuration
{
    using Castle.MicroKernel;
    using Castle.Windsor;
    using Foundation;

    public static class WindsorExtensions
    {
        /// <summary>
        /// Gets the provided <see cref="SettingKey{T}"/> value from the <see cref="ISettingsProvider"/> registered to the provided
        /// <paramref name="container"/>.
        /// </summary>
        /// <typeparam name="T">Data type to convert setting to</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        /// <seealso cref="ISettingsProvider.Get{T}"/>
        public static T GetSetting<T>(this IWindsorContainer container, SettingKey<T> key)
        {
            var provider = container.Resolve<ISettingsProvider>();
            try
            {
                return provider.Get(key);
            }
            finally
            {
                container.Release(provider);
            }
        }

        /// <summary>
        /// Gets the provided <see cref="SettingKey{T}"/> value from the <see cref="ISettingsProvider"/> registered to the provided <see cref="kernel"/>
        /// </summary>
        /// <typeparam name="T">Data type to convert setting to</typeparam>
        /// <param name="kernel">The kernel.</param>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        /// <seealso cref="ISettingsProvider.Get{T}"/>
        public static T GetSetting<T>(this IKernel kernel, SettingKey<T> key)
        {
            var provider = kernel.Resolve<ISettingsProvider>();
            try
            {
                return provider.Get(key);
            }
            finally
            {
                kernel.ReleaseComponent(provider);
            }
        }
    }
}