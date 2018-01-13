namespace Foundation
{
    using System.Runtime.Serialization;

    /// <summary>
    /// A value Response
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    /// <seealso cref="Foundation.Response" />
    [DataContract(Name = "{0}Response")]
    public class Response<T> : Response
    {
        /// <summary>
        /// Gets or sets the value of this response.
        /// </summary>
        [DataMember(Name = "Value", IsRequired = true, EmitDefaultValue = true, Order = 2)]
        public T Value { get; set; }

        /// <summary>
        /// Initializes a new OK Response with no message or value
        /// </summary>
        public Response()
            : this(ResponseState.Ok, null, default(T))
        {
        }

        /// <summary>
        /// Initializes a new OK Response with no message and a provided value
        /// </summary>
        public Response(T value)
            : this(ResponseState.Ok, null, value)
        {
        }

        /// <summary>
        /// Initializes a new Response with the provided state and message, with no value
        /// </summary>
        /// <param name="state"><see cref="ResponseState" /></param>
        /// <param name="message">A <see cref="string" /> message or null</param>
        public Response(ResponseState state, string message)
            : base(state, message)
        {
        }

        /// <summary>
        /// Initializes a new Response with provided state, message and value
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="message">The message.</param>
        /// <param name="value">The value.</param>
        public Response(ResponseState state, string message, T value)
            : base(state, message)
        {
            Value = value;
        }

        /// <summary>
        /// Applies this response to the provided <see cref="IResponseVisitor" />
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IResponseVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
