namespace Foundation.ServiceFabric
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Foundation.Utilities;
    using Microsoft.ServiceFabric.Actors.Runtime;

    public class StateObject<T> : IStateObject<T>
    {
        private readonly Func<Task<T>> _factory;
        private readonly Func<T, T, Task> _onChange;
        private readonly IEqualityComparer<T> _equalityComparer;
        private T _value;
        private bool _needsSync = true;

        public string Key { get; }
        protected IActorStateManager StateManager { get; }

        public StateObject(IActorStateManager stateManager, string key, Func<Task<T>> asyncFactory, Func<T, T, Task> onChange = null, IEqualityComparer<T> equalityComparer = null)
        {
            Args.NotNull(stateManager, nameof(stateManager));
            Args.NotNullOrEmpty(key, nameof(key));

            StateManager = stateManager;
            Key = key;
            _factory = asyncFactory;
            _onChange = onChange;
            _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        public Task<bool> HasStateAsync()
        {
            return StateManager.ContainsStateAsync(Key);
        }

        public async Task<bool> IsEmptyAsync()
        {
            return !await StateManager.ContainsStateAsync(Key) || await ReadAsync(value => Equals(value, default(T)));
        }

        public async Task<T> GetAsync()
        {
            if (_needsSync)
            {
                var recordResult = await StateManager.TryGetStateAsync<T>(Key);
                if (recordResult.HasValue)
                {
                    _value = recordResult.Value;
                }
                else
                {
                    if (_factory == null)
                    {
                        throw new InvalidOperationException($"No factory method was provided and no pre-existing state for '{Key}' could be found");
                    }
                    _value = await _factory();
                    await StateManager.SetStateAsync(Key, _value);
                }
                _needsSync = false;
            }

            return _value;
        }

        public async Task<TResult> ReadAsync<TResult>(Func<T, TResult> valueAccessor)
        {
            Args.NotNull(valueAccessor, nameof(valueAccessor));

            T value;
            if (_needsSync)
            {
                value = await GetAsync();
            }
            else
            {
                value = _value;
            }

            return valueAccessor(value);
        }

        public async Task<T> SetAsync(T value)
        {
            await InternalSetAsync(value);
            return _value;
        }

        public async Task<bool> UpdateAsync(Func<T, T> updateAction)
        {
            Args.NotNull(updateAction, nameof(updateAction));

            T oldValue = await GetAsync();

            var newValue = updateAction(oldValue);

            return await InternalSetAsync(newValue);
        }

        internal async Task<bool> InternalSetAsync(T value)
        {
            if (_needsSync || !_equalityComparer.Equals(_value, value))
            {
                await StateManager.SetStateAsync(Key, value);

                var oldValue = _value;
                _value = value;
                _needsSync = false;
                if (_onChange != null)
                {
                    await _onChange(oldValue, value);
                }
                return true;
            }
            return false;
        }

        public async Task DeleteStateAsync()
        {
            if (await StateManager.TryRemoveStateAsync(Key))
            {
                _needsSync = true;
                _value = default(T);
            }
        }

        public void Refresh()
        {
            _needsSync = true;
        }

        public async Task<bool> CreateSnapshot(string snapshotName)
        {
            Args.NotNullOrEmpty(snapshotName, nameof(snapshotName));

            if (await StateManager.ContainsStateAsync(Key))
            {
                var value = await GetAsync();
                await StateManager.SetStateAsync($"{snapshotName}:{Key}", value);
            }
            else
            {
                await StateManager.SetStateAsync($"{snapshotName}:{Key}:_empty", true);
            }

            return true;
        }

        public async Task<bool> RestoreSnapshot(string snapshotName)
        {
            Args.NotNullOrEmpty(snapshotName, nameof(snapshotName));

            var snapshotKey = $"{snapshotName}:{Key}";
            if (await StateManager.ContainsStateAsync($"{snapshotKey}:_empty"))
            {
                await DeleteStateAsync();
            }
            else if (await StateManager.ContainsStateAsync(snapshotKey))
            {
                var value = await StateManager.GetStateAsync<T>(snapshotKey);
                await SetAsync(value);
            }
            else
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteSnapshot(string snapshotName)
        {
            Args.NotNullOrEmpty(snapshotName, nameof(snapshotName));

            var snapshotKey = $"{snapshotName}:{Key}";
            return await StateManager.TryRemoveStateAsync($"{snapshotKey}:_empty") ||
                   await StateManager.TryRemoveStateAsync(snapshotKey);
        }
    }
}
