namespace Foundation.ServiceFabric
{
    using System;
    using Foundation.Utilities;
    using Microsoft.ServiceFabric.Actors.Client;
    using Microsoft.ServiceFabric.Services.Remoting.Client;

    public class ServiceFabricToolbox : IServiceFabricToolbox
    {
        private readonly Lazy<IActorProxyFactory> _actors;
        private readonly Lazy<IServiceProxyFactory> _services;
        private readonly Lazy<ISettingsProvider> _settings;

        public IActorProxyFactory Actors => _actors.Value;
        public IServiceProxyFactory Services => _services.Value;
        public ISettingsProvider Settings => _settings.Value;

        public ServiceFabricToolbox()
            : this(() => new ActorProxyFactory(), () => new ServiceProxyFactory(), () => null)
        {
            
        }

        public ServiceFabricToolbox(
            Func<IActorProxyFactory> actorProxyFactoryFactory,
            Func<IServiceProxyFactory> serviceProxyFactoryFactory,
            Func<ISettingsProvider> settingsProviderFactory)
        {
            Args.NotNull(actorProxyFactoryFactory, nameof(actorProxyFactoryFactory));
            Args.NotNull(serviceProxyFactoryFactory, nameof(serviceProxyFactoryFactory));
            Args.NotNull(settingsProviderFactory, nameof(settingsProviderFactory));

            _actors = new Lazy<IActorProxyFactory>(actorProxyFactoryFactory);
            _services = new Lazy<IServiceProxyFactory>(serviceProxyFactoryFactory);
            _settings = new Lazy<ISettingsProvider>(settingsProviderFactory);
        }
    }
}