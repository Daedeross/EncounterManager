namespace EncounterManager.Users
{
    using EncounterManager.Users.Model;
    using Foundation;
    using Foundation.ServiceFabric;
    using Foundation.Utilities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.ServiceFabric.Actors;
    using Microsoft.ServiceFabric.Actors.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [ActorService(Name = "UserLoader")]
    public class UserLoader : CoreActorBase, IUserLoader
    {
        private IServiceFabricToolbox _toolbox;

        protected UserLoader(ActorService actorService, ActorId actorId, IServiceFabricToolbox toolbox)
            : base(actorService, actorId)
        {
            Args.NotNull(toolbox, nameof(toolbox));
            _toolbox = toolbox;
        }

        public Task<IdentityResult> CreateUserAsync(UserRecord user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteUserAsync(UserRecord user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
