namespace EncounterManager.Encounters
{
    using EncounterManager.Encounters.Model;
    using Microsoft.ServiceFabric.Actors;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IEncounter : IActor
    {
        /// <summary>
        /// Get the base record for the Encounter, for grids.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="EncounterRecord"/></returns>
        Task<EncounterRecord> GetRecordAsync(CancellationToken cancellationToken);

    }
}
