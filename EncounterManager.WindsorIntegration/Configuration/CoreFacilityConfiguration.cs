namespace EncounterManager.Configuration
{
    using System.Collections.Generic;

    internal class CoreFacilityConfiguration : ICoreFacilityConfigurer
    {
        public List<ICoreFacilityModule> Modules { get; }

        public CoreFacilityConfiguration()
        {
            Modules = new List<ICoreFacilityModule>();
        }

        public ICoreFacilityConfigurer Using(params ICoreFacilityModule[] modules)
        {
            Modules.AddRange(modules);
            return this;
        }
    }
}