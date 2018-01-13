namespace Foundation.ServiceFabric
{
    using System.Collections.Generic;
    using Microsoft.ServiceFabric.Actors;

    public class ActorStateEqualityComparer<TActor> : IEqualityComparer<ActorState<TActor>>
        where TActor : IActor
    {
        public bool Equals(ActorState<TActor> x, ActorState<TActor> y)
        {
            return ReferenceEquals(x, y) || ActorReferenceEqualityComparer.Default.Equals(x?.Actor?.GetActorReference(), y?.Actor?.GetActorReference());
        }

        public int GetHashCode(ActorState<TActor> obj)
        {
            return obj?.GetHashCode() ?? 0;
        }
    }
}