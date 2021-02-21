using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.Markov.Components
{
    public class UnweightedRandomUnigramSelector<T> : IUnigramSelector<T>
    {
        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            return ngrams.GroupBy(a => a)
                .Select(a => a.FirstOrDefault())
                .OrderBy(_ => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}