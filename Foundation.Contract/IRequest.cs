namespace Foundation
{
    using System;

    /// <summary>
    /// Common interface for wellknown properties of a Request
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Gets or sets a <see cref="Guid"/> identifier for this request to be uniquely identified as part of a wider operation set.
        /// </summary>
        /// <remarks>Optional</remarks>
        Guid? RequestId { get; set; }
    }
}