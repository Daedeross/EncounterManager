namespace Foundation.ServiceFabric
{
    using System.Threading.Tasks;

    public interface IStateContainer<T>
    {
        Task<bool> HasStateAsync();

        Task<bool> IsEmptyAsync();

        Task DeleteStateAsync();
    }
}
