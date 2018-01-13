namespace EncounterManager.Users.UserRegistry.Configuration
{
    using Castle.Facilities.ServiceFabricIntegration;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using EncounterManager.Configuration;

    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<ManagerFacility>();
            container.AddFacility<ServiceFabricFacility>();

            container.Register(
                Component.For<UserRegistryService>().AsStatefulService("UserRegistryServiceType"));
        }
    }
}
