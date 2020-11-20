using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Anagram
{
    public class SubsetSum
    {
        public static void Sum(CancellationTokenSource cts, List<string> hashes, string targetHash, int maxDepth = 3)
        {
            var results = new List<string>();
            try
            {
                ParallelOptions po = new ParallelOptions();
                po.CancellationToken = cts.Token;
                po.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Press enter key to exit");
                    Console.Read();
                    cts.Cancel();
                    Environment.Exit(0);
                });

                Parallel.ForEach(hashes, po, (hash =>
                 {
                     SumRecursive(hashes, targetHash, targetHash.SubtractWord(hash), 0, maxDepth, hash);
                     po.CancellationToken.ThrowIfCancellationRequested();
                 }));
            }
            catch (OperationCanceledException e)
            {

                Console.WriteLine(e.Message);
            }
            finally
            {
                cts.Dispose();
            }
        }

        private static void SumRecursive(List<string> hashes, string originalTargetHash, string targetHash, int depth = 0, int maxDepth = 5, string result = "")
        {
            if (depth >= maxDepth) return;
            if (result == null) return;
            var sanitizedResult = result.SanitizeAndOrderString();
            if (!sanitizedResult.IsValid(originalTargetHash)) return;

            if (targetHash == "" || (sanitizedResult == originalTargetHash && sanitizedResult.Length == originalTargetHash.Length))
            {
                MD5Checker.Check(result.TrimStart(' '));
                return;
            }
            var subset = hashes.PruneList(targetHash).ToArray();
            for (var i = 0; i < subset.Length; i++)
            {
                var hash = subset[i];
                var tempTargetHash = originalTargetHash?.SubtractWord(sanitizedResult)?.SubtractWord(hash);
                if (tempTargetHash == null)
                    continue;
                if (result != null)
                {
                    if (tempTargetHash.IsValid(originalTargetHash) && sanitizedResult.IsValid(originalTargetHash))
                    {
                        var tempResult = (result + " " + hash);
                        if (tempTargetHash == "" && tempResult.SanitizeAndOrderString().Length == originalTargetHash.Length)
                            MD5Checker.Check(tempResult.TrimStart(' '));
                        else
                            SumRecursive(hashes, originalTargetHash, tempTargetHash, depth + 1, maxDepth, tempResult);
                    }
                }
            }
        }
    }
}