// -----------------------------------------------------------------------------------
// Copyright 2017, Gilles Zunino
// -----------------------------------------------------------------------------------

using System;
using System.Text;
using Echarde.Indexing.Hashing;
using Echarde.Indexing.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Echarde.Indexing.Tests
{
    /// <summary>
    /// StorageKeyBuilder unit tests.
    /// </summary>
    [TestClass]
    public class StorageKeyBuilderTests
    {
        [TestMethod]
        public void CombineStorageKeys_NullOrEmptyKey_Throws()
        {
            AssertExtensions.ShouldThrow(
                () => { StorageKeyBuilder.CombineStorageKeys(null, null); },
                (exception) => { return exception.GetType() == typeof(ArgumentOutOfRangeException); }
            );

            AssertExtensions.ShouldThrow(
                () => { StorageKeyBuilder.CombineStorageKeys("a", null); },
                (exception) => { return exception.GetType() == typeof(ArgumentOutOfRangeException); }
            );

            AssertExtensions.ShouldThrow(
                () => { StorageKeyBuilder.CombineStorageKeys("a", ""); },
                (exception) => { return exception.GetType() == typeof(ArgumentOutOfRangeException); }
            );

            AssertExtensions.ShouldThrow(
                () => { StorageKeyBuilder.CombineStorageKeys((IHashingStrategy) null, null, null); },
                (exception) => { return exception.GetType() == typeof(ArgumentNullException); }
            );

            AssertExtensions.ShouldThrow(
                () => { StorageKeyBuilder.CombineStorageKeys("a", "b", "c", null, "d"); },
                (exception) => { return exception.GetType() == typeof(ArgumentOutOfRangeException); }
            );
        }

        [TestMethod]
        public void CombineStorageKeys_Basic()
        {
            string storageKey1 = "aa";
            string storageKey2 = "bbbb";

            string combinedKey = StorageKeyBuilder.CombineStorageKeys(storageKey1, storageKey2);
            Assert.IsTrue(string.CompareOrdinal(combinedKey, "AA-BBBB") == 0, "CombineStorageKeys('{0}', '{1}') == '{2}'", storageKey1, storageKey2, combinedKey);

            combinedKey = StorageKeyBuilder.CombineStorageKeys(HashingStrategies.FNV1aStrategy, storageKey1, storageKey2);
            Assert.IsTrue(string.CompareOrdinal(combinedKey, "AA-BBBB") == 0, "CombineStorageKeys('{0}', '{1}') == '{2}'", storageKey1, storageKey2, combinedKey);
        }

        [TestMethod]
        public void CombineStorageKeys_Multiple()
        {
            string storageKey1 = "aa";
            string storageKey2 = "bbbb";
            string storageKey3 = "Ddfe";
            string storageKey4 = "fhdjskahjfkhkxw";

            string combinedKey = StorageKeyBuilder.CombineStorageKeys(storageKey1, storageKey2, storageKey3, storageKey4);
            Assert.IsTrue(string.CompareOrdinal(combinedKey, "AA-BBBB-DDFE-FHDJSKAHJFKHKXW") == 0, "CombineStorageKeys('{0}', '{1}', '{2}, '{3}) == '{4}'", storageKey1, storageKey2, storageKey3, storageKey4, combinedKey);

            StorageKeyBuilder.CombineStorageKeys(HashingStrategies.FNV1aStrategy, storageKey1, storageKey2, storageKey3, storageKey4);
            Assert.IsTrue(string.CompareOrdinal(combinedKey, "AA-BBBB-DDFE-FHDJSKAHJFKHKXW") == 0, "CombineStorageKeys(FNV1aStrategy, '{0}', '{1}', '{2}, '{3}) == '{4}'", storageKey1, storageKey2, storageKey3, storageKey4, combinedKey);
        }

        [TestMethod]
        public void CombineStorageKeys_Multiple_LongString()
        {
            string storageKey1 = "aa";
            string storageKey2 = "bbbb";
            string storageKey3 = ExtendStringBeyond("Ddfe", 256);
            string storageKey4 = ExtendStringBeyond("AbCd-09_21:996$#%$#&^%&^$*\\/?\t\n\r|-" + "\u0012\u0020\u001E\u001F - : \u007F\u0080\u009E\u009F", 256);

            string combinedKey = StorageKeyBuilder.CombineStorageKeys(storageKey1, storageKey2, storageKey3, storageKey4);
            Assert.IsTrue(string.CompareOrdinal(combinedKey, "AA-BBBB-DDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFE-ABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D:12:20:1E:1F:20:2D:20:3A:20:7F:80:9E:9FABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D:12:20:1E:1F:20:2D:20:3A:20:7F:|70FCCA647254E3B6") == 0, "CombineStorageKeys('{0}', '{1}', '{2}, '{3}) == '{4}'", storageKey1, storageKey2, storageKey3, storageKey4, combinedKey);

            combinedKey = StorageKeyBuilder.CombineStorageKeys(HashingStrategies.FNV1aStrategy, storageKey1, storageKey2, storageKey3, storageKey4);
            Assert.IsTrue(string.CompareOrdinal(combinedKey, "AA-BBBB-DDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFEDDFE-ABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D:12:20:1E:1F:20:2D:20:3A:20:7F:80:9E:9FABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D:12:20:1E:1F:20:2D:20:3A:20:7F:|2A94CA362618BE66") == 0, "CombineStorageKeys(FNV1aStrategy, '{0}', '{1}', '{2}, '{3}) == '{4}'", storageKey1, storageKey2, storageKey3, storageKey4, combinedKey);
        }

        [TestMethod]
        public void NormalizeStorageKeys_NullOrEmptyKey_Throws()
        {
            AssertExtensions.ShouldThrow(
                () => { StorageKeyBuilder.NormalizeStorageKey(null, null); },
                (exception) => { return exception.GetType() == typeof(ArgumentNullException); }
            );

            AssertExtensions.ShouldThrow(
                () => { StorageKeyBuilder.NormalizeStorageKey(null, ""); },
                (exception) => { return exception.GetType() == typeof(ArgumentNullException); }
            );

            AssertExtensions.ShouldThrow(
                () => { StorageKeyBuilder.NormalizeStorageKey(new FastHashStrategy(), null); },
                (exception) => { return exception.GetType() == typeof(ArgumentOutOfRangeException); }
            );

            AssertExtensions.ShouldThrow(
                () => { StorageKeyBuilder.NormalizeStorageKey(new FastHashStrategy(), ""); },
                (exception) => { return exception.GetType() == typeof(ArgumentOutOfRangeException); }
            );
        }

        [TestMethod]
        public void NormalizeStorageKey_Basic()
        {
            string storageKey = "AbCd-09_21:996$#%$#&^%&^$*\\/?\t\n\r|-";
            string normalizedKey = StorageKeyBuilder.NormalizeStorageKey(storageKey);
            Assert.IsTrue(string.CompareOrdinal(normalizedKey, "ABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D") == 0, "NormalizeStorageKey('{0}') == '{1}'", storageKey, normalizedKey);

            storageKey = "\u0012\u0020\u001E\u001F - : \u007F\u0080\u009E\u009F";
            normalizedKey = StorageKeyBuilder.NormalizeStorageKey(storageKey);
            Assert.IsTrue(string.CompareOrdinal(normalizedKey, ":12:20:1E:1F:20:2D:20:3A:20:7F:80:9E:9F") == 0, "NormalizeStorageKey('{0}') == '{1}'", storageKey, normalizedKey);

            storageKey = "AbCd-09_21:996$#%$#&^%&^$*\\/?\t\n\r|-";
            normalizedKey = StorageKeyBuilder.NormalizeStorageKey(HashingStrategies.FNV1aStrategy, storageKey);
            Assert.IsTrue(string.CompareOrdinal(normalizedKey, "ABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D") == 0, "NormalizeStorageKey(HashingStrategies.FNV1aStrategy, '{0}') == '{1}'", storageKey, normalizedKey);

            storageKey = "\u0012\u0020\u001E\u001F - : \u007F\u0080\u009E\u009F";
            normalizedKey = StorageKeyBuilder.NormalizeStorageKey(HashingStrategies.FNV1aStrategy, storageKey);
            Assert.IsTrue(string.CompareOrdinal(normalizedKey, ":12:20:1E:1F:20:2D:20:3A:20:7F:80:9E:9F") == 0, "NormalizeStorageKey(HashingStrategies.FNV1aStrategy, '{0}') == '{1}'", storageKey, normalizedKey);;
        }

        [TestMethod]
        public void NormalizeStorageKey_LongString()
        {
            string storageKey = "AbCd-09_21:996$#%$#&^%&^$*\\/?\t\n\r|-" + "\u0012\u0020\u001E\u001F - : \u007F\u0080\u009E\u009F";
            storageKey = ExtendStringBeyond(storageKey, 512);
            string normalizedKey = StorageKeyBuilder.NormalizeStorageKey(storageKey);
            Assert.IsTrue(string.CompareOrdinal(normalizedKey, "ABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D:12:20:1E:1F:20:2D:20:3A:20:7F:80:9E:9FABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D:12:20:1E:1F:20:2D:20:3A:20:7F:80:9E:9FABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D:12:20:1E:1F:20:2D:20:3A:20:7F:80:9E:9FABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D:12:20:1E:1F:20:2D:20:3A:20:7F:80:9E:9FABCD:2D09:5F21:3A99|A2C9A7BB64B5BD33") == 0, "NormalizeStorageKey('{0}') == '{1}'", storageKey, normalizedKey);

            normalizedKey = StorageKeyBuilder.NormalizeStorageKey(HashingStrategies.FNV1aStrategy, storageKey);
            Assert.IsTrue(string.CompareOrdinal(normalizedKey, "ABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D:12:20:1E:1F:20:2D:20:3A:20:7F:80:9E:9FABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D:12:20:1E:1F:20:2D:20:3A:20:7F:80:9E:9FABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D:12:20:1E:1F:20:2D:20:3A:20:7F:80:9E:9FABCD:2D09:5F21:3A996:24:23:25:24:23:26:5E:25:26:5E:24:2A:5C:2F:3F:09:0A:0D:7C:2D:12:20:1E:1F:20:2D:20:3A:20:7F:80:9E:9FABCD:2D09:5F21:3A99|50A20DAB9D104008") == 0, "NormalizeStorageKey(HashingStrategies.FNV1aStrategy, '{0}') == '{1}'", storageKey, normalizedKey);
        }

        private static string ExtendStringBeyond(string stringToExtend, int size)
        {
            if (string.IsNullOrEmpty(stringToExtend))
            {
                throw new ArgumentOutOfRangeException("A non null or empty string must be provided", nameof(stringToExtend));
            }

            int currentLength = stringToExtend.Length;
            StringBuilder longStringBuilder = new StringBuilder(stringToExtend);
            do
            {
                longStringBuilder.Append(stringToExtend);
                currentLength += stringToExtend.Length;
            } while (currentLength < size);

            return longStringBuilder.ToString();
        }
    }
}