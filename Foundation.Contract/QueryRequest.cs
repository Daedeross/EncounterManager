namespace Foundation
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Simple paging query args class to support common querying across the API
    /// </summary>
    [DataContract(Name = "QueryRequest")]
    public class QueryRequest : PagedRequest
    {
        /// <summary>
        /// The query string to search with
        /// </summary>
        /// <remarks>Required</remarks>
        [Required]
        [DataMember(Name = "Query", IsRequired = true, EmitDefaultValue = true, Order = 2)]
        public string Query { get; set; }
    }
}