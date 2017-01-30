// -----------------------------------------------------------------------------------
// Copyright 2017, Gilles Zunino
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Echarde.Indexing.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Echarde.Indexing.Tests.Net46
{
    /// <summary>
    /// FastHash unit tests.
    /// </summary>
    [TestClass]
    public class FastHashTests
    {
        [TestMethod]
        public void String_Hashing()
        {
            string dataToHash = "George Washington";

            uint hash32 = FastHash.FastHash32(dataToHash);
            Assert.IsTrue(hash32 == 0xd22f7524, "FastHash.FastHash32('{0}') == {1:x}", dataToHash, hash32);

            ulong hash64 = FastHash.FastHash64(dataToHash);
            Assert.IsTrue(hash64 == 0xf55ae617c78a5b3b, "FastHash.FastHash64('{0}') == {1:x}", dataToHash, hash64);
        }

        [TestMethod]
        public void Byte_Hashing()
        {
            string dataToHash = "George Washington";
            byte[] bufferToHash = Encoding.UTF8.GetBytes(dataToHash);

            uint hash32 = FastHash.FastHash32(bufferToHash);
            Assert.IsTrue(hash32 == 0xd22f7524, "FastHash.FastHash32('{0}') == {1:x}", dataToHash, hash32);

            ulong hash64 = FastHash.FastHash64(bufferToHash);
            Assert.IsTrue(hash64 == 0xf55ae617c78a5b3b, "FastHash.FastHash64('{0}') == {1:x}", dataToHash, hash64);
        }

        [TestMethod]
        public void Byte_Hashing_Custom_Seed()
        {
            const uint customSeed32 = 0x123456;
            const ulong customSeed64 = 0xf55ae617c78a5b3b;

            string dataToHash = "aBcdefghiJkLMNoP_xRyv9$";

            byte[] bufferToHash = Encoding.UTF8.GetBytes(dataToHash);

            uint hash32 = FastHash.FastHash32(bufferToHash, customSeed32);
            Assert.IsTrue(hash32 == 0xe9dcd4a7, "FastHash.FastHash32('{0}') == {1:x}", dataToHash, hash32);

            ulong hash64 = FastHash.FastHash64(bufferToHash, customSeed64);
            Assert.IsTrue(hash64 == 0xe8990a099b05f460, "FastHash.FastHash64('{0}') == {1:x}", dataToHash, hash64);
        }

        [TestMethod]
        public void Byte_Ulong_Size_Matching_Hashing()
        {
            // Data is exactly 16 bytes, aka 2 * sizeof(ulong)
            string dataToHash = "aBcdefghiJkLMNoP";
            byte[] bufferToHash = Encoding.UTF8.GetBytes(dataToHash);

            uint hash32 = FastHash.FastHash32(bufferToHash);
            Assert.IsTrue(hash32 == 0xf950d073, "FastHash.FastHash32('{0}') == {1:x}", dataToHash, hash32);

            ulong hash64 = FastHash.FastHash64(bufferToHash);
            Assert.IsTrue(hash64 == 0xd6b03639d00106ac, "FastHash.FastHash64('{0}') == {1:x}", dataToHash, hash64);
        }

        [TestMethod]
        public void Byte_Unaligned_Data_Hashing()
        {
            Dictionary<string, Tuple<ulong, ulong>> officialHashes = new Dictionary<string, Tuple<ulong, ulong>>()
            {
                // One byte over
                { "aBcdefghiJkLMNoP_", new Tuple<ulong, ulong>(0x6461a1da, 0xebb62d4e5017cf28) },

                // Two bytes over
                { "aBcdefghiJkLMNoP_x", new Tuple<ulong, ulong>(0x515314ce, 0x3a3fc7a78b92dc75) },

                // Three bytes over
                { "aBcdefghiJkLMNoP_xR", new Tuple<ulong, ulong>(0x5190728e, 0xb633482f07c3babd) },

                // Four bytes over
                { "aBcdefghiJkLMNoP_xRy", new Tuple<ulong, ulong>(0xf701fef5, 0x51fba9f348fda8e8) },

                // Five bytes over
                { "aBcdefghiJkLMNoP_xRyv", new Tuple<ulong, ulong>(0x15c28cd5, 0xd5b19b30eb742805) },

                // Six bytes over
                { "aBcdefghiJkLMNoP_xRyv9", new Tuple<ulong, ulong>(0x376f897, 0xde4c1787e1c3101e) },

                // Seve bytes over
                { "aBcdefghiJkLMNoP_xRyv9$", new Tuple<ulong, ulong>(0x11978a69, 0x9775164aa90ca0b3) }
            };


            // Check all cases
            foreach (string dataToHash in officialHashes.Keys)
            {
                Tuple<ulong, ulong> expectedResults = officialHashes[dataToHash];

                ulong expectedResult32 = expectedResults.Item1;
                ulong expectedResult64 = expectedResults.Item2;

                byte[] bufferToHash = Encoding.UTF8.GetBytes(dataToHash);

                uint hash32 = FastHash.FastHash32(bufferToHash);
                Assert.IsTrue(hash32 == expectedResult32, "FastHash.FastHash32('{0}') == {1:x}", dataToHash, hash32);

                ulong hash64 = FastHash.FastHash64(bufferToHash);
                Assert.IsTrue(hash64 == expectedResult64, "FastHash.FastHash64('{0}') == {1:x}", dataToHash, hash64);
            }
        }
    }
}
