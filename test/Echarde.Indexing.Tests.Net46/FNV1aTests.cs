// -----------------------------------------------------------------------------------
// Copyright 2016, Gilles Zunino
// -----------------------------------------------------------------------------------

using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Echarde.Indexing.Tests.Net46
{
    [TestClass]
    public class FNV1aTests
    {
        [TestMethod]
        public void String_Hashing()
        {
            string dataToHash = "George Washington";

            uint hash32 = FNV1a.Fnv1A32(dataToHash);
            Assert.IsTrue(hash32 == 0x232ef2f2, "FNV1a.Fnv1A32('{0}') == {1:x}", dataToHash, hash32);

            ulong hash64 = FNV1a.Fnv1A64(dataToHash);
            Assert.IsTrue(hash64 == 0xbfd1a777d49363f2UL, "FNV1a.Fnv1A32('{0}') == {1:x}", dataToHash, hash64);
        }

        [TestMethod]
        public void Byte_Hashing()
        {
            string dataToHash = "George Washington";
            byte[] bufferToHash = Encoding.UTF8.GetBytes(dataToHash);

            uint hash32 = FNV1a.Fnv1A32(bufferToHash);
            Assert.IsTrue(hash32 == 0x232ef2f2, "FNV1a.Fnv1A32('{0}') == {1:x}", dataToHash, hash32);

            ulong hash64 = FNV1a.Fnv1A64(bufferToHash);
            Assert.IsTrue(hash64 == 0xbfd1a777d49363f2UL, "FNV1a.Fnv1A32('{0}') == {1:x}", dataToHash, hash64);
        }
    }
}
