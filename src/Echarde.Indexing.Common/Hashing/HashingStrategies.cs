// -----------------------------------------------------------------------------------
// Copyright 2017, Gilles Zunino
// -----------------------------------------------------------------------------------

namespace Echarde.Indexing.Hashing
{
    /// <summary>
    /// Common hashing strategies.
    /// </summary>
    public static class HashingStrategies
    {
        /// <summary>
        /// FastHash hashing strategy.
        /// </summary>
        public static readonly FastHashStrategy FastHashStrategy = new FastHashStrategy();

        /// <summary>
        /// FNV1a hashing strategy.
        /// </summary>
        public static readonly FNV1aStrategy FNV1aStrategy = new FNV1aStrategy();
    }
}