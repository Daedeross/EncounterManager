namespace Foundation.ServiceFabric
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Foundation.Utilities;
    using Microsoft.ServiceFabric.Actors.Runtime;

    public class StateCollection<T> : IStateCollection<T>
    {
        private List<T> _value;
        private bool _needsSync = true;
        private readonly Func<Task<List<T>>> _factory;
        private readonly IEqualityComparer<T> _equalityComparer;
        private readonly Func<T, Task> _onAdd;
        private readonly Func<T, Task> _onRemove;

        public string Key { get; private set; }
        protected IActorStateManager StateManager { get; private set; }

        public StateCollection(IActorStateManager stateManager, string key, Func<Task<List<T>>> asyncFactory, IEqualityComparer<T> equalityComparer, Func<T, Task> onAdd = null, Func<T, Task> onRemove = null)
        {
            Args.NotNull(stateManager, nameof(stateManager));
            Args.NotNullOrEmpty(key, nameof(key));
            Args.NotNull(equalityComparer, nameof(equalityComparer));

            StateManager = stateManager;
            Key = key;
            _factory = asyncFactory;
            _equalityComparer = equalityComparer;
            _onAdd = onAdd;
            _onRemove = onRemove;
        }

        public Task<bool> HasStateAsync()
        {
            return StateManager.ContainsStateAsync(Key);
        }

        public async Task<bool> IsEmptyAsync()
        {
            return !await StateManager.ContainsStateAsync(Key) || await GetAsync() == null;
        }

        public async Task<bool> ContainsAsync(T value)
        {
            var list = await GetAsync();
            return list.Contains(value, _equalityComparer);
        }

        public async Task<int> CountAsync()
        {
            var list = await GetAsync();
            return list.Count;
        }

        public async Task DeleteStateAsync()
        {
            if (await StateManager.TryRemoveStateAsync(Key))
            {
                _needsSync = true;
                _value = null;
            }
        }

        public async Task<bool> AddAsync(T value)
        {
            var list = await GetAsync();
            if (!list.Contains(value, _equalityComparer))
            {
                list.Add(value);
                await SetAsync(list);
                if (_onAdd != null)
                {
                    await _onAdd(value);
                }
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<T>>  AddRangeAsync(IEnumerable<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var list = await GetAsync();
            var incoming = values.Except(list, _equalityComparer).ToList();
            if (incoming.Count > 0)
            {
                list.AddRange(incoming);
                await SetAsync(list);
                if (_onAdd != null)
                {
                    foreach (var v in incoming)
                    {
                        await _onAdd(v);
                    }
                }
            }
            return incoming;
        }

        public async Task<List<T>> GetAsync()
        {
            if (_needsSync)
            {
                var recordResult = await StateManager.TryGetStateAsync<List<T>>(Key);
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

        public async Task<T> GetAsync(int i)
        {
            var values = await GetAsync();
            return values.Count > i ? values[i] : default(T);
        }

        public async Task<T> FindAsync(Predicate<T> match)
        {
            var list = await GetAsync();

            return list.Find(match);
        }

        public async Task<List<T>> SetAsync(List<T> values)
        {
            if (await HasStateAsync() && _onRemove != null)
            {
                var old = await GetAsync();
                foreach (var o in old)
                {
                    await _onRemove(o);
                }
            }

            await StateManager.SetStateAsync(Key, values);
            _value = values;
            _needsSync = false;

            if (_onAdd != null)
            {
                foreach (var v in values)
                {
                    await _onAdd(v);
                }
            }

            return values;
        }

        public async Task<List<TResult>> ReadAsync<TResult>(Func<T, TResult> valueAccessor)
        {
            if (valueAccessor == null)
            {
                throw new ArgumentNullException(nameof(valueAccessor));
            }

            Args.NotNull(valueAccessor, nameof(valueAccessor));

            List<T> value;
            if (_needsSync)
            {
                value = await GetAsync();
            }
            else
            {
                value = _value;
            }

            return value.Select(valueAccessor).ToList();
        }

        public async Task<bool> UpdateAsync(T target, Action<T> modify)
        {
            if (modify == null)
            {
                throw new ArgumentNullException(nameof(modify));
            }

            var list = await GetAsync();

            var value = list.Find(v => _equalityComparer.Equals(v, target));
            if (Equals(value, default(T))) return false;

            modify(value);
            await SetAsync(list);
            return true;
        }

        public async Task<bool> UpdateAsync(Action<List<T>> modify)
        {
            if (modify == null)
            {
                throw new ArgumentNullException(nameof(modify));
            }

            var list = await GetAsync();

            modify(list);
            await SetAsync(list);
            return true;
        }

        public async Task<bool> RemoveAtAsync(int index)
        {
            if (!await HasStateAsync()) return false;
            if (index < 0) return false;

            var list = await GetAsync();
            if (index > list.Count) return false;

            var value = list[index];

            list.RemoveAt(index);

            if (list.Count == 0)
            {
                await DeleteStateAsync();
            }
            else
            {
                await SetAsync(list);
            }
            if (_onRemove != null)
            {
                await _onRemove(value);
            }
            return true;
        }

        public async Task<bool> RemoveAsync(T value)
        {
            if (!await HasStateAsync()) return false;

            var list = await GetAsync();
            var index = list.FindIndex(v => _equalityComparer.Equals(value, v));
            if (index < 0) return false;

            list.RemoveAt(index);

            if (list.Count == 0)
            {
                await DeleteStateAsync();
            }
            else
            {
                await SetAsync(list);
            }
            if (_onRemove != null)
            {
                await _onRemove(value);
            }
            return true;
        }

        /// <summary>
        /// Remove multiple values from this StateCollection
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns><see cref="IEnumerable{T}"/> of removed values</returns>
        public async Task<IEnumerable<T>> RemoveRangeAsync(IEnumerable<T> values)
        {
            if (!await HasStateAsync()) return new T[0];

            var list = await GetAsync();
            var removed = new List<T>();
            foreach (var remove in values)
            {
                var index = list.FindIndex(v => _equalityComparer.Equals(remove, v));
                if (index < 0) continue;

                list.RemoveAt(index);
                removed.Add(remove);
            }
            if (removed.Count > 0)
            {
                await SetAsync(list);
                if (_onRemove != null)
                {
                    foreach (var r in removed)
                    {
                        await _onRemove(r);
                    }
                }
            }
            return removed;
        }

        #region ISnapshotContainer

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
                var value = await StateManager.GetStateAsync<List<T>>(snapshotKey);
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

        #endregion ISnapshotContainer
    }
}
