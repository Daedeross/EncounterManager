namespace Foundation.ServiceFabric
{
    using System.Collections.Generic;
    using Foundation.Utilities;
    using Microsoft.ServiceFabric.Actors;

    public class ActorReferenceEqualityComparer : IEqualityComparer<ActorReference>
    {
        public static readonly ActorReferenceEqualityComparer Default  = new ActorReferenceEqualityComparer();

        public bool Equals(ActorReference x, ActorReference y)
        {
            return (ReferenceEquals(x, null) && ReferenceEquals(y, null))
                   || (!ReferenceEquals(x, null) && !ReferenceEquals(y, null)
                       && x.ActorId == y.ActorId
                       && x.ServiceUri == y.ServiceUri
                       && x.ListenerName == y.ListenerName);
        }

        public int GetHashCode(ActorReference obj)
        {
            if (obj == null) return 0;
            var hash = new FNV1aHash();

            hash.Step(
                obj.ServiceUri.GetHashCode(),
                obj.ActorId.GetHashCode(),
                (obj.ListenerName?.GetHashCode()).GetValueOrDefault());

            return hash.Value;
        }
    }
}
