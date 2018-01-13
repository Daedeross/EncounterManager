namespace Foundation
{
    /// <summary>
    /// ResponseState is tied to some standard HTTP Status Codes, but not all of them.
    /// Values are intended to fulfill the same role as the HTTP status code definitions: https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html
    /// </summary>
    public enum ResponseState
    {
        /// <summary>
        /// Request was successful
        /// </summary>
        Ok = 200,
        /// <summary>
        /// Request was successful and a resource was created
        /// </summary>
        Created = 201,
        /// <summary>
        /// Request was received and pending completion
        /// </summary>
        Accepted = 202,
        /// <summary>
        /// Request was received but not valid or not understood
        /// </summary>
        BadRequest = 400,
        /// <summary>
        /// Request is not allowed
        /// </summary>
        Forbidden = 403,
        /// <summary>
        /// Resource does not exist
        /// </summary>
        NotFound = 404,
        /// <summary>
        /// Resource already exists
        /// </summary>
        Conflict = 409,
        /// <summary>
        /// Unspecified or generic error
        /// </summary>
        Error = 500,
    }
}
