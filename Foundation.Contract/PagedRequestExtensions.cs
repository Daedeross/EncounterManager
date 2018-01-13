namespace Foundation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Data;

    /// <summary>
    /// Extensions to simplify paging operations
    /// </summary>
    public static class PagedRequestExtensions
    {
        /// <summary>
        /// Pages the specified <see cref="IEnumerable{T}"/> based on the passed in <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">Type of the data items</typeparam>
        /// <param name="enumerable"><see cref="IEnumerable{T}"/></param>
        /// <param name="request"><see cref="PagedRequest"/></param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        public static IEnumerable<T> Page<T>(this IEnumerable<T> enumerable, PagedRequest request)
        {
            if (request == null) return enumerable;
            var skip = request.Start > 0 ? enumerable.Skip(request.Start) : enumerable;
            var page = request.PageSize.HasValue && request.PageSize.Value > 0 ? skip.Take(request.PageSize.Value) : skip;
            return page;
        }

        /// <summary>
        /// Pages the specified <see cref="IAsyncEnumerable{T}"/> based on the passed in <paramref name="request"/>
        /// </summary>
        /// <typeparam name="T">Type of the data items</typeparam>
        /// <param name="enumerable"><see cref="IAsyncEnumerable{T}"/></param>
        /// <param name="request"><see cref="PagedRequest"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        public static async Task<IEnumerable<T>> PageAsync<T>(IAsyncEnumerable<T> enumerable, PagedRequest request, CancellationToken cancellationToken)
        {
            var records = new List<T>();
            var enumerator = enumerable.GetAsyncEnumerator();
            int skip = 0;
            while (skip < request.Start && await enumerator.MoveNextAsync(cancellationToken))
            {
                skip++;
            }
            int n = 0;
            int take = request.PageSize.GetValueOrDefault(int.MaxValue);
            while (n < take && await enumerator.MoveNextAsync(cancellationToken))
            {
                records.Add(enumerator.Current);
                n++;
            }
            return records;
        }

    }
}