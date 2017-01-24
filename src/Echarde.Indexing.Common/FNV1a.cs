// -----------------------------------------------------------------------------------
// Copyright 2016, Gilles Zunino
// -----------------------------------------------------------------------------------

using System.Text;

namespace Echarde.Indexing
{
    /// <summary>
    /// Calculates the Fowler-Noll-Vo Hash (FNV1a) - http://www.isthe.com/chongo/tech/comp/fnv/index.html#FNV-1a
    /// </summary>
    public static class FNV1a
    {
        /// <summary>
        /// Default FNV1a 32 seed.
        /// </summary>
        public const uint FNV1_32A_INIT = 0x811c9dc5;

        /// <summary>
        /// Default FNV1a 64 seed.
        /// </summary>
        public const ulong FNV1A_64_INIT = 0xcbf29ce484222325UL;

        /// <summary>
        /// Computes the 32-bit FNV1a hash.
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>32 bits FNV1a hash of <paramref name="str"/>.</returns>
        public static uint Fnv1A32(string str)
        {
            return Fnv1A32(Encoding.UTF8.GetBytes(str), FNV1_32A_INIT);
        }

        /// <summary>
        /// Computes the 32-bit FNV1a hash.
        /// </summary>
        /// <param name="data">Input data.</param>
        /// <returns>32 bits FNV1a hash of <paramref name="data"/>.</returns>
        public static uint Fnv1A32(byte[] data)
        {
            return Fnv1A32(data, FNV1_32A_INIT);
        }

        /// <summary>
        /// Computes the 64-bit FNV1a hash.
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>64 bits FNV1a hash of <paramref name="str"/>.</returns>
        public static ulong Fnv1A64(string str)
        {
            return Fnv1A64(Encoding.UTF8.GetBytes(str), FNV1A_64_INIT);
        }

        /// <summary>
        /// Computes the 64-bit FNV1a hash.
        /// </summary>
        /// <param name="data">Input data.</param>
        /// <returns>64 bits FNV1a hash of <paramref name="data"/>.</returns>
        public static ulong Fnv1A64(byte[] data)
        {
            return Fnv1A64(data, FNV1A_64_INIT);
        }

        /// <summary>
        /// Computes thet 32-bit FNV1a hash.
        /// </summary>
        /// <param name="data">Input data.</param>
        /// <param name="seed">Seed.</param>
        /// <returns>32 bits FNV1a hash of <paramref name="data"/>.</returns>
        public static uint Fnv1A32(byte[] data, uint seed)
        {
            unchecked
            {
                for (int i = 0; i < data.Length; i++)
                {
                    seed = seed ^ data[i];
                    seed *= 0x01000193;
                }

                return seed;
            }
        }

        /// <summary>
        /// Computes the 64-bit FNV1a hash.
        /// </summary>
        /// <param name="data">Input data.</param>
        /// <param name="seed">Seed.</param>
        /// <returns>64 bits FNV1a hash of <paramref name="data"/>.</returns>
        public static ulong Fnv1A64(byte[] data, ulong seed)
        {
            unchecked
            {
                for (int i = 0; i < data.Length; i++)
                {
                    seed = seed ^ data[i];
                    seed *= 0x100000001b3L;
                }

                return seed;
            }
        }
    }
}