namespace Foundation.ServiceFabric
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Factory interface for creating Response payloads given different circumstances
    /// </summary>
    public interface IResponseFactory
    {
        /// <summary>
        /// Success response
        /// </summary>
        /// <returns>Response.</returns>
        Response Ok();
        /// <summary>
        /// Success response with a value payload
        /// </summary>
        /// <typeparam name="T">Data type to be returned in the response</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><see cref="Response{T}"/></returns>
        Response<T> Ok<T>(T value);
        /// <summary>
        /// Success response for a pageable collection of values
        /// </summary>
        /// <typeparam name="T">Data type of individual response value</typeparam>
        /// <param name="values">The values to return</param>
        /// <param name="offset">The offset index the return value start from</param>
        /// <param name="total">The total count of all possible return values. This could be the number returned if not paged, or the count of the entire set outside of paging</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        QueryResponse<T> OkQuery<T>(IEnumerable<T> values, long? offset = null, long? total = null);
        /// <summary>
        /// Resource created
        /// </summary>
        /// <returns>Response.</returns>
        Response Created();
        /// <summary>
        /// Resource created with included result
        /// </summary>
        /// <typeparam name="T">Data type of the returned value</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><see cref="Response{T}"/></returns>
        Response<T> Created<T>(T value);
        /// <summary>
        /// Multiple values were created
        /// </summary>
        /// <typeparam name="T">Data type of individual created resource</typeparam>
        /// <param name="values">The values.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        QueryResponse<T> CreatedQuery<T>(IEnumerable<T> values);
        /// <summary>
        /// Request was accepted
        /// </summary>
        /// <returns>Response.</returns>
        Response Accepted();
        /// <summary>
        /// Request was accepted with a response payload
        /// </summary>
        /// <typeparam name="T">Data type of response payload</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><see cref="Response{T}"/></returns>
        Response<T> Accepted<T>(T value);
        /// <summary>
        /// Request was accepted and multiple response payloads are available
        /// </summary>
        /// <typeparam name="T">Data type of individual response payload</typeparam>
        /// <param name="values">The values.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        QueryResponse<T> AcceptedQuery<T>(IEnumerable<T> values);
        /// <summary>
        /// Request is bad or rejected
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Response.</returns>
        Response BadRequest(string message = null);
        /// <summary>
        /// Request is bad or rejected
        /// </summary>
        /// <typeparam name="T">Data type of response payload</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="Response{T}"/></returns>
        Response<T> BadRequest<T>(string message = null);
        /// <summary>
        /// Request is bad or rejected with a response payload
        /// </summary>
        /// <typeparam name="T">Data type of response payload</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><see cref="Response{T}"/></returns>
        Response<T> BadRequest<T>(T value);
        /// <summary>
        /// Query request is bad or rejected
        /// </summary>
        /// <typeparam name="T">Data type of individual response payload</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        QueryResponse<T> BadQuery<T>(string message = null);
        /// <summary>
        /// Request is not allowed
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Response.</returns>
        Response Forbidden(string message = null);
        /// <summary>
        /// Request is not allowed
        /// </summary>
        /// <typeparam name="T">Data type of payload</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="Response{T}"/></returns>
        Response<T> Forbidden<T>(string message = null);
        /// <summary>
        /// Request is not allowed with a resulting payload
        /// </summary>
        /// <typeparam name="T">Data type of payload</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><see cref="Response{T}"/></returns>
        Response<T> Forbidden<T>(T value);
        /// <summary>
        /// Query request is not allowed
        /// </summary>
        /// <typeparam name="T">Data type of individual payload</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        QueryResponse<T> ForbiddenQuery<T>(string message = null);
        /// <summary>
        /// Requested resource could not be found
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Response.</returns>
        Response NotFound(string message = null);
        /// <summary>
        /// Requested resource could not be found
        /// </summary>
        /// <typeparam name="T">Data type of resource</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="Response{T}"/></returns>
        Response<T> NotFound<T>(string message = null);
        /// <summary>
        /// Nothing could be found for resource query
        /// </summary>
        /// <typeparam name="T">Data type of individual resource</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        QueryResponse<T> NotFoundQuery<T>(string message = null);
        /// <summary>
        /// Target resource of request already exists or desired request could not be achieved because of collision
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Response.</returns>
        Response Conflict(string message = null);
        /// <summary>
        /// Target resource of request already exists or desired request could not be achieved because of collision
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="Response{T}"/></returns>
        Response<T> Conflict<T>(string message = null);
        /// <summary>
        /// Target resource of request already exists or desired request could not be achieved because of collision.
        /// Includes existing resource as response value.
        /// </summary>
        /// <typeparam name="T">Data type of resource</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><see cref="Response{T}"/></returns>
        Response<T> Conflict<T>(T value);
        /// <summary>
        /// Target resource of request already exists or desired request could not be achieved because of collision
        /// </summary>
        /// <typeparam name="T">Data type of individual resource</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        QueryResponse<T> ConflictQuery<T>(string message = null);
        /// <summary>
        /// Request caused an error
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Response.</returns>
        Response Error(string message = null);
        /// <summary>
        /// Request caused an error with an exception
        /// </summary>
        /// <param name="exception">The Exception</param>
        /// <returns>Response.</returns>
        Response Error(Exception exception);
        /// <summary>
        /// Request caused an error
        /// </summary>
        /// <typeparam name="T">Data type of expected payload</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="Response{T}"/></returns>
        Response<T> Error<T>(string message = null);
        /// <summary>
        /// Request caused an error with an exception
        /// </summary>
        /// <typeparam name="T">Data type of expected payload</typeparam>
        /// <param name="exception">The Exception</param>
        /// <returns><see cref="Response{T}"/></returns>
        Response<T> Error<T>(Exception exception);
        /// <summary>
        /// Query request caused an error
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        QueryResponse<T> ErrorQuery<T>(string message = null);
        /// <summary>
        /// Query request caused an error with an exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception">The Exception</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        QueryResponse<T> ErrorQuery<T>(Exception exception);
    }
}