namespace Foundation
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Simple paging request class to support common paging across the API
    /// </summary>
    [DataContract(Name = "PagedRequest")]
    public class PagedRequest
    {
        /// <summary>
        /// The starting index for the paged request
        /// </summary>
        [DataMember(Name = "Start", IsRequired = false, EmitDefaultValue = false, Order = 0)]
        public int Start { get; set; }

        /// <summary>
        /// The number of results desired for the paged request
        /// </summary>
        [DataMember(Name = "PageSize", IsRequired = false, EmitDefaultValue = false, Order = 1)]
        public int? PageSize { get; set; }
    }
}