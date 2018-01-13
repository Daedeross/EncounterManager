namespace Foundation.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        /// <summary>
        /// Computes the sum of a sequence of nullable System.Decimal values.
        /// </summary>
        /// <param name="source">A sequence of nullable System.Decimal values to calculate the sum of.</param>
        /// <returns>NULL if all values in the sequence are null. Otherwise, the sum of the values in the sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///    <paramref name="source" /> is null.
        /// </exception>
        public static decimal? NullableSum(this IEnumerable<decimal?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(value => value.HasValue)
                         .Aggregate<decimal?, decimal?>(null, (current, value) => current.GetValueOrDefault() + value.Value);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable System.Double values.
        /// </summary>
        /// <param name="source">A sequence of nullable System.Double values to calculate the sum of.</param>
        /// <returns>NULL if all values in the sequence are null. Otherwise, the sum of the values in the sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///    <paramref name="source" /> is null.
        /// </exception>
        public static double? NullableSum(this IEnumerable<double?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(value => value.HasValue)
                         .Aggregate<double?, double?>(null, (current, value) => current.GetValueOrDefault() + value.Value);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable System.Int32 values.
        /// </summary>
        /// <param name="source">A sequence of nullable System.Int32 values to calculate the sum of.</param>
        /// <returns>NULL if all values in the sequence are null. Otherwise, the sum of the values in the sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///    <paramref name="source" /> is null.
        /// </exception>
        public static int? NullableSum(this IEnumerable<int?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(value => value.HasValue)
                         .Aggregate<int?, int?>(null, (current, value) => current.GetValueOrDefault() + value.Value);
        }

        /// <summary>
        /// Return a distinct set of values using a selector to determine comparison key
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="equalityComparer">Equality comparer for the key values, null uses a default comparer</param>
        /// <returns><see cref="IEnumerable{T}"/> of <typeparamref name="TSource"/></returns>
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IEqualityComparer<TKey> equalityComparer = null)
        {
            var set = new HashSet<TKey>(equalityComparer ?? EqualityComparer<TKey>.Default);
            foreach (var item in source)
            {
                if (set.Add(selector(item))) yield return item;
            }
        }
    }
}
