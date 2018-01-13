namespace Foundation.ServiceFabric
{
    using System;

    /// <summary>
    /// Constant defaults for ReliableCollection calls
    /// </summary>
    public static class ReliableCollectionDefaults
    {
        /// <summary>
        /// Default ReliableCollection read timeout is 4 seconds
        /// </summary>
        public static readonly TimeSpan ReadTimeout = TimeSpan.FromSeconds(4);

        /// <summary>
        /// Default ReliableCollection write timeout is 4 seconds
        /// </summary>
        public static readonly TimeSpan WriteTimeout = TimeSpan.FromSeconds(4);
    }
}