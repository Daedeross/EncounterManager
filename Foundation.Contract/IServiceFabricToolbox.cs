namespace Foundation
{
    using Microsoft.ServiceFabric.Actors.Client;
    using Microsoft.ServiceFabric.Services.Remoting.Client;

    public interface IServiceFabricToolbox
    {
        IActorProxyFactory Actors { get; }

        IServiceProxyFactory Services { get; }

        ISettingsProvider Settings { get; }
    }
}