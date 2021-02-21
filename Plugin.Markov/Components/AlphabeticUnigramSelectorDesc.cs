using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedMember.Global

namespace Plugin.Markov.Components
{
    public class AlphabeticUnigramSelectorDesc<T> : IUnigramSelector<T>
    {
        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            return ngrams
                .OrderByDescending(a => a)
                .FirstOrDefault();
        }
    }
}