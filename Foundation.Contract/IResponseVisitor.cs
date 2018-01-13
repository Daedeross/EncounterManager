namespace Foundation
{
    /// <summary>
    /// Simple visitor pattern interface for handling different types of response generation
    /// </summary>
    public interface IResponseVisitor
    {
        /// <summary>
        /// Visit a basic response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns><see cref="Response"/></returns>
        Response Visit(Response response);
        /// <summary>
        /// Visit a value response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <returns><see cref="Response{T}"/></returns>
        Response<T> Visit<T>(Response<T> response);
        /// <summary>
        /// Visit a query response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        QueryResponse<T> Visit<T>(QueryResponse<T> response);
    }
}