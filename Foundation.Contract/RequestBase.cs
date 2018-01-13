namespace Foundation
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Base abstract implementation of a Request
    /// </summary>
    /// <seealso cref="Foundation.IRequest" />
    [DataContract]
    public abstract class RequestBase : IRequest
    {
        /// <summary>
        /// Gets or sets a <see cref="Guid" /> identifier for this request to be uniquely identified as part of a wider operation set.
        /// </summary>
        /// <value>The request identifier.</value>
        /// <remarks>Optional</remarks>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public Guid? RequestId { get; set; }
    }
}