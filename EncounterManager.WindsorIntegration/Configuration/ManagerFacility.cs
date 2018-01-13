namespace EncounterManager.Configuration
{
    using System;
    using System.Fabric;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using Castle.MicroKernel.Facilities;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Foundation;
    using Foundation.ServiceFabric;
    using Foundation.Utilities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.ServiceFabric.Actors.Client;
    using Microsoft.ServiceFabric.Services.Remoting.Client;

    /// <summary>
    /// This facility provides common and required configuration of components necessary for operation of Core actors and services.
    /// Specifically this facility registers the <see cref="IServiceFabricToolbox"/> component and all dependencies it requires.
    /// This facility also provides a basic module pattern via <see cref="Configure"/> and <see cref="ICoreFacilityModule"/>.
    /// Example:
    /// <code><![CDATA[
    /// container.AddFacility<CoreFacility>(facility => facility.Configure(config => {...}));
    /// ]]></code>
    /// </summary>
    public class ManagerFacility : AbstractFacility
    {
        private readonly CoreFacilityConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerFacility"/> class.
        /// </summary>
        public ManagerFacility()
        {
            _config = new CoreFacilityConfiguration();
        }

        /// <summary>
        /// Perform customized operations on this facility during initialization.
        /// </summary>
        /// <param name="configure">The <see cref="ICoreFacilityConfigurer"/> for this facility</param>
        /// <returns>The CoreFacility for further fluent operations</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the provided delegate is null</exception>
        public ManagerFacility Configure(Action<ICoreFacilityConfigurer> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            configure(_config);
            return this;
        }

        protected override void Init()
        {
            try
            {
                Kernel.Resolver.AddSubResolver(new CollectionResolver(Kernel));

                var activationContext = FabricRuntime.GetActivationContext();

                Kernel.Register(
                    Component.For<ISettingsProvider>().ImplementedBy<CompositeSettingsProvider>()
                        .LifestyleTransient(),
                    Component.For<ISettingsProvider>().UsingFactoryMethod(() => new ServiceFabricSettingsProvider(activationContext))
                        .LifestyleSingleton());

                var useKeyVault = activationContext.GetSetting(new SettingKey<bool>("Config", "Azure", "UseKeyVault"));
                if (useKeyVault)
                {
                    var builder = ConfigureAzureKeyVaultConfig(new ConfigurationBuilder(), activationContext);
                    var config = builder.Build();

                    Kernel.Register(
                        Component.For<ISettingsProvider>().Instance(new ConfigurationSettingsProvider(config))
                            .LifestyleSingleton());
                }

                Kernel.Register(
                    Component.For<IServiceFabricToolbox>()
                        .UsingFactoryMethod(k => new ServiceFabricToolbox(
                            () => new ActorProxyFactory(),
                            () => new ServiceProxyFactory(),
                            k.Resolve<ISettingsProvider>))
                        .LifestyleSingleton());

                _config.Modules.ForEach(m => m.Init(Kernel));
            }
            catch (Exception exception)
            {
                ServiceEventSource.Current.Message($"CoreFacility Exception: {exception}");
                throw;
            }
        }

        private IConfigurationBuilder ConfigureAzureKeyVaultConfig(IConfigurationBuilder builder, CodePackageActivationContext activationContext)
        {
            var keyVaultUri = activationContext.GetSetting(new SettingKey<string>("Config", "Azure", "KeyVaultUri"));
            Args.NotNullOrEmpty(keyVaultUri, "KeyVaultUri", "No KeyVaultUri was provided for KeyVault authorization");
            var clientId = activationContext.GetSetting(new SettingKey<string>("Config", "Azure", "ClientId"));
            Args.NotNullOrEmpty(clientId, "ClientId", "No ClientId was provided for KeyVault authorization");

            var authMethod = activationContext.GetSetting(new SettingKey<string>("Config", "Azure", "AuthMethod"), "Secret");
            switch (authMethod)
            {
                case "Secret":
                    return ConfigureAzureKeyVaultConfigUsingSecret(builder, activationContext, keyVaultUri, clientId);
                case "Certificate":
                    return ConfigureAzureKeyVaultConfigUsingCert(builder, activationContext, keyVaultUri, clientId);
                default:
                    throw new InvalidOperationException($"AuthMethod {authMethod} not recognized");
            }
        }

        private IConfigurationBuilder ConfigureAzureKeyVaultConfigUsingCert(IConfigurationBuilder builder, CodePackageActivationContext activationContext, string keyVaultUri, string clientId)
        {
            var store = new X509Store(StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                var certThumbprint = activationContext.GetSetting(new SettingKey<string>("Config", "Azure", "CertificateThumbprint"));
                Args.NotNullOrEmpty(certThumbprint, "CertificateThumbprint", "No certificate thumbprint is configured for KeyVault authorization");
                var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, certThumbprint, false);
                var cert = certCollection.OfType<X509Certificate2>().Single();

                return builder.AddAzureKeyVault(keyVaultUri, clientId, cert);
            }
            finally
            {
                store.Close();
            }
        }

        private IConfigurationBuilder ConfigureAzureKeyVaultConfigUsingSecret(IConfigurationBuilder builder, CodePackageActivationContext activationContext, string keyVaultUri, string clientId)
        {
            var clientSecret = activationContext.GetSetting(new SettingKey<string>("Config", "Azure", "ClientSecret"));
            Args.NotNullOrEmpty(clientSecret, "ClientSecret", "No ClientSecret was provided for KeyVault authorization");

            return builder.AddAzureKeyVault(keyVaultUri, clientId, clientSecret);
        }
    }
}