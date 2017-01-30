// -----------------------------------------------------------------------------------
// Copyright 2017, Gilles Zunino
// -----------------------------------------------------------------------------------

namespace Echarde.Indexing.Hashing
{
    /// <summary>
    /// FNV1a hashing strategy.
    /// </summary>
    public class FNV1aStrategy : IHashingStrategy
    {
        /// <summary>
        /// Computes the 64-bit FNV1a hash.
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>64 bits FNV1a hash of <paramref name="str"/>.</returns>
        public ulong Hash(string str)
        {
            return FNV1a.Fnv1A64(str);
        }
    }
}