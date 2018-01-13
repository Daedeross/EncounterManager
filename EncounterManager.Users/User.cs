namespace EncounterManager.Users
{
    using EncounterManager.Users.Model;
    using Foundation;
    using Foundation.ServiceFabric;
    using Microsoft.ServiceFabric.Actors;
    using Microsoft.ServiceFabric.Actors.Runtime;
    using System.Threading.Tasks;

    [StatePersistence(StatePersistence.Persisted)]
    [ActorService(Name = "User")]
    internal class User : CoreActorBase, IUser
    {
        private StateObject<string> _userName;
        private StateObject<string> _passwordHash;

        public User(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        protected async override Task OnActivateAsync()
        {
            await base.OnActivateAsync();

            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            _userName = StateFactory.Create<string>(StateManager, "userName");
            _passwordHash = StateFactory.Create<string>(StateManager, "passwordHash");
        }

        public Task<Identity> GetIdentityAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetNameAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetNormalizedNameAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<UserRecord> GetRecordAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
