namespace Foundation.ServiceFabric
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class QueryResponseExtensions
    {
        public static QueryResponse<T> ToQueryResponse<T>(this IEnumerable<T> values, long? offset, long? total)
        {
            return ResponseFactory.OkQuery(values, offset, total);
        }

        public static QueryResponse<T> ToQueryResponse<T>(this ICollection<T> values, PagedRequest pagedRequest)
        {
            return ResponseFactory.OkQuery(values.Page(pagedRequest), pagedRequest?.Start, values.Count);
        }

        public static QueryResponse<TResult> ToQueryResponse<TSource, TResult>(this IEnumerable<TSource> values, Func<TSource, TResult> selector, long? offset, long? total)
        {
            return ResponseFactory.OkQuery(values.Select(selector), offset, total);
        }

        public static QueryResponse<TResult> ToQueryResponse<TSource, TResult>(this ICollection<TSource> values, Func<TSource, TResult> selector, PagedRequest pagedRequest)
        {
            return ResponseFactory.OkQuery(values.Page(pagedRequest).Select(selector), pagedRequest?.Start, values.Count);
        }

    }
}
