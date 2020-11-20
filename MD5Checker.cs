using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace Anagram
{
    static class MD5Checker
    {
        private static List<string> md5s = new List<string>
            {
                "e4820b45d2277f3844eac66c903e84be",
                "23170acc097c24edb98fc5488ab033fe",
                "665e5bcb0c20062fe8abaaf4628bb154"
            };

        private static ConcurrentDictionary<string, object> checkedPermutations = new ConcurrentDictionary<string, object>();
        private static ConcurrentDictionary<string, object> foundMd5Sums = new ConcurrentDictionary<string, object>();

        private static MD5 md5 = MD5.Create();
        public static void Check(string phrase)
        {
            var permutations = GetPermutations(phrase);
            if (!checkedPermutations.ContainsKey(phrase))
            {
                foreach (var permutation in permutations)
                {
                    checkedPermutations.TryAdd(permutation, null);
                    var md5sum = permutation.GetMD5();
                    var foundMd5 = md5s.SingleOrDefault(x => md5sum == x);
                    if (foundMd5 != null)
                    {
                        if (foundMd5Sums.TryAdd(foundMd5, null))
                        {
                            Program.ReportTime($"Found {foundMd5} - \"{permutation}\"");
                        }
                        if (foundMd5Sums.Count == md5s.Count)
                        {
                            Program.StopTimer();
                        }
                    }
                }
            }
        }

        private static IEnumerable<string> GetPermutations(string phrase)
        {
            var words = phrase.Split(" ");
            var list = new List<IList<string>>();
            var permutations = Permute(words, 0, words.Length - 1, list);
            return permutations.Select(x => string.Join(" ", x)).ToList();
        }

        private static IList<IList<string>> Permute(string[] words, int start, int end, IList<IList<string>> list)
        {
            if (start == end)
            {
                list.Add(new List<string>(words));
            }
            else
            {
                for (var i = start; i <= end; i++)
                {
                    Swap(ref words[start], ref words[i]);
                    Permute(words, start + 1, end, list);
                    Swap(ref words[start], ref words[i]);
                }
            }

            return list;
        }

        private static void Swap(ref string a, ref string b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

    }
}