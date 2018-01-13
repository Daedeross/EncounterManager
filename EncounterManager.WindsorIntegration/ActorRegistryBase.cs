namespace EncounterManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Fabric;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Foundation;
    using Foundation.ServiceFabric;
    using Microsoft.ServiceFabric.Actors;
    using Microsoft.ServiceFabric.Data;
    using Microsoft.ServiceFabric.Data.Collections;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;
    using Microsoft.ServiceFabric.Services.Remoting;
    using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
    using Microsoft.ServiceFabric.Services.Remoting.V1.FabricTransport.Runtime;
    using Microsoft.ServiceFabric.Services.Runtime;

    public abstract class ActorRegistryBase : StatefulService, IService
    {
        protected IReliableStateAccessor StateAccessor { get; }

        protected ActorRegistryBase(StatefulServiceContext serviceContext)
            : base(serviceContext)
        {
            StateAccessor = new PrefixReliableStateAccessor(StateManager, Context.ServiceTypeName);
        }

        protected ActorRegistryBase(StatefulServiceContext serviceContext,
                                    IReliableStateManagerReplica2 reliableStateManagerReplica)
            : base(serviceContext, reliableStateManagerReplica)
        {
            StateAccessor = new PrefixReliableStateAccessor(StateManager, Context.ServiceTypeName);
        }

        /// <summary>
        /// Catalogs the provided actor and its identity into this registry.
        /// This method expects an external transaction to be provided for managing atomicity
        /// </summary>
        /// <param name="tx">The <see cref="ITransaction"/> this Add operation is part of.</param>
        /// <param name="reference"><see cref="ActorReference"/> to store into internal state</param>
        /// <param name="identity">The underlying identity of the provided actor. This is not the actor id</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        protected async Task AddActorAsync(ITransaction tx, ActorReference reference, Identity identity, CancellationToken cancellationToken)
        {
            var referenceMap = await GetReferenceMapAsync();
            var indexMap = await GetIndexMapAsync();
            var indexableRef = new IndexableActorReference(reference);
            var indexLookup = await referenceMap.TryGetValueAsync(tx, indexableRef, ReliableCollectionDefaults.ReadTimeout, cancellationToken);

            long index;
            IndexState state;

            if (indexLookup.HasValue)
            {
                index = indexLookup.Value;
                var stateLookup = await indexMap.TryGetValueAsync(tx, index, ReliableCollectionDefaults.ReadTimeout, cancellationToken);
                if (!stateLookup.HasValue)
                {
                    throw new InvalidOperationException($"{indexableRef.StringRepresentation} is missing its internal IndexState");
                }
                state = stateLookup.Value;
                IndexState newState;
                if (Equals(state.Identity, identity))
                {
                    newState = state;
                }
                else
                {
                    newState = new IndexState
                    {
                        Reference = reference,
                        Identity = identity,
                        Index = index
                    };
                    // replace old state
                    await indexMap.SetAsync(tx, index, newState, ReliableCollectionDefaults.WriteTimeout, cancellationToken);
                }

                await OnUpdateActorAsync(tx, state, newState, cancellationToken);
            }
            else
            {
                var identityMap = await GetIdentityMapAsync();
                var existingIndex = await identityMap.TryGetValueAsync(tx, identity, ReliableCollectionDefaults.ReadTimeout, cancellationToken);
                if (existingIndex.HasValue)
                {
                    var existingState = await indexMap.TryGetValueAsync(tx, existingIndex.Value);
                    throw new InvalidOperationException($"{identity} is already registered to Actor {new IndexableActorReference(existingState.Value.Reference).StringRepresentation}");
                }

                var propertiesMap = await GetPropertiesMapAsync();

                index = (long)await propertiesMap.AddOrUpdateAsync(tx, "currentIndex", 1L, (key, value) => (long)value + 1, ReliableCollectionDefaults.WriteTimeout, cancellationToken);
                state = new IndexState
                {
                    Reference = reference,
                    Identity = identity,
                    Index = index
                };

                await referenceMap.AddAsync(tx, indexableRef, index, ReliableCollectionDefaults.WriteTimeout, cancellationToken);
                await indexMap.AddAsync(tx, index, state, ReliableCollectionDefaults.WriteTimeout, cancellationToken);
                await identityMap.AddAsync(tx, identity, index, ReliableCollectionDefaults.WriteTimeout, cancellationToken);

                await OnAddActorAsync(tx, state, cancellationToken);
            }
        }

        /// <summary>
        /// Registry implementations that need to catalog more information about each actor registration than what is stored by default should override
        /// this method to participate in the transactional registration process.
        /// This method is passed the <see cref="IndexState"/> stored into the transactional state. This state object should provide all the details necessary to
        /// create reference state for more details.
        /// </summary>
        /// <param name="tx">The <see cref="ITransaction"/> this Add operation is part of.</param>
        /// <param name="state"><see cref="IndexState"/></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        /// <remarks>
        /// Expected behavior for this method is the provided transaction <paramref name="tx"/> should not be explicitly committed under normal behavior.
        /// If an error occurs an exception is the preferred method to trigger failure, but the transaction can be aborted with <see cref="ITransaction.Abort"/>
        /// explicitly.
        /// Both methods will result in complete rollback of the transaction.
        /// </remarks>
        protected virtual Task OnAddActorAsync(ITransaction tx, IndexState state, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Implementations that need to allow alterable state should override this method. Additions that already have an internal state will trigger an update
        /// instead of an add which will pass the old and new state to this method for further re-cataloging activity.
        /// </summary>
        /// <param name="tx">The <see cref="ITransaction"/> this Add operation is part of.</param>
        /// <param name="oldState">The old <see cref="IndexState"/></param>
        /// <param name="newState">The new <see cref="IndexState"/></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        protected virtual Task OnUpdateActorAsync(ITransaction tx, IndexState oldState, IndexState newState, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        /// <summary>
        /// Removes the provided actor and all cataloged state from this registry.
        /// This method expects an external transaction to be provided for managing atomicity
        /// </summary>
        /// <param name="tx">The <see cref="ITransaction"/> this Remove operation is part of.</param>
        /// <param name="reference"><see cref="ActorReference"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        protected async Task RemoveActorAsync(ITransaction tx, ActorReference reference, CancellationToken cancellationToken)
        {
            var referenceMap = await GetReferenceMapAsync();
            var indexableRef = new IndexableActorReference(reference);

            var index = await referenceMap.TryRemoveAsync(tx, indexableRef, ReliableCollectionDefaults.WriteTimeout, cancellationToken);
            if (!index.HasValue) return;

            var indexMap = await GetIndexMapAsync();
            var state = await indexMap.TryRemoveAsync(tx, index.Value, ReliableCollectionDefaults.WriteTimeout, cancellationToken);
            if (state.HasValue)
            {
                var identityMap = await GetIdentityMapAsync();
                await identityMap.TryRemoveAsync(tx, state.Value.Identity, ReliableCollectionDefaults.WriteTimeout, cancellationToken);

                await OnRemoveActorAsync(tx, state.Value, cancellationToken);
            }
        }

        /// <summary>
        /// OnRemove is the counterpart to OnAdd, both should be implemented if one is.
        /// Like OnAdd the OnRemove is provided the <see cref="IndexState"/> used for all registration operations and should be used to unregister any additional
        /// details previously added.
        /// </summary>
        /// <param name="tx">The <see cref="ITransaction"/> this Remove operation is part of.</param>
        /// <param name="state"><see cref="IndexState"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        /// <remarks>
        /// Expected behavior for this method is the provided transaction <paramref name="tx"/> should not be explicitly committed under normal behavior.
        /// If an error occurs an exception is the preferred method to trigger failure, but the transaction can be aborted with <see cref="ITransaction.Abort"/>
        /// explicitly.
        /// Both methods will result in complete rollback of the transaction.
        /// </remarks>
        protected virtual Task OnRemoveActorAsync(ITransaction tx, IndexState state, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Attempts to locate the <see cref="ActorReference"/> by using the provided <paramref name="identity"/>
        /// This method expects an external transaction to be provided for managing atomicity
        /// </summary>
        /// <param name="tx">The tx.</param>
        /// <param name="identity">The identity.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task{TResult}"/> of type <see cref="ActorReference"/></returns>
        protected async Task<ActorReference> FindActorByIdentityAsync(ITransaction tx, Identity identity, CancellationToken cancellationToken)
        {
            var identityMap = await GetIdentityMapAsync();

            var index = await identityMap.TryGetValueAsync(tx, identity, ReliableCollectionDefaults.ReadTimeout, cancellationToken);
            if (!index.HasValue) return null;

            var indexMap = await GetIndexMapAsync();
            var reference = await indexMap.TryGetValueAsync(tx, index.Value, ReliableCollectionDefaults.ReadTimeout, cancellationToken);

            if (!reference.HasValue) return null;
            return reference.Value.Reference;
        }

        /// <summary>
        /// Get all indexed actor references from this manager
        /// </summary>
        /// <param name="transaction">The <see cref="ITransaction"/> this operation is part of.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <param name="pagedRequest"></param>
        /// <returns><see cref="QueryResponse{ActorReference}"/></returns>
        protected async Task<QueryResponse<ActorReference>> GetActorsAsync(ITransaction transaction, CancellationToken cancellationToken, PagedRequest pagedRequest)
        {
            var referenceMap = await GetReferenceMapAsync();
            var asyncEnumerable = await referenceMap.CreateEnumerableAsync(transaction, EnumerationMode.Ordered);

            var queryResponse = new QueryResponse<ActorReference>
            {
                Offset = pagedRequest.Start,
                Total = await referenceMap.GetCountAsync(transaction),
                Results = (await PageAsync(asyncEnumerable, pagedRequest, cancellationToken)).Select(keyValuePair => keyValuePair.Key.Reference).ToList()
            };

            return queryResponse;
        }

        /// <summary>
        /// Get the state reference for a given external identity
        /// </summary>
        /// <param name="tx">The <see cref="ITransaction"/> this operation is part of.</param>
        /// <param name="identity">The <see cref="Identity"/> to retrieve</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task{TResult}"/> of <see cref="IndexState"/></returns>
        protected async Task<IndexState> GetStateAsync(ITransaction tx, Identity identity, CancellationToken cancellationToken)
        {
            var identityMap = await GetIdentityMapAsync();
            var index = await identityMap.TryGetValueAsync(tx, identity);
            if (!index.HasValue) return null;

            var indexMap = await GetIndexMapAsync();
            var state = await indexMap.TryGetValueAsync(tx, index.Value, ReliableCollectionDefaults.ReadTimeout, cancellationToken);
            return state.Value;
        }

        /// <summary>
        /// Get the state reference for a given internal index
        /// </summary>
        /// <param name="tx">The <see cref="ITransaction"/> this operation is part of.</param>
        /// <param name="index">The index to retrieve</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task{TResult}"/> of <see cref="IndexState"/></returns>
        protected async Task<IndexState> GetStateAsync(ITransaction tx, long index, CancellationToken cancellationToken)
        {
            var indexMap = await GetIndexMapAsync();
            var result = await indexMap.TryGetValueAsync(tx, index, ReliableCollectionDefaults.ReadTimeout, cancellationToken);
            return result.Value;
        }

        /// <summary>
        /// Get the state references for the given internal indexes
        /// </summary>
        /// <param name="transaction">The <see cref="ITransaction"/> this operation is part of.</param>
        /// <param name="indices">The indices to retrieve</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task{TResult}"/> of <see cref="IndexState"/></returns>
        protected async Task<List<IndexState>> GetStatesAsync(ITransaction transaction, IEnumerable<long> indices, CancellationToken cancellationToken)
        {
            var results = new List<IndexState>();
            var indexMap = await GetIndexMapAsync();
            foreach (var index in indices)
            {
                var stateRequest = await indexMap.TryGetValueAsync(transaction, index, ReliableCollectionDefaults.ReadTimeout, cancellationToken);
                if (stateRequest.HasValue)
                {
                    results.Add(stateRequest.Value);
                }
            }
            return results;
        }

        /// <summary>
        /// Get the state references for the given internal indexes
        /// </summary>
        /// <param name="transaction">The <see cref="ITransaction"/> this operation is part of.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <param name="pagedRequest"></param>
        /// <returns><see cref="QueryResponse{IndexState}"/></returns>
        protected async Task<QueryResponse<IndexState>> GetStatesAsync(ITransaction transaction, CancellationToken cancellationToken, PagedRequest pagedRequest)
        {
            var indexMap = await GetIndexMapAsync();
            var asyncEnumerable = await indexMap.CreateEnumerableAsync(transaction, EnumerationMode.Ordered);

            return new QueryResponse<IndexState>
            {
                Offset = pagedRequest.Start,
                Total = await indexMap.GetCountAsync(transaction),
                Results = (await PageAsync(asyncEnumerable, pagedRequest, cancellationToken)).Select(keyValuePair => keyValuePair.Value).ToList()
            };
        }

        #region Helpers

        protected Task<IReliableDictionary<string, object>> GetPropertiesMapAsync()
        {
            return TimeoutRetryHelper.Execute(token => StateAccessor.Get<IReliableDictionary<string, object>>("propertiesMap"));
        }

        protected Task<IReliableDictionary<IndexableActorReference, long>> GetReferenceMapAsync()
        {
            return TimeoutRetryHelper.Execute(token => StateAccessor.Get<IReliableDictionary<IndexableActorReference, long>>("referenceMap"));
        }

        protected Task<IReliableDictionary<long, IndexState>> GetIndexMapAsync()
        {
            return TimeoutRetryHelper.Execute(token => StateAccessor.Get<IReliableDictionary<long, IndexState>>("indexMap"));
        }

        protected Task<IReliableDictionary<Identity, long>> GetIdentityMapAsync()
        {
            return TimeoutRetryHelper.Execute(token => StateAccessor.Get<IReliableDictionary<Identity, long>>("identityMap"));
        }

        private static async Task<List<T>> PageAsync<T>(IAsyncEnumerable<T> enumerable, PagedRequest request, CancellationToken cancellationToken)
        {
            var records = new List<T>();
            var enumerator = enumerable.GetAsyncEnumerator();
            int skip = 0;
            while (skip < request.Start && await enumerator.MoveNextAsync(cancellationToken))
            {
                skip++;
            }
            int n = 0;
            int take = request.PageSize.GetValueOrDefault(int.MaxValue);
            while (n < take && await enumerator.MoveNextAsync(cancellationToken))
            {
                records.Add(enumerator.Current);
                n++;
            }
            return records;
        }

        #endregion Helpers

        /// <inheritdoc />
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context => new FabricTransportServiceRemotingListener(context, this, new FabricTransportRemotingListenerSettings
                {
                    EndpointResourceName = FormatEndpointName(context)
                }))
            };
        }

        private static string FormatEndpointName(StatefulServiceContext serviceContext)
        {
            var prefix = serviceContext.ServiceTypeName;
            if (prefix.EndsWith("Type"))
            {
                prefix = prefix.Substring(0, prefix.Length - 4);
            }

            return $"{prefix}Endpoint";
        }
    }
}
