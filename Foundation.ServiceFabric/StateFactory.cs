namespace Foundation.ServiceFabric
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Actors.Runtime;

    public sealed class StateFactory : IStateFactory
    {
        public static readonly IStateFactory Default = new StateFactory();

        public static StateObject<T> Create<T>(IActorStateManager stateManager, string key, Func<T, T, Task> onChange = null, IEqualityComparer<T> equalityComparer = null)
        {
            return Default.Create<T>(stateManager, key, onChange, equalityComparer);
        }

        public static StateObject<T> Create<T>(IActorStateManager stateManager, string key, Func<T> factory, Func<T, T, Task> onChange = null, IEqualityComparer<T> equalityComparer = null)
        {
            return Default.Create(stateManager, key, factory, onChange, equalityComparer);
        }

        public static StateObject<T> CreateAsync<T>(IActorStateManager stateManager, string key, Func<Task<T>> factory, Func<T, T, Task> onChange = null, IEqualityComparer<T> equalityComparer = null)
        {
            return Default.CreateAsync(stateManager, key, factory, onChange, equalityComparer);
        }

        public static StateCollection<T> CreateCollection<T>(IActorStateManager stateManager, string key, IEqualityComparer<T> equalityComparer = null, Func<T, Task> onAdd = null, Func<T, Task> onRemove = null)
        {
            return Default.CreateCollection(stateManager, key, equalityComparer, onAdd, onRemove);
        }

        public static StateCollection<T> CreateCollection<T>(IActorStateManager stateManager, string key, Func<List<T>> factory, IEqualityComparer<T> equalityComparer = null, Func<T, Task> onAdd = null, Func<T, Task> onRemove = null)
        {
            return Default.CreateCollection(stateManager, key, factory, equalityComparer, onAdd, onRemove);
        }

        public static StateCollection<T> CreateCollectionAsync<T>(IActorStateManager stateManager, string key, Func<Task<List<T>>> factory, IEqualityComparer<T> equalityComparer = null, Func<T, Task> onAdd = null, Func<T, Task> onRemove = null)
        {
            return Default.CreateCollectionAsync(stateManager, key, factory, equalityComparer, onAdd, onRemove);
        }

        StateObject<T> IStateFactory.Create<T>(IActorStateManager stateManager, string key, Func<T, T, Task> onChange, IEqualityComparer<T> equalityComparer)
        {
            return new StateObject<T>(stateManager, key, null, onChange, equalityComparer);
        }

        StateObject<T> IStateFactory.Create<T>(IActorStateManager stateManager, string key, Func<T> factory, Func<T, T, Task> onChange, IEqualityComparer<T> equalityComparer)
        {
            return new StateObject<T>(stateManager, key, () => Task.FromResult(factory()), onChange, equalityComparer);
        }

        StateObject<T> IStateFactory.CreateAsync<T>(IActorStateManager stateManager, string key, Func<Task<T>> factory, Func<T, T, Task> onChange, IEqualityComparer<T> equalityComparer)
        {
            return new StateObject<T>(stateManager, key, factory, onChange, equalityComparer);
        }

        StateCollection<T> IStateFactory.CreateCollection<T>(IActorStateManager stateManager, string key, IEqualityComparer<T> equalityComparer, Func<T, Task> onAdd, Func<T, Task> onRemove)
        {
            return new StateCollection<T>(stateManager, key, () => Task.FromResult(new List<T>()), equalityComparer ?? EqualityComparer<T>.Default, onAdd, onRemove);
        }

        StateCollection<T> IStateFactory.CreateCollection<T>(IActorStateManager stateManager, string key, Func<List<T>> factory, IEqualityComparer<T> equalityComparer, Func<T, Task> onAdd, Func<T, Task> onRemove)
        {
            return new StateCollection<T>(stateManager, key, () => Task.FromResult(factory()), equalityComparer ?? EqualityComparer<T>.Default, onAdd, onRemove);
        }

        StateCollection<T> IStateFactory.CreateCollectionAsync<T>(IActorStateManager stateManager, string key, Func<Task<List<T>>> factory, IEqualityComparer<T> equalityComparer, Func<T, Task> onAdd, Func<T, Task> onRemove)
        {
            return new StateCollection<T>(stateManager, key, factory, equalityComparer ?? EqualityComparer<T>.Default, onAdd, onRemove);
        }
    }
}
