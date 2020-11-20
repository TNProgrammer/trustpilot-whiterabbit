using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Anagram
{
    class Program
    {
        private static Stopwatch sw;
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            const string phrase = "poultry outwits ants";
            Program.StartTimer();
            var availableLetters = phrase.SanitizeAndOrderString();
            var validWordList = WordList.GetWordList("./wordlist", phrase);
            SubsetSum.Sum(cts, validWordList.ToList(), availableLetters, 3);
            Program.StopTimer();
        }
        private static void StartTimer()
        {
            sw = Stopwatch.StartNew();
        }

        internal static void StopTimer()
        {
            sw.Stop();
            Console.WriteLine("All hashes found, terminating execution.");
            Environment.Exit(0);
        }

        internal static void ReportTime(string message)
        {
            Console.WriteLine($"{message} after {sw.ElapsedTicks} ticks ({sw.Elapsed.TotalSeconds}seconds)");
        }
    }
}
