// -----------------------------------------------------------------------------------
// Copyright 2017, Gilles Zunino
// -----------------------------------------------------------------------------------

namespace Echarde.Indexing.Hashing
{
    /// <summary>
    /// Defines a hashing strategy for combining / trimming Azure Storage keys.
    /// </summary>
    public interface IHashingStrategy
    {
        /// <summary>
        /// Calculates the 64 bits hash used when combining / trimming Azure Storage keys.
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>64 bits hash <paramref name="str"/>.</returns>
        ulong Hash(string str);
    }
}