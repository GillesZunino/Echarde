// -----------------------------------------------------------------------------------
// Copyright 2017, Gilles Zunino
// -----------------------------------------------------------------------------------

using Echarde.Indexing.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Echarde.Indexing.Tests
{
    /// <summary>
    /// HashsingStrategies unit tests.
    /// </summary>
    [TestClass]
    public class HashingStrategiesTests
    {
        [TestMethod]
        public void Basic_Strategies()
        {
            string dataToHash = "aBcdefghiJkLMNoP_xRyv9$";

            ulong fastHash = HashingStrategies.FastHashStrategy.Hash(dataToHash);
            Assert.IsTrue(fastHash == 0x9775164AA90CA0B3, "HashingStrategies.FastHashStrategy.Hash('{0}') == {1:x}", dataToHash);

            ulong fnv1a = HashingStrategies.FNV1aStrategy.Hash(dataToHash);
            Assert.IsTrue(fnv1a == 0x10A085EC86517C0A, "HashingStrategies.FNV1aStrategy.Hash('{0}') == {1:x}", dataToHash, fnv1a);

            ulong xxHash = HashingStrategies.xxHash64Strategy.Hash(dataToHash);
            Assert.IsTrue(xxHash == 0x1BF40850181F40ED, "HashingStrategies.xxHash64Strategy.Hash('{0}') == {1:x}", dataToHash, xxHash);
        }
    }
}
