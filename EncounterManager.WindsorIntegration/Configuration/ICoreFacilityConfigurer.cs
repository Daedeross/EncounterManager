namespace EncounterManager.Configuration
{
    using Castle.MicroKernel;

    /// <summary>
    /// This interface declares operations that can be used to alter the initialization of a <see cref="ManagerFacility"/>.
    /// The primary way to hook into the core facility is by providing one or more instances of <see cref="ICoreFacilityModule"/>
    /// which provides easy, pre-packaged inclusion of additional <see cref="IKernel"/> registration.
    /// </summary>
    public interface ICoreFacilityConfigurer
    {
        /// <summary>
        /// Apply additional <see cref="ICoreFacilityModule"/> instances to the current configuration.
        /// </summary>
        /// <param name="modules"><see cref="ICoreFacilityModule"/></param>
        /// <returns>The current <see cref="ICoreFacilityConfigurer"/> instance for further Fluent API inclusions</returns>
        ICoreFacilityConfigurer Using(params ICoreFacilityModule[] modules);
    }
}