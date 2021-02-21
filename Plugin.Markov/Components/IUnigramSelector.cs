using System.Collections.Generic;

namespace Plugin.Markov.Components
{
    public interface IUnigramSelector <TUnigram>
    {
        TUnigram SelectUnigram(IEnumerable<TUnigram> ngrams);
    }
}