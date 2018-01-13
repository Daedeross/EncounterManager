/*
 * This class is sourced verbatim from: https://github.com/keithbo/Utterance/blob/master/Utterance/FNV1AHash.cs
 * Attribution license: Apache 2.0
 */
// ReSharper disable InconsistentNaming
namespace Foundation.Utilities
{
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// This class computes FNV-1a Int64 hashes with the ability to carry out multi-part or disjoint
    /// permutation sets. As such the hash can be reset to its initial state for additional hashing.
    /// 
    /// Note: FNV1aHash is "thread-safe" but multi-threaded computation will likely have
    /// unexpected results because hashing order matters. Hashing [a,b,c] will have a different
    /// compute than [b,a,c]
    /// </summary>
    public sealed class FNV1a64Hash
    {
        private const long OffsetBasis = unchecked((long)14695981039346656037);
        private const long Prime = unchecked(1099511628211);

        private long _hash;

        /// <summary>
        /// Int64 value of the currently computed FNV1a hash
        /// </summary>
        public long Value => _hash;

        public FNV1a64Hash()
        {
            Reset();
        }

        /// <summary>
        /// Resets this FNV1aHash instance to its original state
        /// </summary>
        public void Reset()
        {
            Interlocked.Exchange(ref _hash, OffsetBasis);
        }

        /// <summary>
        /// Hashes one or more bytes into the final computation of this FNV1aHash instance.
        /// </summary>
        /// <param name="bytes">array of bytes compute</param>
        /// <returns>Current hash value after provided steps are computed</returns>
        public long Step(byte[] bytes)
        {
            long initial, value;
            do
            {
                initial = _hash;
                value = bytes.Aggregate(initial, (r, o) => (r ^ o) * Prime);
            } while (initial != Interlocked.CompareExchange(ref _hash, value, initial));
            return value;
        }
    }
}