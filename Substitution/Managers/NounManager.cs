using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Substitution.Managers
{
    internal class NounManager
    {
        private bool _initialized;
        private static NounManager Manager { get; } = new NounManager();

        private NounManager()
        {
            
        }

        private readonly HashSet<string> _nouns = new HashSet<string>();

        internal static void Initialize()
        {
            if (Manager._initialized) return;

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

            Manager._initialized = true;
        }

        internal static bool IsNoun(string word)
        {
            var modded = word.ToLower().Trim();
            return !string.IsNullOrEmpty(modded) && Manager._nouns.Contains(modded);
        }
    }
}
