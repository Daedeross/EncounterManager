namespace Foundation.ServiceFabric
{
    using System.Diagnostics;
    using Foundation.Utilities;
    using Microsoft.ServiceFabric.Actors;
    using Microsoft.ServiceFabric.Actors.Runtime;

    public abstract class CoreActorBase : Actor
    {
        protected CoreActorBase(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
            Args.NotNull(actorService, nameof(actorService));
            Args.NotNull(actorId, nameof(actorId));
        }

        [Conditional("DEBUG")]
        public void ActivateInternal()
        {
            OnActivateAsync().GetAwaiter().GetResult();
        }

        [Conditional("DEBUG")]
        public void DeactivateInternal()
        {
            OnDeactivateAsync().GetAwaiter().GetResult();
        }
    }
}
