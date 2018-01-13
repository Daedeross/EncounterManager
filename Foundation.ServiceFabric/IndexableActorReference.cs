namespace Foundation.ServiceFabric
{
    using System.Runtime.Serialization;
    using System.Text;
    using Microsoft.ServiceFabric.Actors;

    [DataContract(Name = "IndexableActorReference")]
    public sealed class IndexableActorReference : Indexable<IndexableActorReference, ActorReference>
    {
        public IndexableActorReference(ActorReference actorReference)
            : base(actorReference)
        {
        }

        protected override string BuildRepresentation()
        {
            return new StringBuilder()
                .Append(Reference.ServiceUri)
                .Append(';')
                .Append(Reference.ActorId.Kind)
                .Append(';')
                .Append(Reference.ActorId)
                .Append(';')
                .Append(Reference.ListenerName)
                .ToString();
        }
    }
}