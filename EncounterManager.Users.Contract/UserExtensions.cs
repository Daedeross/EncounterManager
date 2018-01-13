namespace EncounterManager.Users
{
    using Foundation;
    using Microsoft.ServiceFabric.Actors;
    using Microsoft.ServiceFabric.Actors.Client;
    using Microsoft.ServiceFabric.Services.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class UserExtensions
    {
        public static IUser User(this IServiceFabricToolbox toolbox, ActorId actorId)
        {
            return toolbox.Actors.CreateActorProxy<IUser>(UserConstants.UserUri, actorId);
        }

        public static IUser User(this IServiceFabricToolbox toolbox, ActorReference actorReference)
        {
            return toolbox.Actors.CreateActorProxy<IUser>(actorReference);
        }

        public static IUserLoader UserLoader(this IServiceFabricToolbox toolbox)
        {
            return toolbox.Actors.CreateActorProxy<IUserLoader>(UserConstants.UserLoaderUri, ActorId.CreateRandom());
        }

        public static IUserRegistryService UserRegistry(this IServiceFabricToolbox toolbox)
        {
            return toolbox.Services.CreateServiceProxy<IUserRegistryService>(
                UserConstants.UserRegistryUri,
                ServicePartitionKey.Singleton);
        }

    }
}
