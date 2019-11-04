using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SubstitutionBot.Managers
{
    internal class NounManager
    {
        private static NounManager Manager { get; } = new NounManager();

        private NounManager()
        {
            
        }

        private readonly HashSet<string> _nouns = new HashSet<string>();

        internal static void Initialize()
        {
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var nounFile = Path.Combine(currentDirectory, "nounlist.txt");

            if (!File.Exists(nounFile)) throw new Exception("nounlist.txt not found");

            var nouns = File.ReadAllLines(nounFile).ToList();

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var noun in nouns)
            {
                var lowerVariant = noun.Trim().ToLower();
                if (string.IsNullOrEmpty(lowerVariant)) continue;
                if (Manager._nouns.Contains(lowerVariant)) continue;
                Manager._nouns.Add(lowerVariant);
            }
            Manager._nouns.TrimExcess();
        }

        internal static List<int> NounIndexes(IEnumerable<string> words)
        {
            var currentIndex = 0;
            var returnIndexes = new List<int>();

            foreach (var word in words)
            {
                var lowerVariant = word.ToLower();
                if (Manager._nouns.Contains(lowerVariant)) returnIndexes.Add(currentIndex);
                currentIndex += 1;
            }

            return returnIndexes;
        }
    }
}
