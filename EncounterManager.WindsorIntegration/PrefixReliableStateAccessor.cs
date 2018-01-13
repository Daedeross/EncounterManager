namespace EncounterManager.Services
{
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Data;

    public class PrefixReliableStateAccessor : IReliableStateAccessor
    {
        private readonly string _prefix;

        public IReliableStateManager StateManager { get; }

        public PrefixReliableStateAccessor(IReliableStateManager stateManager, string prefix)
        {
            StateManager = stateManager;
            _prefix = prefix;
        }

        public Task<T> Get<T>(string name) where T : IReliableState
        {
            return StateManager.GetOrAddAsync<T>($"{_prefix}_{name}");
        }
    }
}