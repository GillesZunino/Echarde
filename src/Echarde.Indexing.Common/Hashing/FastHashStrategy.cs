// -----------------------------------------------------------------------------------
// Copyright 2017, Gilles Zunino
// -----------------------------------------------------------------------------------

namespace Echarde.Indexing.Hashing
{
    /// <summary>
    /// FastHash hashing strategy.
    /// </summary>
    public class FastHashStrategy : IHashingStrategy
    {
        /// <summary>
        /// Computes the 64-bit FastHash. 
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>64 bits Fasthash of <paramref name="str"/>.</returns>
        public ulong Hash(string str)
        {
            return FastHash.FastHash64(str);
        }
    }
}