namespace EncounterManager.Users.UserRegistry
{
    using EncounterManager.Services;
    using Foundation;
    using Foundation.ServiceFabric;
    using Foundation.Utilities;
    using Microsoft.ServiceFabric.Actors;
    using Microsoft.ServiceFabric.Data;
    using Microsoft.ServiceFabric.Data.Collections;
    using System;
    using System.Collections.Generic;
    using System.Fabric;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class UserRegistryService : ActorRegistryBase, IUserRegistryService
    {
        private IServiceFabricToolbox _toolbox;

        private const string NameMapKey = "name";
        private const string NormalizedNameMapKey = "normalizedName";

        public UserRegistryService(StatefulServiceContext serviceContext, IServiceFabricToolbox toolbox)
            : base(serviceContext)
        {
            Args.NotNull(toolbox, nameof(toolbox));
            _toolbox = toolbox;
        }

        internal UserRegistryService(
            StatefulServiceContext serviceContext,
            IReliableStateManagerReplica2 stateManager,
            IServiceFabricToolbox toolbox)
            : base(serviceContext, stateManager)
        {
            Args.NotNull(toolbox, nameof(toolbox));
            _toolbox = toolbox;
        }

        #region IReliableCollection Helpers

        private Task<IReliableDictionary<string, long>> GetNameMapAsync()
        {
            return StateManager.GetOrAddAsync<IReliableDictionary<string, long>>(NameMapKey);
        }

        private Task<IReliableDictionary<string, long>> GetNormalizedNameMapAsync()
        {
            return StateManager.GetOrAddAsync<IReliableDictionary<string, long>>(NormalizedNameMapKey);
        }

        #endregion // IReliableCollection Helpers

        public async Task AddUserAsync(ActorReference reference, Identity identity, CancellationToken cancellationToken)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                await AddActorAsync(tx, reference, identity, cancellationToken);
                await tx.CommitAsync();
            }
        }

        public async Task RemoveUserAsync(ActorReference reference, CancellationToken cancellationToken)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                await RemoveActorAsync(tx, reference, cancellationToken);
                await tx.CommitAsync();
            }
        }

        public Task<ActorReference> FindUserByIdentityAsync(Identity identity, CancellationToken cancellationToken)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                return FindActorByIdentityAsync(tx, identity, cancellationToken);
            }
        }

        public async Task<ActorReference> FindUserByNameAsync(string userName, CancellationToken cancellationToken)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var nameMap = await GetNameMapAsync();

                var result = await nameMap.TryGetValueAsync(tx, userName);
                if (!result.HasValue)
                {
                    return null;
                }

                var state = await GetStateAsync(tx, result.Value, cancellationToken);
                return state.Reference;
            }
        }

        public async Task<ActorReference> FindUserByNormalizedNameAsync(string normalizedName, CancellationToken cancellationToken)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var nameMap = await GetNormalizedNameMapAsync();

                var result = await nameMap.TryGetValueAsync(tx, normalizedName);
                if (!result.HasValue)
                {
                    return null;
                }

                var state = await GetStateAsync(tx, result.Value, cancellationToken);
                return state.Reference;
            }
        }

        protected override async Task OnAddActorAsync(ITransaction tx, IndexState state, CancellationToken cancellationToken)
        {
            await base.OnAddActorAsync(tx, state, cancellationToken);

            var user =  _toolbox.User(state.Reference);

            var name = await user.GetNameAsync();
            var normalizedName = await user.GetNormalizedNameAsync();

            var nameMap = await GetNameMapAsync();
            var normalizedNameMap = await GetNormalizedNameMapAsync();

            await nameMap.AddAsync(tx, name, state.Index, ReliableCollectionDefaults.WriteTimeout, cancellationToken);
            await normalizedNameMap.AddAsync(tx, normalizedName, state.Index, ReliableCollectionDefaults.WriteTimeout, cancellationToken);
        }

        protected override async Task OnRemoveActorAsync(ITransaction tx, IndexState state, CancellationToken cancellationToken)
        {
            await base.OnRemoveActorAsync(tx, state, cancellationToken);


            var user = _toolbox.User(state.Reference);

            var name = await user.GetNameAsync();
            var normalizedName = await user.GetNormalizedNameAsync();

            var nameMap = await GetNameMapAsync();
            var normalizedNameMap = await GetNormalizedNameMapAsync();

            await nameMap.TryRemoveAsync(tx, name, ReliableCollectionDefaults.WriteTimeout, cancellationToken);
            await normalizedNameMap.TryRemoveAsync(tx, normalizedName, ReliableCollectionDefaults.WriteTimeout, cancellationToken);
        }
    }
}
