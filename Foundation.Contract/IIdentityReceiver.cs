namespace Foundation
{
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Actors;

    /// <summary>
    /// Foundational interface contract for setting an actor's Identity
    /// </summary>
    /// <remarks>
    /// This interface should only be used from controlled interaction and not for general consumption.
    /// Specifically, Identity should only be set at the beginning of an actor's life and never set again
    /// as Identity is globally identifiable information it shouldn't be changing once set.
    /// </remarks>
    public interface IIdentityReceiver : IActor
    {
        /// <summary>
        /// Set an actor's Identity
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns><see cref="Task"/></returns>
        Task SetIdentityAsync(Identity identity);
    }
}