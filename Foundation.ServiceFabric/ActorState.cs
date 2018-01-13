namespace Foundation.ServiceFabric
{
    using System;
    using System.Runtime.Serialization;
    using Microsoft.ServiceFabric.Actors;

    [DataContract]
    public class ActorState<TActorInterface> : IEquatable<ActorState<TActorInterface>>
        where TActorInterface : IActor
    {
        private ActorReference _reference;

        [DataMember(Name = "Actor", IsRequired = true, EmitDefaultValue = false)]
        private TActorInterface _actor;

        [IgnoreDataMember]
        public ActorReference Reference
        {
            get
            {
                if (_reference == null && _actor != null)
                {
                    _reference = _actor.GetActorReference();
                }
                return _reference;
            }
        }

        [IgnoreDataMember]
        public TActorInterface Actor
        {
            get
            {
                return _actor;
            }
            set
            {
                _actor = value;
                _reference = null;
            }
        }

        public bool Equals(ActorState<TActorInterface> other)
        {
            return other != null &&
                   (ReferenceEquals(this, other) || ActorReferenceEqualityComparer.Default.Equals(Reference, other.Reference));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ActorState<TActorInterface>);
        }

        public override int GetHashCode()
        {
            return ActorReferenceEqualityComparer.Default.GetHashCode(Reference);
        }
    }
}
