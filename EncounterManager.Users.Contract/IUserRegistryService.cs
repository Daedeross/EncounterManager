namespace EncounterManager.Users
{
    using Foundation;
    using Microsoft.ServiceFabric.Actors;
    using Microsoft.ServiceFabric.Services.Remoting;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUserRegistryService: IService
    {
        #region Gereral

        /// <summary>
        /// Add a new user to this registry
        /// </summary>
        /// <param name="reference"><see cref="ActorReference"/> for <see cref="IUser"/></param>
        /// <param name="identity"><see cref="Identity"/> of the user being registered</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task AddUserAsync(ActorReference reference, Identity identity, CancellationToken cancellationToken);

        /// <summary>
        /// Removes an existing user from this registry
        /// </summary>
        /// <param name="reference"><see cref="ActorReference"/> for <see cref="IUser"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task RemoveUserAsync(ActorReference reference, CancellationToken cancellationToken);

        /// <summary>
        /// Locates an <see cref="IUser"/> instance registered in this registry with the given domain identity
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="ActorReference"/> for <see cref="IUser"/></returns>
        Task<ActorReference> FindUserByIdentityAsync(Identity identity, CancellationToken cancellationToken);

        #endregion // Gereral

        Task<ActorReference> FindUserByNormalizedNameAsync(string normalizedName, CancellationToken cancellationToken);

        Task<ActorReference> FindUserByNameAsync(string userName, CancellationToken cancellationToken);
    }
}
