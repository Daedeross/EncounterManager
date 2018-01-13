namespace Foundation
{
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Actors;

    /// <summary>
    /// Base actor interface that enables retrieval of identity.
    /// This interface is not to be implemented directly, it should be an augmentation of
    /// an actual Actor interface.
    /// </summary>
    public interface IIdentityProvider : IActor
    {
        /// <summary>
        /// Gets the <see cref="Identity"/> of the current actor.
        /// </summary>
        /// <returns><see cref="Task{TResult}"/> of <see cref="Identity"/></returns>
        Task<Identity> GetIdentityAsync();
    }
}