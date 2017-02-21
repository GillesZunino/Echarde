// -----------------------------------------------------------------------------------
// Copyright 2017, Gilles Zunino
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Echarde.Indexing.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Echarde.Indexing.Tests
{
    /// <summary>
    /// xxHash unit tests.
    /// </summary>
    [TestClass]
    public class xxHashTests
    {
        [TestMethod]
        public void String_Hashing()
        {
            string dataToHash = "George Washington";

            uint hash32 = xxHash.xxHash32(dataToHash);
            Assert.IsTrue(hash32 == 0x2B4309A0, "xxHash.xxHash32('{0}') == {1:x}", dataToHash, hash32);

            ulong hash64 = xxHash.xxHash64(dataToHash);
            Assert.IsTrue(hash64 == 0x407409FFF9E55316, "xxHash.xxHash64('{0}') == {1:x}", dataToHash, hash64);
        }

        [TestMethod]
        public void Byte_Hashing()
        {
            string dataToHash = "George Washington";
            byte[] bufferToHash = Encoding.UTF8.GetBytes(dataToHash);

            uint hash32 = xxHash.xxHash32(bufferToHash);
            Assert.IsTrue(hash32 == 0x2B4309A0, "xxHash.xxHash32('{0}') == {1:x}", dataToHash, hash32);

            ulong hash64 = xxHash.xxHash64(bufferToHash);
            Assert.IsTrue(hash64 == 0x407409FFF9E55316, "xxHash.xxHash64('{0}') == {1:x}", dataToHash, hash64);
        }

        [TestMethod]
        public void Byte_Hashing_Custom_Seed()
        {
            const uint customSeed32 = 0x123456;
            const ulong customSeed64 = 0xf55ae617c78a5b3b;

            string dataToHash = "aBcdefghiJkLMNoP_xRyv9$";

            byte[] bufferToHash = Encoding.UTF8.GetBytes(dataToHash);

            uint hash32 = xxHash.xxHash32(bufferToHash, customSeed32);
            Assert.IsTrue(hash32 == 0x055BDDF4, "xxHash.xxHash32('{0}') == {1:x}", dataToHash, hash32);

            ulong hash64 = xxHash.xxHash64(bufferToHash, customSeed64);
            Assert.IsTrue(hash64 == 0xF48043C981CA9321, "xxHash.xxHash64('{0}') == {1:x}", dataToHash, hash64);
        }

        [TestMethod]
        public void Byte_Ulong_Size_Matching_Hashing()
        {
            // Data is exactly 16 bytes, aka 2 * sizeof(ulong)
            string dataToHash = "aBcdefghiJkLMNoP";
            byte[] bufferToHash = Encoding.UTF8.GetBytes(dataToHash);

            uint hash32 = xxHash.xxHash32(bufferToHash);
            Assert.IsTrue(hash32 == 0xC9EC8BF9, "xxHash.xxHash32('{0}') == {1:x}", dataToHash, hash32);

            ulong hash64 = xxHash.xxHash64(bufferToHash);
            Assert.IsTrue(hash64 == 0x71D17260404853CD, "xxHash.xxHash64('{0}') == {1:x}", dataToHash, hash64);
        }

        [TestMethod]
        public void Byte_Unaligned_Data_Hashing()
        {
            Dictionary<string, Tuple<ulong, ulong>> officialHashes = new Dictionary<string, Tuple<ulong, ulong>>()
            {
                // 13 bytes
                { "aBcdehkLMNoP_", new Tuple<ulong, ulong>(0x9a69d1f6, 0xbda8fde1e5c554b0) },

                // 14 bytes
                { "aBcdehkLMNoP_x", new Tuple<ulong, ulong>(0xdb7e9059, 0x5ed488b8395fb5bf) },

                // 15 bytes
                { "aBcdehkLMNoP_xR", new Tuple<ulong, ulong>(0xcf94191, 0x6276737d5f174ee2) },

                // 16 bytes
                { "aBcdehkLMNoP_xRy", new Tuple<ulong, ulong>(0x8a427d6, 0x4ff4e69039d04fb2) },

                // 17 bytes
                { "aBcdehkLMNoP_xRyv", new Tuple<ulong, ulong>(0x3f7cebb8, 0x25330fb06b4e36cf) },

                // 18 bytes
                { "aBcdehkLMNoP_xRyv9", new Tuple<ulong, ulong>(0xe89aa92a, 0x184867780f02d817) },

                // 19 bytes
                { "aBcdehkLMNoP_xRyv9$", new Tuple<ulong, ulong>(0x7fba288, 0x6aea46ec77ad93c7) },

                // 20 bytes
                { "aBcdehkLMNoP_xRyv9$0", new Tuple<ulong, ulong>(0x9b25d468, 0xc6970899ec2abe0a) },

                // 21 bytes
                { "aBcdehkLMNoP_xRyv9$01", new Tuple<ulong, ulong>(0xe2cd5472, 0x8c8ede37cf7aa333) },

                // 22 bytes
                { "aBcdehkLMNoP_xRyv9$012", new Tuple<ulong, ulong>(0x86d48e3b, 0xb0e858e2098c8ad4) },
                
                // 23 bytes
                { "aBcdehkLMNoP_xRyv9$0123", new Tuple<ulong, ulong>(0xca228074, 0x1e96adb8f9fcc67d) },
                
                // 24 bytes
                { "aBcdehkLMNoP_xRyv9$01234", new Tuple<ulong, ulong>(0xda8f6a70, 0x26adf43e2840e716) },
                
                // 25 bytes
                { "aBcdehkLMNoP_xRyv9$012345", new Tuple<ulong, ulong>(0xaaab10a9, 0x3e104d9853954f06) },
                
                // 26 bytes
                { "aBcdehkLMNoP_xRyv9$0123456", new Tuple<ulong, ulong>(0xc89174fc, 0x2202c1669c507c6d) },

                // 27 bytes
                { "aBcdehkLMNoP_xRyv9$01234567", new Tuple<ulong, ulong>(0xd19b7dd7, 0x21f0cfebb036ebc5) },

                // 28 bytes
                { "aBcdehkLMNoP_xRyv9$012345678", new Tuple<ulong, ulong>(0x329e24cc, 0xeb766ba57bd18ea9) },

                // 29 bytes
                { "aBcdehkLMNoP_xRyv9$0123456789", new Tuple<ulong, ulong>(0x5e392df9, 0x9567c651a71428e5) },

                // 30 bytes
                { "aBcdehkLMNoP_xRyv9$0123456789!", new Tuple<ulong, ulong>(0x57ac865a, 0xe5e91109a9802b72) },

                // 31 bytes
                { "aBcdehkLMNoP_xRyv9$0123456789!@", new Tuple<ulong, ulong>(0xb01900c2, 0x39c1781c0ee63b13) },

                // 32 bytes
                { "aBcdehkLMNoP_xRyv9$0123456789!@#", new Tuple<ulong, ulong>(0xcc21ca78, 0xcc1cffbfe664d4e5) },

                // 33 bytes
                { "aBcdehkLMNoP_xRyv9$0123456789!@#%", new Tuple<ulong, ulong>(0x4129ca2b, 0xc9e67d62640760b5) },

                // 34 bytes
                { "aBcdehkLMNoP_xRyv9$0123456789!@#%^", new Tuple<ulong, ulong>(0xa5e6f896, 0x8ad157f80dd056f8) }
            };


            // Check all cases
            foreach (string dataToHash in officialHashes.Keys)
            {
                Tuple<ulong, ulong> expectedResults = officialHashes[dataToHash];

                ulong expectedResult32 = expectedResults.Item1;
                ulong expectedResult64 = expectedResults.Item2;

                byte[] bufferToHash = Encoding.UTF8.GetBytes(dataToHash);

                uint hash32 = xxHash.xxHash32(bufferToHash);
                Assert.IsTrue(hash32 == expectedResult32, "xxHash.xxHash32('{0}') == {1:x}", dataToHash, hash32);

                ulong hash64 = xxHash.xxHash64(bufferToHash);
                Assert.IsTrue(hash64 == expectedResult64, "xxHash.xxHash64('{0}') == {1:x}", dataToHash, hash64);
            }
        }
    }
}
