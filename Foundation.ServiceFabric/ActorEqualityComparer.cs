namespace Foundation.ServiceFabric
{
    using System.Collections.Generic;
    using Microsoft.ServiceFabric.Actors;

    public class ActorEqualityComparer<TActor> : IEqualityComparer<TActor>
        where TActor : IActor
    {
        public bool Equals(TActor x, TActor y)
        {
            return ReferenceEquals(x, y) || ActorReferenceEqualityComparer.Default.Equals(x?.GetActorReference(), y?.GetActorReference());
        }

        public int GetHashCode(TActor obj)
        {
            return ActorReferenceEqualityComparer.Default.GetHashCode(obj?.GetActorReference());
        }
    }
}
