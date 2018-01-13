namespace Foundation.ServiceFabric
{
    public static class ResponseExtensions
    {
        public static Response Cast(this Response response)
        {
            var builder = new ResponseBuilder();
            var visitor = new ResponseToBuilderVisitor(builder);
            response.Accept(visitor);

            return builder.Type(null).Query(false).Build();
        }

        public static Response<T> Cast<T>(this Response response)
        {
            var builder = new ResponseBuilder();
            var visitor = new ResponseToBuilderVisitor(builder);
            response.Accept(visitor);

            return builder.Type(typeof(T)).Query(false).Build<T>();
        }

        public static QueryResponse<T> CastQuery<T>(this Response response)
        {
            var builder = new ResponseBuilder();
            var visitor = new ResponseToBuilderVisitor(builder);
            response.Accept(visitor);

            return builder.Type(typeof(T)).Query(true).BuildQuery<T>();
        }

        internal class ResponseToBuilderVisitor : IResponseVisitor
        {
            public ResponseBuilder Builder { get; set; }

            public ResponseToBuilderVisitor(ResponseBuilder builder)
            {
                Builder = builder;
            }

            public Response Visit(Response response)
            {
                Builder
                    .State(response.ResponseState)
                    .Message(response.Message);
                return response;
            }

            public Response<T> Visit<T>(Response<T> response)
            {
                Builder
                    .State(response.ResponseState)
                    .Type(typeof(T)).Value(response.Value)
                    .Message(response.Message);
                return response;
            }

            public QueryResponse<T> Visit<T>(QueryResponse<T> response)
            {
                Builder
                    .State(response.ResponseState)
                    .Type(typeof(T)).Value(response.Results)
                    .Message(response.Message)
                    .Query(true).Offset(response.Offset).Total(response.Total);
                return response;
            }
        }
    }
}