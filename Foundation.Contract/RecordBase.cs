namespace Foundation
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The base abstract for a Record
    /// </summary>
    /// <seealso cref="Foundation.IRecord" />
    [DataContract]
    public abstract class RecordBase : IRecord
    {
        /// <summary>
        /// Gets or sets the <see cref="int" /> id for this record.
        /// </summary>
        /// <value>The identifier.</value>
        /// <remarks>Required</remarks>
        [DataMember(Order = 0, IsRequired = true, EmitDefaultValue = true)]
        public Identity Id { get; set; }
    }
}
