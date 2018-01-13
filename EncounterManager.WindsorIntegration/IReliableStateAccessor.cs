namespace EncounterManager.Services
{
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Data;

    public interface IReliableStateAccessor
    {
        IReliableStateManager StateManager { get; }

        Task<T> Get<T>(string name) where T : IReliableState;
    }
}