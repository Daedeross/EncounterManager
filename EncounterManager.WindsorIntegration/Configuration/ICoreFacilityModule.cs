namespace EncounterManager.Configuration
{
    using Castle.MicroKernel;

    /// <summary>
    /// This interface provides a simple extension mechanism for the <see cref="ManagerFacility"/> to add additional
    /// kernel registrations.
    /// </summary>
    public interface ICoreFacilityModule
    {
        /// <summary>
        /// Initializes the specified kernel. Called when <see cref="ManagerFacility"/> is initialized.
        /// </summary>
        /// <param name="kernel"><see cref="IKernel"/></param>
        void Init(IKernel kernel);
    }
}
