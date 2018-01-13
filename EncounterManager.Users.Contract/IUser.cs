using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EncounterManager.Users.Model;
using EncounterManager.Users.Stores;
using Foundation;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

[assembly: FabricTransportActorRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace EncounterManager.Users
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IUser : IIdentityProvider
    {
        Task<UserRecord> GetRecordAsync();

        Task<string> GetNameAsync();

        Task<string> GetNormalizedNameAsync();

        Task<string> GetPasswordHashAsync();
    }
}
