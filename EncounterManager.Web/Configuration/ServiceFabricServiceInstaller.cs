namespace EncounterManager.Web.Configuration
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    //using EncounterManager.Configuration;
    using Microsoft.AspNetCore.Mvc;
    //using Foundation;
    //using EncounterManager.Storage;
    //using EncounterManager.Storage.Client;
    using EncounterManager.Web.Internals;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Infrastructure;

    /// <summary>
    /// ServiceFabric service installer
    /// </summary>
    public class ServiceFabricServiceInstaller : IWindsorInstaller
    {
        /// <summary>
        ///   Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IActionContextAccessor>()
                    .ImplementedBy<ActionContextAccessor>()
                    .LifestyleSingleton(),
                Component.For<IHttpContextAccessor>()
                    .ImplementedBy<HttpContextAccessor>()
                    .LifestyleSingleton(),
                Component.For<IActionResultFactory>()
                    .ImplementedBy<ActionResultFactory>()
                    .LifestyleSingleton());

            container.Register(
                Classes.FromThisAssembly().BasedOn<Controller>()
                    .WithServiceDefaultInterfaces()
                    .LifestyleTransient());

            //container.Register(
            //    Component.For<IStorageClientFactory>()
            //        .UsingFactoryMethod(kernel =>
            //        {
            //            var provider = kernel.Resolve<ISettingsProvider>();
            //            try
            //            {
            //                var connectionString = provider.Get(StorageSettingKeys.StorageConnectionString);
            //                return new StorageClientFactory(connectionString);
            //            }
            //            finally
            //            {
            //                kernel.ReleaseComponent(provider);
            //            }
            //        })
            //        .LifestyleSingleton());

            #region Behaviors

            container.Register(
                Classes.FromThisAssembly()
                    .InNamespace("EncounterManager.Web.Behaviors.ServiceFabric")
                    .WithServiceDefaultInterfaces()
                    .LifestyleSingleton());

            #endregion // Behaviors

        }
    }
}
