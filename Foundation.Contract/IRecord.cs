namespace Foundation
{
    /// <summary>
    /// Common interface for wellknown properties of a Record
    /// </summary>
    public interface IRecord
    {
        /// <summary>
        /// Gets or sets the <see cref="Identity"/> for this record.
        /// </summary>
        /// <remarks>Required</remarks>
        Identity Id { get; set; }
    }
}