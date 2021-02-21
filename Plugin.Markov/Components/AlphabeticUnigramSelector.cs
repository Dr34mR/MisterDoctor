using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedMember.Global

namespace Plugin.Markov.Components
{
    public class AlphabeticUnigramSelector<T> : IUnigramSelector<T>
    {
        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            return ngrams
                .OrderBy(a => a)
                .FirstOrDefault();
        }
    }
}
