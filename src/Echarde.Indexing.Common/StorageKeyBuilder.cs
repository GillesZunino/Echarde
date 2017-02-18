// -----------------------------------------------------------------------------------
// Copyright 2016, Gilles Zunino
// -----------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Echarde.Indexing.Hashing;

namespace Echarde.Indexing
{
    /// <summary>
    /// Utilities to generate normalized Azure Table Storage Partition Keys and Row Keys.
    /// </summary>
    public static class StorageKeyBuilder
    {
        /// <summary>
        /// Combines strings in a manner suitable for use as Azure Table Storage Partition Key or Row Key.
        /// </summary>
        /// <param name="storageKey1">First storage keys to combine.</param>
        /// <param name="storageKey2">Second storage keys to combine.</param>
        /// <param name="storageKeys">Additional storage keys to combine.</param>
        /// <returns>String suitable for use as Azure Table Storage Partition Key or Row Key.</returns>
        public static string CombineStorageKeys(string storageKey1, string storageKey2, params string[] storageKeys)
        {
            return CombineStorageKeys(HashingStrategies.FastHashStrategy, storageKey1, storageKey2, storageKeys);
        }

        /// <summary>
        /// Combines strings in a manner suitable for use as Azure Table Storage Partition Key or Row Key.
        /// </summary>
        /// <param name="hashingStrategy">Instance of <see cref="IHashingStrategy"/> to use when trimming the storage key.</param>
        /// <param name="storageKey1">First storage keys to combine.</param>
        /// <param name="storageKey2">Second storage keys to combine.</param>
        /// <param name="storageKeys">Additional storage keys to combine.</param>
        /// <returns>String suitable for use as Azure Table Storage Partition Key or Row Key.</returns>
        public static string CombineStorageKeys(IHashingStrategy hashingStrategy, string storageKey1, string storageKey2, params string[] storageKeys)
        {
            if (hashingStrategy == null)
            {
                throw new ArgumentNullException(nameof(hashingStrategy), "Hashing strategy must be provided");
            }

            if (string.IsNullOrEmpty(storageKey1))
            {
                throw new ArgumentOutOfRangeException(nameof(storageKey1), "Storage key must not be null or empty.");
            }

            if (string.IsNullOrEmpty(storageKey2))
            {
                throw new ArgumentOutOfRangeException(nameof(storageKey2), "Storage key must not be null or empty.");
            }

            string[] escapedKeys = new string[storageKeys.Length + 2];

            escapedKeys[0] = EscapeStorageKey(storageKey1);
            escapedKeys[1] = EscapeStorageKey(storageKey2);

            for (int index = 0; index < storageKeys.Length; index++)
            {
                string currentKey = storageKeys[index];

                if (string.IsNullOrEmpty(currentKey))
                {
                    throw new ArgumentOutOfRangeException("When combining storage keys, all keys must not be null or empty.");
                }

                escapedKeys[index + 2] = EscapeStorageKey(currentKey);
            }

            string untrimmedKey = string.Join("-", escapedKeys);
            return TrimStorageKey(untrimmedKey, hashingStrategy);
        }

        /// <summary>
        /// Normalizes a string to make it suitable for use as Azure Table Storage Partition Key or Row Key.
        /// </summary>
        /// <param name="storageKey">Storage key to normalize.</param>
        /// <returns>String suitable for use as Azure Table Storage Partition Key or Row Key.</returns>
        public static string NormalizeStorageKey(string storageKey)
        {
            return NormalizeStorageKey(HashingStrategies.FastHashStrategy, storageKey);
        }

        /// <summary>
        /// Normalizes a string to make it suitable for use as Azure Table Storage Partition Key or Row Key.
        /// </summary>
        /// <param name="hashingStrategy">Instance of <see cref="IHashingStrategy"/> to use when trimming the storage key.</param>
        /// <param name="storageKey">Storage key to normalize.</param>
        /// <returns>String suitable for use as Azure Table Storage Partition Key or Row Key.</returns>
        public static string NormalizeStorageKey(IHashingStrategy hashingStrategy, string storageKey)
        {
            if (hashingStrategy == null)
            {
                throw new ArgumentNullException(nameof(hashingStrategy), "Hashing strategy must be provided");
            }

            if (string.IsNullOrEmpty(storageKey))
            {
                throw new ArgumentOutOfRangeException(nameof(storageKey), "Storage key must not be null or empty.");
            }

            string untrimmedKey = EscapeStorageKey(storageKey);
            return TrimStorageKey(untrimmedKey, hashingStrategy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string EscapeStorageKey(string storageKey)
        {
            //
            // It is necessary to encode the storage key because:
            //
            //  1 - Not all characters are allowed in Row Key / Partition Keys - See https://msdn.microsoft.com/library/azure/dd179338.aspx
            //      * - The forward slash (/) character
            //      * - The backslash (\) character
            //      * - The number sign (#) character
            //      * - The question mark(?) character
            //      * - Control characters from U+0000 to U+001F, including:
            //      * - The horizontal tab(\t) character
            //      * - The linefeed(\n) character
            //      * - The carriage return (\r) character
            //      * - Control characters from U + 007F to U+009F
            //
            //  2 - We reserve ":", "-" and "|" for internal use
            //
            StringBuilder escapedStorageKey = new StringBuilder(storageKey.Length);
            foreach (char c in storageKey)
            {
                UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
                switch (unicodeCategory)
                {
                    case UnicodeCategory.DecimalDigitNumber:
                    case UnicodeCategory.UppercaseLetter:
                        escapedStorageKey.Append(c);
                        break;

                    case UnicodeCategory.LowercaseLetter:
                        escapedStorageKey.Append(char.ToUpperInvariant(c));
                        break;

                    default:
                        escapedStorageKey.Append(EscapeStorageKeyCharacter(c));
                        break;
                }
            }

            return escapedStorageKey.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string TrimStorageKey(string storageKey, IHashingStrategy hashingStrategy)
        {
            const int StorageKeyLimitInCharacters = (1 * 1024) / 2;
            const int StorageKeyTrimPaddingInCharacters = 17;

            // The storage key whas been encoded so that all characters fit in the BMP - This makes it possible to assume 1 char == 2 bytes
            if (storageKey.Length > StorageKeyLimitInCharacters)
            {
                // Replace everything above StorageKeyLimitInCharacters with a hash of the entire key
                ulong hash64 = hashingStrategy.Hash(storageKey);
                return string.Concat(storageKey.Substring(0, StorageKeyLimitInCharacters - StorageKeyTrimPaddingInCharacters), "|", hash64.ToString("X16"));
            }

            return storageKey;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string EscapeStorageKeyCharacter(char character)
        {
            const ushort HighCharacterCodePoint = 0x100;

            ushort ordinalValue = character;
            string escapePattern = ordinalValue < HighCharacterCodePoint ? ":{0:X2}" : "::{0:X4}";
            return string.Format(CultureInfo.InvariantCulture, escapePattern, ordinalValue);
        }
    }
}
