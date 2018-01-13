namespace Foundation.ServiceFabric
{
    using System;
    using Microsoft.ServiceFabric.Actors;

    public static class ActorExtensions
    {
        public static TTargetActorInterface Rebind<TTargetActorInterface>(this IServiceFabricToolbox toolbox, IActor actor)
            where TTargetActorInterface : IActor
        {
            if (toolbox == null) throw new ArgumentNullException(nameof(toolbox));
            if (actor == null) throw new ArgumentNullException(nameof(actor));

            var actorReference = actor.GetActorReference();
            return toolbox.Actors.CreateActorProxy<TTargetActorInterface>(actorReference.ServiceUri, actorReference.ActorId, actorReference.ListenerName);
        }

        public static TTargetActorInterface Bind<TTargetActorInterface>(this IServiceFabricToolbox toolbox, ActorReference actorReference)
            where TTargetActorInterface : IActor
        {
            if (toolbox == null) throw new ArgumentNullException(nameof(toolbox));
            if (actorReference == null) throw new ArgumentNullException(nameof(actorReference));

            return toolbox.Actors.CreateActorProxy<TTargetActorInterface>(actorReference.ServiceUri, actorReference.ActorId, actorReference.ListenerName);
        }
    }
}
