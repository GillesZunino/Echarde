// -----------------------------------------------------------------------------------
// Copyright 2017, Gilles Zunino
// -----------------------------------------------------------------------------------

using System.Text;

namespace Echarde.Indexing.Hashing
{
    /// <summary>
    /// Calculates FastHash - See https://github.com/rurban/smhasher
    /// </summary>
    public static class FastHash
    {
        /// <summary>
        /// Default FastHash 32 seed.
        /// </summary>
        public const uint DefaultSeed32 = 0U;

        /// <summary>
        /// Default FastHash 64 seed.
        /// </summary>
        public const ulong DefaultSeed64 = 0UL;


        /// <summary>
        /// Computes the 32 FastHash. 
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>32 bits Fasthash of <paramref name="str"/>.</returns>
        public static uint FastHash32(string str)
        {
            return FastHash32(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Computes the 32 FastHash. 
        /// </summary>
        /// <param name="data">Input data.</param>
        /// <returns>32 bits Fasthash of <paramref name="data"/>.</returns>
        public static uint FastHash32(byte[] data)
        {
            return FastHash32(data, DefaultSeed32);
        }

        /// <summary>
        /// Computes the 64 FastHash. 
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>64 bits Fasthash of <paramref name="str"/>.</returns>
        public static ulong FastHash64(string str)
        {
            return FastHash64(Encoding.UTF8.GetBytes(str), DefaultSeed64);
        }

        /// <summary>
        /// Computes the 64 FastHash. 
        /// </summary>
        /// <param name="data">Input data.</param>
        /// <returns>64 bits Fasthash of <paramref name="data"/>.</returns>
        public static ulong FastHash64(byte[] data)
        {
            return FastHash64(data, DefaultSeed64);
        }

        /// <summary>
        /// Computes the 32 FastHash. 
        /// </summary>
        /// <param name="data">Input data.</param>
        /// <param name="seed">Seed.</param>
        /// <returns>32 bits Fasthash of <paramref name="data"/>.</returns>
        public static uint FastHash32(byte[] data, uint seed)
        {
            ulong hash = FastHash64(data, seed);

            // Convert the 64 bits hashcode to Fermat residue to retain information on both the higher and lower parts of the hashcode
            return (uint) (hash - (hash >> 32));
        }

        /// <summary>
        /// Computes the 64 FastHash. 
        /// </summary>
        /// <param name="data">Input data.</param>
        /// <param name="seed">Seed.</param>
        /// <returns>64 bits Fasthash of <paramref name="data"/>.</returns>
        public static ulong FastHash64(byte[] data, ulong seed)
        {
            const ulong m = 0x880355f21e6d1965UL;

            unchecked
            {
                ulong hash = seed ^ ((ulong)data.Length * m);

                unsafe
                {
                    fixed (byte* byteSrc = data)
                    {
                        ulong* ulongSrc = (ulong*)byteSrc;

                        for (int i = 0; i < data.Length / 8; i++)
                        {
                            hash ^= MerkleDamgardCompress(*ulongSrc);
                            hash *= m;

                            ulongSrc++;
                        }
                    }
                }

                // If the input buffer size is not exactly a multiple of sizeof(ulong) (aka 8 bytes), we need to hash the remaining data
                int remainingBytes = data.Length & 7;
                int index = data.Length - 1;
                ulong v = 0;

                for (; remainingBytes > 1; remainingBytes--)
                {
                    v ^= (ulong) data[index] << (8 * (remainingBytes - 1));
                    index--;
                }

                if (remainingBytes == 1)
                {
                    v ^= data[index];

                    hash ^= MerkleDamgardCompress(v);
                    hash *= m;
                }

                return MerkleDamgardCompress(hash);
            }
        }

        private static ulong MerkleDamgardCompress(ulong value)
        {
            unchecked
            {
                value ^= value >> 23;
                value *= 0x2127599bf4325c37UL;
                value ^= value >> 47;

                return value;
            }
        }
    }
}