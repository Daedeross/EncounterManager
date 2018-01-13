namespace Foundation.Utilities
{
    using System;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for the <see cref="FNV1aHash"/> and <see cref="FNV1a64Hash"/> classes
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class FNV1aExtensions
    {
        /// <summary>
        /// Hashes one or more integer values into the final computation of this FNV1aHash instance.
        /// </summary>
        /// <param name="hash">target FNV1aHash</param>
        /// <param name="values">array of integer values to compute</param>
        /// <returns>Current hash value after provided steps are computed</returns>
        public static int Step(this FNV1aHash hash, params int[] values)
        {
            return hash.Step(values.SelectMany(BitConverter.GetBytes).ToArray());
        }

        /// <summary>
        /// Hashes one or more long values into the final computation of this FNV1aHash instance.
        /// </summary>
        /// <param name="hash">target FNV1aHash</param>
        /// <param name="values">array of long values to compute</param>
        /// <returns>Current hash value after provided steps are computed</returns>
        public static int Step(this FNV1aHash hash, params long[] values)
        {
            return hash.Step(values.SelectMany(BitConverter.GetBytes).ToArray());
        }

        /// <summary>
        /// Hashes one or more string values into the final computation of this FNV1aHash instance.
        /// </summary>
        /// <param name="hash">target FNV1aHash</param>
        /// <param name="values">array of string values to compute</param>
        /// <returns>Current hash value after provided steps are computed</returns>
        public static int Step(this FNV1aHash hash, params string[] values)
        {
            var output = 0;
            foreach (var value in values)
            {
                output = hash.Step(Encoding.UTF8.GetBytes(value));
            }
            return output;
        }

        /// <summary>
        /// Hashes one or more integer values into the final computation of this FNV1aHash instance.
        /// </summary>
        /// <param name="hash">target FNV1aHash</param>
        /// <param name="values">array of integer values to compute</param>
        /// <returns>Current hash value after provided steps are computed</returns>
        public static long Step(this FNV1a64Hash hash, params int[] values)
        {
            return hash.Step(values.SelectMany(BitConverter.GetBytes).ToArray());
        }

        /// <summary>
        /// Hashes one or more long values into the final computation of this FNV1aHash instance.
        /// </summary>
        /// <param name="hash">target FNV1aHash</param>
        /// <param name="values">array of long values to compute</param>
        /// <returns>Current hash value after provided steps are computed</returns>
        public static long Step(this FNV1a64Hash hash, params long[] values)
        {
            return hash.Step(values.SelectMany(BitConverter.GetBytes).ToArray());
        }

        /// <summary>
        /// Hashes one or more string values into the final computation of this FNV1aHash instance.
        /// </summary>
        /// <param name="hash">target FNV1aHash</param>
        /// <param name="values">array of string values to compute</param>
        /// <returns>Current hash value after provided steps are computed</returns>
        public static long Step(this FNV1a64Hash hash, params string[] values)
        {
            var output = 0L;
            foreach (var value in values)
            {
                output = hash.Step(Encoding.UTF8.GetBytes(value));
            }
            return output;
        }
    }
}