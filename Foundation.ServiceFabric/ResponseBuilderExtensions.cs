namespace Foundation.ServiceFabric
{
    public static class ResponseBuilderExtensions
    {
        /// <summary>
        /// Builds a <see cref="Response{T}"/> from the source <see cref="IResponseBuilder"/>.
        /// </summary>
        /// <typeparam name="T">Type of the response value</typeparam>
        /// <param name="builder"><see cref="IResponseBuilder"/></param>
        /// <returns><see cref="Response{T}"/></returns>
        public static Response<T> Build<T>(this IResponseBuilder builder)
        {
            return (Response<T>) builder.Build();
        }

        /// <summary>
        /// Builds a <see cref="QueryResponse{T}"/> from the source <see cref="IResponseBuilder"/>.
        /// </summary>
        /// <typeparam name="T">Type of the response results</typeparam>
        /// <param name="builder"><see cref="IResponseBuilder"/></param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        public static QueryResponse<T> BuildQuery<T>(this IResponseBuilder builder)
        {
            return (QueryResponse<T>) builder.Build();
        }
    }
}