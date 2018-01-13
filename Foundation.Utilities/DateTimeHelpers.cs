namespace Foundation.Utilities
{
    using System;

    public static class DateTimeHelpers
    {
        /// <summary>
        /// Test if two DateTime ranges intersect.
        /// </summary>
        /// <param name="startA">The start a.</param>
        /// <param name="endA">The end a.</param>
        /// <param name="startB">The start b.</param>
        /// <param name="endB">The end b.</param>
        /// <returns><c>true</c> if A overlaps B, <c>false</c> otherwise.</returns>
        public static bool Intersects(DateTime startA, DateTime endA, DateTime startB, DateTime endB)
        {
            return Max(startA, startB) < Min(endA, endB);
        }

        /// <summary>
        /// Returns the larger of two dates
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>DateTime.</returns>
        public static DateTime Max(DateTime first, DateTime second)
        {
            return first < second ? second : first;
        }

        /// <summary>
        /// Returns the lesser of two dates
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>DateTime.</returns>
        public static DateTime Min(DateTime first, DateTime second)
        {
            return first < second ? first : second;
        }
    }
}