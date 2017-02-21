// -----------------------------------------------------------------------------------
// Copyright 2017, Gilles Zunino
// -----------------------------------------------------------------------------------

using System.Text;


namespace Echarde.Indexing.Hashing
{
    /// <summary>
    /// Calculates xxHash - See https://github.com/rurban/smhasher
    /// </summary>
    public static class xxHash
    {
        /// <summary>
        /// xxHash32 prime constants.
        /// </summary>
        private const uint PRIME32_1 = 2654435761U;
        private const uint PRIME32_2 = 2246822519U;
        private const uint PRIME32_3 = 3266489917U;
        private const uint PRIME32_4 = 668265263U;
        private const uint PRIME32_5 = 374761393U;

        /// <summary>
        /// xxHash64 prime constants.
        /// </summary>
        private const ulong PRIME64_1 = 11400714785074694791UL;
        private const ulong PRIME64_2 = 14029467366897019727UL;
        private const ulong PRIME64_3 = 1609587929392839161UL;
        private const ulong PRIME64_4 = 9650029242287828579UL;
        private const ulong PRIME64_5 = 2870177450012600261UL;


        /// <summary>
        /// Default xxHash 32 seed.
        /// </summary>
        public const uint DefaultSeed32 = 0U;

        /// <summary>
        /// Default xxHash 64 seed.
        /// </summary>
        public const ulong DefaultSeed64 = 0UL;


        /// <summary>
        /// Computes the 32 xxHash. 
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>32 bits xxHash of <paramref name="str"/>.</returns>
        public static uint xxHash32(string str)
        {
            return xxHash32(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Computes the 32 xxHash. 
        /// </summary>
        /// <param name="data">Input data.</param>
        /// <returns>32 bits xxHash of <paramref name="data"/>.</returns>
        public static uint xxHash32(byte[] data)
        {
            return xxHash32(data, DefaultSeed32);
        }

        /// <summary>
        /// Computes the 64 xxHash. 
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>64 bits xxHash of <paramref name="str"/>.</returns>
        public static ulong xxHash64(string str)
        {
            return xxHash64(Encoding.UTF8.GetBytes(str), DefaultSeed64);
        }

        /// <summary>
        /// Computes the 64 xxHash. 
        /// </summary>
        /// <param name="data">Input data.</param>
        /// <returns>64 bits xxHash of <paramref name="data"/>.</returns>
        public static ulong xxHash64(byte[] data)
        {
            return xxHash64(data, DefaultSeed64);
        }

        /// <summary>
        /// Computes the 32 xxHash. 
        /// </summary>
        /// <param name="data">Input data.</param>
        /// <param name="seed">Seed.</param>
        /// <returns>32 bits xxHash of <paramref name="data"/>.</returns>
        public static uint xxHash32(byte[] data, uint seed)
        {
            uint h32 = 0;

            unsafe
            {
                fixed (byte* inputSrc = data)
                {
                    uint* uintSrc = (uint*) inputSrc;

                    if (data.Length >= 16)
                    {
                        uint v1 = seed + PRIME32_1 + PRIME32_2;
                        uint v2 = seed + PRIME32_2;
                        uint v3 = seed + 0;
                        uint v4 = seed - PRIME32_1;

                        do
                        {
                            v1 = Round32(v1, *uintSrc); uintSrc++;
                            v2 = Round32(v2, *uintSrc); uintSrc++;
                            v3 = Round32(v3, *uintSrc); uintSrc++;
                            v4 = Round32(v4, *uintSrc); uintSrc++;

                        } while (uintSrc <= (uint *) (inputSrc + data.Length - 16));

                        h32 = Rotl32(v1, 1) + Rotl32(v2, 7) + Rotl32(v3, 12) + Rotl32(v4, 18);
                    }
                    else
                    {
                        h32 = seed + PRIME32_5;
                    }

                    h32 += (uint)data.Length;

                    while ((uintSrc + 1) <= (uint *) (inputSrc + data.Length))
                    {
                        h32 += *uintSrc * PRIME32_3;
                        h32 = Rotl32(h32, 17) * PRIME32_4;
                        uintSrc++;
                    }

                    byte* byteSrc = (byte *) uintSrc;

                    while (byteSrc < (uint *) (inputSrc + data.Length))
                    {
                        h32 += *byteSrc * PRIME32_5;
                        h32 = Rotl32(h32, 11) * PRIME32_1;
                        byteSrc++;
                    }
                }

                h32 ^= h32 >> 15;
                h32 *= PRIME32_2;
                h32 ^= h32 >> 13;
                h32 *= PRIME32_3;
                h32 ^= h32 >> 16;
            }

            return h32;
        }

        /// <summary>
        /// Computes the 64 xxHash. 
        /// </summary>
        /// <param name="data">Input data.</param>
        /// <param name="seed">Seed.</param>
        /// <returns>64 bits xxHash of <paramref name="data"/>.</returns>
        public static ulong xxHash64(byte[] data, ulong seed)
        {
            ulong h64 = 0;

            unsafe
            {
                fixed (byte* inputSrc = data)
                {
                    ulong* ulongSrc = (ulong*)inputSrc;

                    if (data.Length >= 32)
                    {
                        ulong v1 = seed + PRIME64_1 + PRIME64_2;
                        ulong v2 = seed + PRIME64_2;
                        ulong v3 = seed + 0;
                        ulong v4 = seed - PRIME64_1;

                        do
                        {
                            v1 = Round64(v1, *ulongSrc); ulongSrc++;
                            v2 = Round64(v2, *ulongSrc); ulongSrc++;
                            v3 = Round64(v3, *ulongSrc); ulongSrc++;
                            v4 = Round64(v4, *ulongSrc); ulongSrc++;

                        } while (ulongSrc <= (ulong*)(inputSrc + data.Length - 32));

                        h64 = Rotl64(v1, 1) + Rotl64(v2, 7) + Rotl64(v3, 12) + Rotl64(v4, 18);

                        h64 = MergeRound64(h64, v1);
                        h64 = MergeRound64(h64, v2);
                        h64 = MergeRound64(h64, v3);
                        h64 = MergeRound64(h64, v4);
                    }
                    else
                    {
                        h64 = seed + PRIME64_5;
                    }

                    h64 += (ulong) data.Length;

                    while (ulongSrc + 1 <= (ulong*)(inputSrc + data.Length))
                    {
                        ulong k1 = Round64(0, *ulongSrc);
                        h64 ^= k1;
                        h64 = Rotl64(h64, 27) * PRIME64_1 + PRIME64_4;
                        ulongSrc++;
                    }

                    uint* uintSrc = (uint*) ulongSrc;

                    if (uintSrc + 1 <= (uint*)(inputSrc + data.Length))
                    {
                        h64 ^= (ulong) *uintSrc * PRIME64_1;
                        h64 = Rotl64(h64, 23) * PRIME64_2 + PRIME64_3;
                        uintSrc++;
                    }

                    byte* byteSrc = (byte*) uintSrc;

                    while (byteSrc < (uint*)(inputSrc + data.Length))
                    {
                        h64 ^= *byteSrc * PRIME64_5;
                        h64 = Rotl64(h64, 11) * PRIME64_1;
                        byteSrc++;
                    }
                }

                h64 ^= h64 >> 33;
                h64 *= PRIME64_2;
                h64 ^= h64 >> 29;
                h64 *= PRIME64_3;
                h64 ^= h64 >> 32;
            }

            return h64;            
        }

        private static uint Rotl32(uint x, int r)
        {
            unchecked
            {
                return (x << r) | (x >> (32 - r));
            }
        }

        private static ulong Rotl64(ulong x, int r)
        {
            unchecked
            {
                return (x << r) | (x >> (64 - r));
            }
        }

        private static uint Round32(uint seed, uint input)
        {
            unchecked
            {
                seed += input * PRIME32_2;
                seed = Rotl32(seed, 13);
                seed *= PRIME32_1;

                return seed;
            }
        }

        private static ulong Round64(ulong acc, ulong input)
        {
            unchecked
            {
                acc += input * PRIME64_2;
                acc = Rotl64(acc, 31);
                acc *= PRIME64_1;

                return acc;
            }
        }

        private static ulong MergeRound64(ulong acc, ulong val)
        {
            val = Round64(0, val);
            acc ^= val;
            acc = acc * PRIME64_1 + PRIME64_4;

            return acc;
        }
    }
}