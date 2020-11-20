using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Anagram
{
    public static class WordList
    {
        public static IEnumerable<string> GetWordList(string path, string phrase)
        {
            var words = File.ReadAllLines(path);
            Console.WriteLine($"Initial Word Count: {words.Length}");
            return words.PruneList(phrase, true);
        }

        //prune dictionary to disallow unavailable letters / counts, reducing set dramatically.
        public static IEnumerable<string> PruneList(this IEnumerable<string> wordList, string phrase, bool output = false)
        {
            var formattedPhrase = phrase.SanitizeAndOrderString();
            if (output)
                Console.WriteLine($"Formatted Phrase: {formattedPhrase}");
            var validLetters = new string(formattedPhrase.Distinct().ToArray());
            if (output)
                Console.WriteLine($"Valid Letters: {validLetters}");
            var validWords = wordList.Where(x => x.Length <= formattedPhrase.Length && x.Except(validLetters).Count() == 0).Distinct();
            var charCounts = formattedPhrase.GroupBy(c => c).Select(x => new { Key = x.Key, Count = x.Count() }).ToDictionary(x => x.Key, x => x.Count);
            foreach (char c in charCounts.Keys)
            {
                validWords = validWords.Where(word => word.Count(ch => ch == c) <= charCounts[c]);
            }
            if (output)
                Console.WriteLine($"Valid Word Count: {validWords.Count()}");
            return validWords.OrderByDescending(x => x.Length);
        }
    }
}