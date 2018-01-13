namespace Foundation.ServiceFabric
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStateCollection<T> : IStateContainer<List<T>>
    {
        Task<bool> ContainsAsync(T value);

        Task<int> CountAsync();

        Task<bool> AddAsync(T value);

        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> values);
        Task<List<T>> GetAsync();

        Task<T> GetAsync(int i);

        Task<List<TResult>> ReadAsync<TResult>(Func<T, TResult> valueAccessor);

        Task<T> FindAsync(Predicate<T> match);

        Task<List<T>> SetAsync(List<T> values);

        Task<bool> RemoveAtAsync(int index);

        Task<bool> RemoveAsync(T value);

        Task<IEnumerable<T>> RemoveRangeAsync(IEnumerable<T> values);
    }
}