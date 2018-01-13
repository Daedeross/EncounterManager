namespace Foundation.ServiceFabric
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ResponseFactory is a default implementation of IResponseFactory that provides static versions of the methods
    /// for shorthand access and standardization
    /// </summary>
    /// <seealso cref="IResponseFactory" />
    public sealed class ResponseFactory : IResponseFactory
    {
        public static readonly IResponseFactory Default = new ResponseFactory();

        #region Ok

        /// <summary>
        /// Success response
        /// </summary>
        /// <returns>Response.</returns>
        public static Response Ok()
        {
            return Default.Ok();
        }

        /// <summary>
        /// Success response with a value payload
        /// </summary>
        /// <typeparam name="T">Data type to be returned in the response</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><see cref="Response{T}"/></returns>
        public static Response<T> Ok<T>(T value)
        {
            return Default.Ok(value);
        }

        /// <summary>
        /// Success response for a pageable collection of values
        /// </summary>
        /// <typeparam name="T">Data type of individual response value</typeparam>
        /// <param name="values">The values to return</param>
        /// <param name="offset">The offset index the return value start from</param>
        /// <param name="total">The total count of all possible return values. This could be the number returned if not paged, or the count of the entire set outside of paging</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        public static QueryResponse<T> OkQuery<T>(IEnumerable<T> values, long? offset = null, long? total = null)
        {
            return Default.OkQuery(values, offset, total);
        }

        #endregion Ok
        #region Created

        /// <summary>
        /// Resource created
        /// </summary>
        /// <returns>Response.</returns>
        public static Response Created()
        {
            return Default.Created();
        }

        /// <summary>
        /// Resource created with included result
        /// </summary>
        /// <typeparam name="T">Data type of the returned value</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><see cref="Response{T}"/></returns>
        public static Response<T> Created<T>(T value)
        {
            return Default.Created(value);
        }

        /// <summary>
        /// Multiple values were created
        /// </summary>
        /// <typeparam name="T">Data type of individual created resource</typeparam>
        /// <param name="values">The values.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        public static QueryResponse<T> CreatedQuery<T>(IEnumerable<T> values)
        {
            return Default.CreatedQuery(values);
        }

        #endregion Created
        #region Accepted

        /// <summary>
        /// Request was accepted
        /// </summary>
        /// <returns>Response.</returns>
        public static Response Accepted()
        {
            return Default.Accepted();
        }

        /// <summary>
        /// Request was accepted with a response payload
        /// </summary>
        /// <typeparam name="T">Data type of response payload</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><see cref="Response{T}"/></returns>
        public static Response<T> Accepted<T>(T value)
        {
            return Default.Accepted(value);
        }

        /// <summary>
        /// Request was accepted and multiple response payloads are available
        /// </summary>
        /// <typeparam name="T">Data type of individual response payload</typeparam>
        /// <param name="values">The values.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        public static QueryResponse<T> Accepted<T>(IEnumerable<T> values)
        {
            return Default.AcceptedQuery(values);
        }

        #endregion Accepted
        #region BadRequest

        /// <summary>
        /// Request is bad or rejected
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Response.</returns>
        public static Response BadRequest(string message = null)
        {
            return Default.BadRequest(message);
        }

        /// <summary>
        /// Request is bad or rejected
        /// </summary>
        /// <typeparam name="T">Data type of response payload</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="Response{T}"/></returns>
        public static Response<T> BadRequest<T>(string message = null)
        {
            return Default.BadRequest<T>(message);
        }

        /// <summary>
        /// Request is bad or rejected with a response payload
        /// </summary>
        /// <typeparam name="T">Data type of response payload</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><see cref="Response{T}"/></returns>
        public static Response<T> BadRequest<T>(T value)
        {
            return Default.BadRequest(value);
        }

        /// <summary>
        /// Query request is bad or rejected
        /// </summary>
        /// <typeparam name="T">Data type of individual response payload</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        public static QueryResponse<T> BadQuery<T>(string message = null)
        {
            return Default.BadQuery<T>(message);
        }

        #endregion BadRequest
        #region Forbidden

        /// <summary>
        /// Request is not allowed
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Response.</returns>
        public static Response Forbidden(string message = null)
        {
            return Default.Forbidden(message);
        }

        /// <summary>
        /// Request is not allowed
        /// </summary>
        /// <typeparam name="T">Data type of payload</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="Response{T}"/></returns>
        public static Response<T> Forbidden<T>(string message = null)
        {
            return Default.Forbidden<T>(message);
        }

        /// <summary>
        /// Request is not allowed with a resulting payload
        /// </summary>
        /// <typeparam name="T">Data type of payload</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><see cref="Response{T}"/></returns>
        public static Response<T> Forbidden<T>(T value)
        {
            return Default.Forbidden(value);
        }

        /// <summary>
        /// Query request is not allowed
        /// </summary>
        /// <typeparam name="T">Data type of individual payload</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        public static QueryResponse<T> ForbiddenQuery<T>(string message = null)
        {
            return Default.ForbiddenQuery<T>(message);
        }

        #endregion Forbidden
        #region NotFound

        /// <summary>
        /// Requested resource could not be found
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Response.</returns>
        public static Response NotFound(string message = null)
        {
            return Default.NotFound(message);
        }

        /// <summary>
        /// Requested resource could not be found
        /// </summary>
        /// <typeparam name="T">Data type of resource</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="Response{T}"/></returns>
        public static Response<T> NotFound<T>(string message = null)
        {
            return Default.NotFound<T>(message);
        }

        /// <summary>
        /// Nothing could be found for resource query
        /// </summary>
        /// <typeparam name="T">Data type of individual resource</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        public static QueryResponse<T> NotFoundQuery<T>(string message = null)
        {
            return Default.NotFoundQuery<T>(message);
        }

        #endregion NotFound
        #region Conflict

        /// <summary>
        /// Target resource of request already exists or desired request could not be achieved because of collision
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Response.</returns>
        public static Response Conflict(string message = null)
        {
            return Default.Conflict(message);
        }

        /// <summary>
        /// Target resource of request already exists or desired request could not be achieved because of collision
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="Response{T}"/></returns>
        public static Response<T> Conflict<T>(string message = null)
        {
            return Default.Conflict<T>(message);
        }

        /// <summary>
        /// Target resource of request already exists or desired request could not be achieved because of collision.
        /// Includes existing resource as response value.
        /// </summary>
        /// <typeparam name="T">Data type of resource</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><see cref="Response{T}"/></returns>
        public static Response<T> Conflict<T>(T value)
        {
            return Default.Conflict(value);
        }

        /// <summary>
        /// Target resource of request already exists or desired request could not be achieved because of collision
        /// </summary>
        /// <typeparam name="T">Data type of individual resource</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        public static QueryResponse<T> ConflictQuery<T>(string message = null)
        {
            return Default.ConflictQuery<T>(message);
        }

        #endregion Conflict
        #region Error

        /// <summary>
        /// Request caused an error
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Response.</returns>
        public static Response Error(string message = null)
        {
            return Default.Error(message);
        }

        /// <summary>
        /// Request caused an error with an Exception
        /// </summary>
        /// <param name="exception">The Exception.</param>
        /// <returns>Response.</returns>
        public static Response Error(Exception exception)
        {
            return Default.Error(exception);
        }

        /// <summary>
        /// Request caused an error
        /// </summary>
        /// <typeparam name="T">Data type of expected payload</typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="Response{T}"/></returns>
        public static Response<T> Error<T>(string message = null)
        {
            return Default.Error<T>(message);
        }

        /// <summary>
        /// Request caused an error
        /// </summary>
        /// <typeparam name="T">Data type of expected payload</typeparam>
        /// <param name="exception">The Exception.</param>
        /// <returns><see cref="Response{T}"/></returns>
        public static Response<T> Error<T>(Exception exception)
        {
            return Default.Error<T>(exception);
        }

        /// <summary>
        /// Query request caused an error
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        public static QueryResponse<T> ErrorQuery<T>(string message = null)
        {
            return Default.ErrorQuery<T>(message);
        }

        /// <summary>
        /// Query request caused an error
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception">The Exception.</param>
        /// <returns><see cref="QueryResponse{T}"/></returns>
        public static QueryResponse<T> ErrorQuery<T>(Exception exception)
        {
            return Default.ErrorQuery<T>(exception);
        }

        #endregion Error

        #region IResponseFactory
        #region Ok

        /// <inheritdoc />
        Response IResponseFactory.Ok()
        {
            return new ResponseBuilder()
                .State(ResponseState.Ok)
                .Build();
        }

        /// <inheritdoc />
        Response<T> IResponseFactory.Ok<T>(T value)
        {
            return new ResponseBuilder()
                .State(ResponseState.Ok)
                .Type(typeof(T)).Value(value)
                .Build<T>();
        }

        /// <inheritdoc />
        QueryResponse<T> IResponseFactory.OkQuery<T>(IEnumerable<T> values, long? offset, long? total)
        {
            return new ResponseBuilder()
                .State(ResponseState.Ok)
                .Type(typeof(T)).Value(values)
                .Query(true).Offset(offset).Total(total)
                .BuildQuery<T>();
        }

        #endregion Ok
        #region Created

        /// <inheritdoc />
        Response IResponseFactory.Created()
        {
            return new ResponseBuilder()
                .State(ResponseState.Created)
                .Build();
        }

        /// <inheritdoc />
        Response<T> IResponseFactory.Created<T>(T value)
        {
            return new ResponseBuilder()
                .State(ResponseState.Created)
                .Type(typeof(T)).Value(value)
                .Build<T>();
        }

        /// <inheritdoc />
        QueryResponse<T> IResponseFactory.CreatedQuery<T>(IEnumerable<T> values)
        {
            return new ResponseBuilder()
                .State(ResponseState.Created)
                .Type(typeof(T)).Value(values)
                .Query(true)
                .BuildQuery<T>();
        }

        #endregion Created
        #region Accepted

        /// <inheritdoc />
        Response IResponseFactory.Accepted()
        {
            return new ResponseBuilder()
                .State(ResponseState.Accepted)
                .Build();
        }

        /// <inheritdoc />
        Response<T> IResponseFactory.Accepted<T>(T value)
        {
            return new ResponseBuilder()
                .State(ResponseState.Accepted)
                .Type(typeof(T)).Value(value)
                .Build<T>();
        }

        /// <inheritdoc />
        QueryResponse<T> IResponseFactory.AcceptedQuery<T>(IEnumerable<T> values)
        {
            return new ResponseBuilder()
                .State(ResponseState.Accepted)
                .Type(typeof(T)).Value(values)
                .Query(true)
                .BuildQuery<T>();
        }

        #endregion Accepted
        #region BadRequest

        /// <inheritdoc />
        Response IResponseFactory.BadRequest(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.BadRequest)
                .Message(message)
                .Build();
        }

        /// <inheritdoc />
        Response<T> IResponseFactory.BadRequest<T>(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.BadRequest)
                .Type(typeof(T))
                .Message(message)
                .Build<T>();
        }

        /// <inheritdoc />
        Response<T> IResponseFactory.BadRequest<T>(T value)
        {
            return new ResponseBuilder()
                .State(ResponseState.BadRequest)
                .Type(typeof(T)).Value(value)
                .Build<T>();
        }

        /// <inheritdoc />
        QueryResponse<T> IResponseFactory.BadQuery<T>(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.BadRequest)
                .Type(typeof(T))
                .Message(message)
                .Query(true)
                .BuildQuery<T>();
        }

        #endregion BadRequest
        #region Forbidden

        /// <inheritdoc />
        Response IResponseFactory.Forbidden(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.Forbidden)
                .Message(message)
                .Build();
        }

        /// <inheritdoc />
        Response<T> IResponseFactory.Forbidden<T>(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.Forbidden)
                .Type(typeof(T))
                .Message(message)
                .Build<T>();
        }

        /// <inheritdoc />
        Response<T> IResponseFactory.Forbidden<T>(T value)
        {
            return new ResponseBuilder()
                .State(ResponseState.Forbidden)
                .Type(typeof(T)).Value(value)
                .Build<T>();
        }

        /// <inheritdoc />
        QueryResponse<T> IResponseFactory.ForbiddenQuery<T>(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.Forbidden)
                .Type(typeof(T))
                .Message(message)
                .Query(true)
                .BuildQuery<T>();
        }

        #endregion Forbidden
        #region NotFound

        /// <inheritdoc />
        Response IResponseFactory.NotFound(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.NotFound)
                .Message(message)
                .Build();
        }

        /// <inheritdoc />
        Response<T> IResponseFactory.NotFound<T>(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.NotFound)
                .Type(typeof(T))
                .Message(message)
                .Build<T>();
        }

        /// <inheritdoc />
        QueryResponse<T> IResponseFactory.NotFoundQuery<T>(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.NotFound)
                .Type(typeof(T))
                .Message(message)
                .Query(true)
                .BuildQuery<T>();
        }

        #endregion NotFound
        #region Conflict

        /// <inheritdoc />
        Response IResponseFactory.Conflict(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.Conflict)
                .Message(message)
                .Build();
        }

        /// <inheritdoc />
        Response<T> IResponseFactory.Conflict<T>(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.Conflict)
                .Type(typeof(T))
                .Message(message)
                .Build<T>();
        }

        /// <inheritdoc />
        Response<T> IResponseFactory.Conflict<T>(T value)
        {
            return new ResponseBuilder()
                .State(ResponseState.Conflict)
                .Type(typeof(T)).Value(value)
                .Build<T>();
        }

        /// <inheritdoc />
        QueryResponse<T> IResponseFactory.ConflictQuery<T>(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.Conflict)
                .Type(typeof(T))
                .Message(message)
                .Query(true)
                .BuildQuery<T>();
        }

        #endregion Conflict
        #region Error

        /// <inheritdoc />
        Response IResponseFactory.Error(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.Error)
                .Message(message)
                .Build();
        }

        /// <inheritdoc />
        Response IResponseFactory.Error(Exception exception)
        {
            return new ResponseBuilder()
                .State(ResponseState.Error)
                .Exception(exception)
                .Build();
        }

        /// <inheritdoc />
        Response<T> IResponseFactory.Error<T>(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.Error)
                .Type(typeof(T))
                .Message(message)
                .Build<T>();
        }

        /// <inheritdoc />
        Response<T> IResponseFactory.Error<T>(Exception exception)
        {
            return new ResponseBuilder()
                .State(ResponseState.Error)
                .Type(typeof(T))
                .Exception(exception)
                .Build<T>();
        }

        /// <inheritdoc />
        QueryResponse<T> IResponseFactory.ErrorQuery<T>(string message)
        {
            return new ResponseBuilder()
                .State(ResponseState.Error)
                .Type(typeof(T))
                .Message(message)
                .Query(true)
                .BuildQuery<T>();
        }

        /// <inheritdoc />
        QueryResponse<T> IResponseFactory.ErrorQuery<T>(Exception exception)
        {
            return new ResponseBuilder()
                .State(ResponseState.Error)
                .Type(typeof(T))
                .Exception(exception)
                .Query(true)
                .BuildQuery<T>();
        }

        #endregion Error
        #endregion IResponseFactory
    }
}