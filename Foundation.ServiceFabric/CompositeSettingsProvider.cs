namespace Foundation.ServiceFabric
{
    using System.Linq;
    using Foundation.Utilities;
    using Microsoft.ServiceFabric.Data;

    public class CompositeSettingsProvider : ISettingsProvider
    {
        private readonly ISettingsProvider[] _providers;

        public CompositeSettingsProvider(ISettingsProvider[] providers)
        {
            Args.NotNull(providers, nameof(providers));
            _providers = providers;
        }

        public T Get<T>(SettingKey<T> key, T defaultValue = default(T))
        {
            foreach (var provider in _providers.Reverse())
            {
                var result = provider.TryGet(key);
                if (result.HasValue)
                {
                    return result.Value;
                }
            }

            return defaultValue;
        }

        public ConditionalValue<T> TryGet<T>(SettingKey<T> key)
        {
            foreach (var provider in _providers.Reverse())
            {
                var result = provider.TryGet(key);
                if (result.HasValue)
                {
                    return result;
                }
            }

            return new ConditionalValue<T>();
        }
    }
}