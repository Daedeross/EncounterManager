namespace Foundation.ServiceFabric
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Data;

    /// <remarks>
    /// Borrowed from https://github.com/Azure-Samples/service-fabric-dotnet-management-party-cluster/blob/sdkupdate/PartyCluster/Common/IAsyncEnumerableExtensions.cs
    /// </remarks>
    public static class AsyncEnumerableExtensions
    {
        /// <summary>
        /// Performs an asynchronous for-each loop on an IAsyncEnumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task ForeachAsync<T>(this IAsyncEnumerable<T> instance, CancellationToken cancellationToken, Action<T> action)
        {
            using (IAsyncEnumerator<T> asyncEnumerator = instance.GetAsyncEnumerator())
            {
                while (await asyncEnumerator.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                {
                    action(asyncEnumerator.Current);
                }
            }
        }

        /// <summary>
        /// Counts the number of items that pass the given predicate.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static async Task<int> CountAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            int count = 0;

            using (var asyncEnumerator = source.GetAsyncEnumerator())
            {
                while (await asyncEnumerator.MoveNextAsync(CancellationToken.None).ConfigureAwait(false))
                {
                    if (predicate(asyncEnumerator.Current))
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}