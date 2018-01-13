namespace Microsoft.ServiceFabric.Actors.Client
{
    using System;
    using Microsoft.ServiceFabric.Actors;

    public static class ActorProxyFactoryExtensions
    {
        public static TActorInterface CreateActorProxy<TActorInterface>(this IActorProxyFactory actorProxyFactory, ActorReference actorReference)
            where TActorInterface : IActor
        {
            if (actorProxyFactory == null) throw new ArgumentNullException(nameof(actorProxyFactory));
            if (actorReference == null) throw new ArgumentNullException(nameof(actorReference));

            return actorProxyFactory.CreateActorProxy<TActorInterface>(actorReference.ServiceUri, actorReference.ActorId, actorReference.ListenerName);
        }

        public static TTargetActorInterface Rebind<TTargetActorInterface>(this IActorProxyFactory actorProxyFactory, IActor actor)
            where TTargetActorInterface : IActor
        {
            if (actor == null) throw new ArgumentNullException(nameof(actor));
            if (actorProxyFactory == null) throw new ArgumentNullException(nameof(actorProxyFactory));

            var actorReference = actor.GetActorReference();
            return actorProxyFactory.CreateActorProxy<TTargetActorInterface>(actorReference.ServiceUri, actorReference.ActorId, actorReference.ListenerName);
        }
    }
}
