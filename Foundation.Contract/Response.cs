namespace Foundation
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Standard response payload
    /// </summary>
    [DataContract(Name = "Response")]
    public class Response
    {
        /// <summary>
        /// Gets or sets the state of the response.
        /// </summary>
        /// <remarks>Required</remarks>
        [Required]
        [DataMember(Name = "ResponseState", IsRequired = true, EmitDefaultValue = true, Order = 0)]
        public ResponseState ResponseState { get; set; }

        /// <summary>
        /// Gets or sets the message for this response.
        /// </summary>
        /// <remarks>Optional</remarks>
        [DataMember(Name = "Message", IsRequired = false, EmitDefaultValue = false, Order = 1)]
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new OK Response with no message
        /// </summary>
        public Response()
            : this(ResponseState.Ok, null)
        {
        }

        /// <summary>
        /// Initializes a new Response with the provided state and message
        /// </summary>
        /// <param name="state"><see cref="ResponseState"/></param>
        /// <param name="message">A <see cref="string"/> message or null</param>
        public Response(ResponseState state, string message)
        {
            ResponseState = state;
            Message = message;
        }

        /// <summary>
        /// Applies this response to the provided <see cref="IResponseVisitor"/>
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public virtual void Accept(IResponseVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
