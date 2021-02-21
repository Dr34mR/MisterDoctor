using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.Markov.Components
{
    public class WeightedRandomUnigramSelector<T> : IUnigramSelector<T>
    {
        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            return ngrams.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
        }
    }
}