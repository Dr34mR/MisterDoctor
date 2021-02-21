using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedMember.Global

namespace Plugin.Markov.Components
{
    public class LeastPopularUnigramSelector<T> : IUnigramSelector<T>
    {
        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            return ngrams
                .GroupBy(a => a).OrderByDescending(a => a.Count())
                .FirstOrDefault()
                .FirstOrDefault();
        }
    }
}