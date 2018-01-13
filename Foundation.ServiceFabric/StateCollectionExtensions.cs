namespace Foundation.ServiceFabric
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Actors;

    public static class StateCollectionExtensions
    {
        public static async Task<IEnumerable<TState>> AddRangeAsync<TState, TActor>(this IStateCollection<TState> collection, IEnumerable<TActor> values, Func<TActor, Task<TState>> factory, IEqualityComparer<ActorReference> comparer = null)
            where TState : ActorState<TActor>
            where TActor : IActor
        {
            var list = await collection.GetAsync();
            var actualRange = new List<TState>();

            var set = new ActorStateSet<TState, TActor>(comparer);
            foreach (var element in list) set.Add(element);
            foreach (var value in values)
            {
                var added = await set.AddAsync(value, factory);
                if (added != null)
                {
                    actualRange.Add(added);
                }
            }

            if (actualRange.Count == 0)
            {
                return actualRange;
            }

            return await collection.AddRangeAsync(actualRange);
        }

        public static async Task<IEnumerable<TState>> RemoveRangeAsync<TState, TActor>(this IStateCollection<TState> collection, IEnumerable<TActor> values, IEqualityComparer<ActorReference> comparer = null)
            where TState : ActorState<TActor>
            where TActor : IActor
        {
            var list = await collection.GetAsync();

            var set = new ActorStateSet<TState, TActor>(comparer);
            foreach (var element in list) set.Add(element);

            return await collection.RemoveRangeAsync(values.Select(value => set.Remove(value)).Where(removed => removed != null));
        }
    }
}