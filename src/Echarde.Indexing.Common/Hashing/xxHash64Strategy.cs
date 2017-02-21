// -----------------------------------------------------------------------------------
// Copyright 2017, Gilles Zunino
// -----------------------------------------------------------------------------------

namespace Echarde.Indexing.Hashing
{
    /// <summary>
    /// xxHash64 hashing strategy.
    /// </summary>
    public class xxHash64Strategy : IHashingStrategy
    {
        /// <summary>
        /// Computes the xxHash64 hash. 
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>xxHash64 of <paramref name="str"/>.</returns>
        public ulong Hash(string str)
        {
            return xxHash.xxHash64(str);
        }
    }
}