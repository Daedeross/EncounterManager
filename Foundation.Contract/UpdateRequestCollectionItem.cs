namespace Foundation
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Generic implementation of an Update Request
    /// </summary>
    /// <typeparam name="TUpdateRequest"></typeparam>
    [DataContract(Name = "UpdateRequestCollectionItemOf{0}")]
    public class UpdateRequestCollectionItem<TUpdateRequest>
    {
        /// <summary>
        /// The identity of the existing actor to update.
        /// </summary>
        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public Identity Identity { get; set; }

        /// <summary>
        /// The update request
        /// </summary>
        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public TUpdateRequest Request { get; set; }
    }
}
