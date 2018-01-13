namespace Foundation.ServiceFabric
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Simple implementation of <see cref="IResponseBuilder"/> that uses a Fluent-like pattern to build up the response instance
    /// that will be built.
    /// </summary>
    /// <seealso cref="IResponseBuilder" />
    public class ResponseBuilder : IResponseBuilder
    {
        private ResponseState? _state;
        private string _message;
        private Exception _exception;
        private Type _type;
        private object _value;
        private bool _isQuery;
        private long? _offset;
        private long? _total;

        public ResponseBuilder State(ResponseState state)
        {
            _state = state;
            return this;
        }

        public ResponseBuilder Message(string message)
        {
            _message = message;
            return this;
        }

        public ResponseBuilder Exception(Exception exception)
        {
            _exception = exception;
            return this;
        }

        public ResponseBuilder Type(Type type)
        {
            _type = type;
            return this;
        }

        public ResponseBuilder Value(object value)
        {
            _value = value;
            return this;
        }

        public ResponseBuilder Query(bool isQuery)
        {
            _isQuery = isQuery;
            return this;
        }

        public ResponseBuilder Offset(long? offset)
        {
            _offset = offset;
            return this;
        }

        public ResponseBuilder Total(long? total)
        {
            _total = total;
            return this;
        }

        /// <inheritdoc />
        public Response Build()
        {
            IBuildResponse impl;
            if (_isQuery)
            {
                if (_type == null)
                {
                    throw new InvalidOperationException("Unable to build QueryResponse<T> when no type or value is specified");
                }
                impl = (IBuildResponse)Activator.CreateInstance(typeof(QueryResponseBuilderImpl<>).MakeGenericType(_type));
            }
            else
            {
                if (_type == null)
                {
                    impl = new ResponseBuilderImpl();
                }
                else
                {
                    impl = (IBuildResponse)Activator.CreateInstance(typeof(GenericResponseBuilderImpl<>).MakeGenericType(_type));
                }
            }
            return impl.BuildImpl(this);
        }

        internal interface IBuildResponse
        {
            Response BuildImpl(ResponseBuilder source);
        }

        internal class QueryResponseBuilderImpl<T> : IBuildResponse
        {
            public Response BuildImpl(ResponseBuilder source)
            {
                if (source._exception != null && source._state.HasValue && source._state.Value != ResponseState.Error)
                {
                    throw new InvalidOperationException($"Unable to build QueryResponse<{typeof(T).Name}> with state {source._state} when an Exception is also provided");
                }

                var message = source._message ?? source._exception?.Message;

                var state = source._state.GetValueOrDefault(source._exception != null ? ResponseState.Error : ResponseState.Ok);

                var results = new List<T>();

                var value = source._value;
                if (value != null)
                {
                    var enumerable = value as IEnumerable;
                    if (enumerable != null)
                    {
                        results.AddRange(enumerable.Cast<T>());
                    }
                    else
                    {
                        if (value is T)
                        {
                            results.Add((T)value);
                        }
                    }
                }

                var response = new QueryResponse<T>(state, message, results);
                if (source._offset.HasValue)
                {
                    response.Offset = source._offset.Value;
                }
                if (source._total.HasValue)
                {
                    response.Total = source._total.Value;
                }
                return response;
            }
        }

        internal class ResponseBuilderImpl : IBuildResponse
        {
            public Response BuildImpl(ResponseBuilder source)
            {
                if (source._exception != null && source._state.HasValue && source._state.Value != ResponseState.Error)
                {
                    throw new InvalidOperationException($"Unable to build Response with state {source._state} when an Exception is also provided");
                }

                var message = source._message ?? source._exception?.Message;

                var state = source._state.GetValueOrDefault(source._exception != null ? ResponseState.Error : ResponseState.Ok);

                return new Response(state, message);
            }
        }

        internal class GenericResponseBuilderImpl<T> : IBuildResponse
        {
            public Response BuildImpl(ResponseBuilder source)
            {
                if (source._exception != null && source._state.HasValue && source._state.Value != ResponseState.Error)
                {
                    throw new InvalidOperationException($"Unable to build Response with state {source._state} when an Exception is also provided");
                }

                var message = source._message ?? source._exception?.Message;

                var state = source._state.GetValueOrDefault(source._exception != null ? ResponseState.Error : ResponseState.Ok);

                T value = default(T);
                if (source._value is T)
                {
                    value = (T) source._value;
                }

                return new Response<T>(state, message, value);
            }
        }
    }
}