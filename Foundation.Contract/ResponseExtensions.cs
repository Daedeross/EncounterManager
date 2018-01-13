namespace Foundation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class ResponseExtensions
    {
        /// <summary>
        /// Utility method that returns the value of a <see cref="Response{T}"/> if successful, otherwise throws a <see cref="ResponseException"/>
        /// with the response message as the exception message.
        /// </summary>
        /// <typeparam name="T">Value type contained in the response instance</typeparam>
        /// <param name="response"><see cref="Response{T}"/></param>
        /// <returns></returns>
        /// <exception cref="ResponseException">Thrown if the response resulted in anything other than success</exception>
        public static T GetValueOrThrow<T>(this Response<T> response)
        {
            if (Fail(response))
            {
                throw new ResponseException(response.Message);
            }

            return response.Value;
        }

        /// <summary>
        /// Utility method that awaits value of a <see cref="Response{T}"/> and returns it if successful, otherwise throws a <see cref="ResponseException"/>
        /// with the response message as the exception message.
        /// </summary>
        /// <typeparam name="T">Value type contained in the response instance</typeparam>
        /// <param name="task"><see cref="Task{TResult}"/> of type <see cref="Response{T}"/></param>
        /// <returns><see cref="Task{TResult}"/> of type <typeparamref name="T"/></returns>
        /// <exception cref="AggregateException">Thrown if the response resulted in anything other than success.
        /// Since this is a task the nested exception will be of type <see cref="ResponseException"/> inside
        /// the <see cref="AggregateException.InnerExceptions"/> property</exception>
        public static async Task<T> GetValueOrThrow<T>(this Task<Response<T>> task)
        {
            return (await task).GetValueOrThrow();
        }

        /// <summary>
        /// Utility method that returns the results of a <see cref="QueryResponse{T}"/> if successful, otherwise throws <see cref="ResponseException"/>
        /// with the response message as the exception message.
        /// </summary>
        /// <typeparam name="T">Type of the results</typeparam>
        /// <param name="response"><see cref="QueryResponse{T}"/></param>
        /// <returns><see cref="IEnumerable{T}"/> of <typeparamref name="T"/></returns>
        /// <exception cref="ResponseException"></exception>
        public static IEnumerable<T> GetResultsOrThrow<T>(this QueryResponse<T> response)
        {
            if (Fail(response))
            {
                throw new ResponseException(response.Message);
            }
            return response.Results;
        }

        /// <summary>
        /// Utility method that awaits value of a <see cref="QueryResponse{T}"/> and returns it if successful, otherwise throws a <see cref="ResponseException"/>
        /// with the response message as the exception message.
        /// </summary>
        /// <typeparam name="T">Type of the results</typeparam>
        /// <param name="task"><see cref="Task{TResult}"/> of type <see cref="QueryResponse{T}"/></param>
        /// <returns><see cref="Task{TResult}"/> of type <see cref="IEnumerable{T}"/></returns>
        /// <exception cref="AggregateException">Thrown if the response resulted in anything other than success.
        /// Since this is a task the nested exception will be of type <see cref="ResponseException"/> inside
        /// the <see cref="AggregateException.InnerExceptions"/> property</exception>
        public static async Task<IEnumerable<T>> GetResultsOrThrow<T>(this Task<QueryResponse<T>> task)
        {
            return (await task).GetResultsOrThrow();
        }

        /// <summary>
        /// Test the <see cref="Response"/> for success
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>System.Boolean.</returns>
        public static bool Success(this Response response)
        {
            return response.ResponseState == ResponseState.Ok
                || response.ResponseState == ResponseState.Created
                || response.ResponseState == ResponseState.Accepted;
        }

        /// <summary>
        /// Test the <see cref="Response"/> for failure
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>System.Boolean.</returns>
        public static bool Fail(this Response response)
        {
            return !(response.ResponseState == ResponseState.Ok
                || response.ResponseState == ResponseState.Created
                || response.ResponseState == ResponseState.Accepted);
        }
    }
}