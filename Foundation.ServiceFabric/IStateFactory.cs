namespace Foundation.ServiceFabric
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Actors.Runtime;

    public interface IStateFactory
    {
        /// <summary>
        /// Constructs a <see cref="StateObject{T}"/> with no initializer factory.
        /// </summary>
        /// <typeparam name="T">DataType stored in this StateObject</typeparam>
        /// <param name="stateManager"><see cref="IActorStateManager"/> that handles serialization of the state data</param>
        /// <param name="key"><see cref="string"/> key used to store and retrieve the state data</param>
        /// <param name="onChange">Delegate to call when the StateObject value changes, null if not used</param>
        /// <param name="equalityComparer">Optional equality comparer for type T. If left null a default equality comparer will be used</param>
        /// <returns><see cref="StateObject{T}"/></returns>
        StateObject<T> Create<T>(
            IActorStateManager stateManager,
            string key,
            Func<T, T, Task> onChange = null,
            IEqualityComparer<T> equalityComparer = null);

        /// <summary>
        /// Constructs a <see cref="StateObject{T}"/> with a synchronous initializer factory.
        /// </summary>
        /// <typeparam name="T">DataType stored in this StateObject</typeparam>
        /// <param name="stateManager"><see cref="IActorStateManager"/> that handles serialization of the state data</param>
        /// <param name="key"><see cref="string"/> key used to store and retrieve the state data</param>
        /// <param name="factory">Synchronous delegate factory to construct initial state value</param>
        /// <param name="onChange">Delegate to call when the StateObject value changes, null if not used</param>
        /// <returns><see cref="StateObject{T}"/></returns>
        StateObject<T> Create<T>(
            IActorStateManager stateManager,
            string key,
            Func<T> factory,
            Func<T, T, Task> onChange = null,
            IEqualityComparer<T> equalityComparer = null);

        /// <summary>
        /// Constructs a <see cref="StateObject{T}"/> with an asynchronous initializer factory.
        /// </summary>
        /// <typeparam name="T">DataType stored in this StateObject</typeparam>
        /// <param name="stateManager"><see cref="IActorStateManager"/> that handles serialization of the state data</param>
        /// <param name="key"><see cref="string"/> key used to store and retrieve the state data</param>
        /// <param name="factory">Async delegate factory to construct initial state value</param>
        /// <param name="onChange">Delegate to call when the StateObject value changes, null if not used</param>
        /// <returns><see cref="StateObject{T}"/></returns>
        StateObject<T> CreateAsync<T>(
            IActorStateManager stateManager,
            string key,
            Func<Task<T>> factory,
            Func<T, T, Task> onChange = null,
            IEqualityComparer<T> equalityComparer = null);

        /// <summary>
        /// Constructs a <see cref="StateCollection{T}"/> with a default initializer factory.
        /// </summary>
        /// <typeparam name="T">DataType to be stored in the returned collection</typeparam>
        /// <param name="stateManager"><see cref="IActorStateManager"/> that manages storage of state</param>
        /// <param name="key"><see cref="string"/> identifier for this state</param>
        /// <param name="equalityComparer"><see cref="Func{T, T, Boolean}"/> equality predicate used to search out comparable values. Pass in null to ignore this argument.</param>
        /// <param name="onAdd"><see cref="Func{T, Task}"/> custom delegate to be called upon addition of a new item to this state. Pass in null to ignore this argument.</param>
        /// <param name="onRemove"><see cref="Func{T, Task}"/> custom delegate to be called upon removal of an item from this state. Pass in null to ignore this argument.</param>
        /// <returns><see cref="StateCollection{T}"/></returns>
        StateCollection<T> CreateCollection<T>(
            IActorStateManager stateManager,
            string key,
            IEqualityComparer<T> equalityComparer,
            Func<T, Task> onAdd,
            Func<T, Task> onRemove);

        /// <summary>
        /// Constructs a <see cref="StateCollection{T}"/> with a provided initializer factory.
        /// </summary>
        /// <typeparam name="T">DataType to be stored in the returned collection</typeparam>
        /// <param name="stateManager"><see cref="IActorStateManager"/> that manages storage of state</param>
        /// <param name="key"><see cref="string"/> identifier for this state</param>
        /// <param name="factory"><see cref="Func{TResult}"/> where TResult is of type <see cref="List{T}"/>. This factory returns the value to be used for initial state</param>
        /// <param name="equalityComparer"><see cref="Func{T, T, Boolean}"/> equality predicate used to search out comparable values. Pass in null to ignore this argument.</param>
        /// <param name="onAdd"><see cref="Func{T, Task}"/> custom delegate to be called upon addition of a new item to this state. Pass in null to ignore this argument.</param>
        /// <param name="onRemove"><see cref="Func{T, Task}"/> custom delegate to be called upon removal of an item from this state. Pass in null to ignore this argument.</param>
        /// <returns><see cref="StateCollection{T}"/></returns>
        /// <remarks>Note, the initializer <paramref name="factory"/> is for simple, potentially blocking construction</remarks>
        StateCollection<T> CreateCollection<T>(
            IActorStateManager stateManager,
            string key,
            Func<List<T>> factory,
            IEqualityComparer<T> equalityComparer,
            Func<T, Task> onAdd,
            Func<T, Task> onRemove);

        /// <summary>
        /// Constructs a <see cref="StateCollection{T}"/> with a provided async initializer factory.
        /// </summary>
        /// <typeparam name="T">DataType to be stored in the returned collection</typeparam>
        /// <param name="stateManager"><see cref="IActorStateManager"/> that manages storage of state</param>
        /// <param name="key"><see cref="string"/> identifier for this state</param>
        /// <param name="factory">
        /// <see cref="Func{TResult}"/> where TResult is of type <see cref="Task{TResult}"/> that returns <see cref="List{T}"/>.
        /// This factory returns the value to be used for initial state</param>
        /// <param name="equalityComparer"><see cref="Func{T, T, Boolean}"/> equality predicate used to search out comparable values. Pass in null to ignore this argument.</param>
        /// <param name="onAdd"><see cref="Func{T, Task}"/> custom delegate to be called upon addition of a new item to this state. Pass in null to ignore this argument.</param>
        /// <param name="onRemove"><see cref="Func{T, Task}"/> custom delegate to be called upon removal of an item from this state. Pass in null to ignore this argument.</param>
        /// <returns><see cref="StateCollection{T}"/></returns>
        /// <remarks>Note, the initializer <paramref name="factory"/> is for potentially complex construction that might require external calls via further async tasks.</remarks>
        StateCollection<T> CreateCollectionAsync<T>(
            IActorStateManager stateManager,
            string key,
            Func<Task<List<T>>> factory,
            IEqualityComparer<T> equalityComparer,
            Func<T, Task> onAdd,
            Func<T, Task> onRemove);
    }
}
