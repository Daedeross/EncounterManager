namespace EncounterManager.Encounters.Configuration
{
    using Castle.Facilities.ServiceFabricIntegration;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using EncounterManager.Configuration;

    class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<ManagerFacility>();
            container.AddFacility<ServiceFabricFacility>(f => f.Configure(c => c.UsingActors()));

            container.Register(
                Component.For<Encounter>().AsActor().LifestyleTransient(),
                Component.For<EncounterLoader>().AsActor().LifestyleTransient());
        }
    }
}
