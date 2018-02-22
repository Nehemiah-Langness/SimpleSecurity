using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class Extensions
    {
        /// <summary>
        /// Returns UTF8 bytes for the string
        /// </summary>
        public static byte[] ToBytes(this string text) => text == null ? new byte[0] : Encoding.UTF8.GetBytes(text);

        /// <summary>
        /// Returns a string from UTF8 bytes
        /// </summary>
        public static string ToUtf8(this IEnumerable<byte> bytes) => Encoding.UTF8.GetString(bytes.ToArray());

        /// <summary>
        /// Returns "bytes" in Base64 string format
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase64(this IEnumerable<byte> bytes) => Convert.ToBase64String(bytes.ToArray());

        public static bool SlowEquals(this IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            var differences = a.Count ^ b.Count; // Check Lengths

            for (var i = 0; i < a.Count && i < b.Count; i++)
                differences |= a[i] ^ b[i];      // Check Values

            return differences == 0;
        }

        public static bool FastEquals(this IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            if (a.Count != b.Count) return false;

            for (var i = 0; i < a.Count && i < b.Count; i++)
                if (a[i] != b[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Returns the last "count" characters of "text"
        /// </summary>
        public static string Last(this string text, int count)
        {
            if (count > text.Length) throw new ArgumentOutOfRangeException(nameof(count), "Count must be less than the length of the string");
            return new string(text.Skip(text.Length - count).Take(count).ToArray());
        }

        /// <summary>
        /// Split a read only list of T into two lists
        /// </summary>
        public static BisectedList<T> Bisect<T>(this IReadOnlyList<T> list, int firstSegmentLength)
        {
            if (firstSegmentLength > list.Count) throw new ArgumentOutOfRangeException(nameof(firstSegmentLength));

            return new BisectedList<T>(list.Take(firstSegmentLength).ToArray(), list.Skip(firstSegmentLength).Take(list.Count - firstSegmentLength).ToArray());
        }
    }
}
