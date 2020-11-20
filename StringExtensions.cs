using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Anagram
{
    public static class StringExtensions
    {
        public static string SubtractWord(this string input, string word)
        {
            foreach (var c in word)
            {
                if (input.IndexOf(c) == -1)
                    return null;
                input = input.RemoveFirst(c);
            }
            return input;
        }

        public static string SanitizeAndOrderString(this string phrase)
        {
            return new string(phrase?.Replace(" ", "")?.ToCharArray()?.OrderBy(x => x)?.ToArray());
        }

        public static bool IsValid(this string phrase, string toCompare)
        {
            var thisCharCounts = phrase.GroupBy(c => c).Select(x => new { Key = x.Key, Count = x.Count() }).ToDictionary(x => x.Key, x => x.Count);
            var toCompareCounts = toCompare.GroupBy(c => c).Select(x => new { Key = x.Key, Count = x.Count() }).ToDictionary(x => x.Key, x => x.Count);
            foreach (var c in toCompareCounts.Keys.Intersect(thisCharCounts.Keys))
            {
                if (toCompareCounts[c] < thisCharCounts[c])
                    return false;
            }
            return true;
        }

        private static string RemoveFirst(this string text, char search)
        {
            int pos = text.IndexOf(search);
            return text.Substring(0, pos) + text.Substring(pos + 1);
        }
        public static string GetMD5(this string s)
        {
            using (MD5 md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(s));
                return string.Concat(hash.Select(x => x.ToString("X2"))).ToLowerInvariant();
            }
        }
    }
}