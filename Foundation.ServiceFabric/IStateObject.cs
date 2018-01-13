namespace Foundation.ServiceFabric
{
    using System;
    using System.Threading.Tasks;

    public interface IStateObject<T> : IStateContainer<T>
    {
        Task<T> GetAsync();

        Task<T> SetAsync(T value);

        Task<TResult> ReadAsync<TResult>(Func<T, TResult> valueAccessor);
    }
}