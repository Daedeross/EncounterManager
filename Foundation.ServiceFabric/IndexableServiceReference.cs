namespace Foundation.ServiceFabric
{
    using System.Runtime.Serialization;
    using System.Text;

    [DataContract(Name = "IndexableServiceReference")]
    public sealed class IndexableServiceReference : Indexable<IndexableServiceReference, ServiceReference>
    {
        public IndexableServiceReference(ServiceReference serviceReference)
            : base(serviceReference)
        {
        }

        protected override string BuildRepresentation()
        {
            return new StringBuilder()
                .Append(Reference.ServiceUri)
                .Append(';')
                .Append(Reference.PartitionKind)
                .Append(';')
                .Append(Reference.PartitionName)
                .Append(';')
                .Append(Reference.PartitionGuid)
                .Append(';')
                .Append(Reference.PartitionId)
                .ToString();
        }
    }
}