using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.Markov.Models;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace Plugin.Markov.TokenisationStrategies
{
    public interface IMarkovStrategy<TPhrase, TUnigram>
    {
        Type UnigramType { get; }

        Type PhraseType { get; }

        IEnumerable<TUnigram> SplitTokens(TPhrase input);
        
        TPhrase RebuildPhrase(IEnumerable<TUnigram> tokens);

        void Learn(IEnumerable<TPhrase> phrases, bool ignoreAlreadyLearnt = true);
        
        void Learn(TPhrase phrase);
        
        void Retrain(int newLevel);
        
        IEnumerable<TPhrase> Walk(int lines = 1, TPhrase seed = default);
        
        List<TUnigram> GetMatches(TPhrase input);

        TUnigram GetTerminatorUnigram();

        TUnigram GetPrepadUnigram();

        IEnumerable<StateStatistic<TUnigram>> GetStatistics();

        ChainPhraseProbability<TPhrase> GetFit(TPhrase test);

        double GetTransitionProbabilityUnigram(TPhrase currentState, TUnigram nextStates);

        double GetTransitionProbabilityPhrase(TPhrase currentState, TPhrase nextStates);
    }


    public class SubstringMarkov : GenericMarkov<string, char?>
    {
        public SubstringMarkov(int level = 2) : base(level)
        {

        }

        public override IEnumerable<char?> SplitTokens(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
            {
                return new List<char?> { GetPrepadUnigram() };
            }
            
            return phrase.Select(c => new char?(c));
        }

        public override string RebuildPhrase(IEnumerable<char?> tokens)
        {
            var transformed = tokens
                .Where(t => t != null)
                .Select(t => t.Value).ToArray();
            
            // ReSharper disable once PossibleInvalidOperationException
            return new string(transformed).Replace(new string(new[] { GetPrepadUnigram().Value }), "");
        }

        public override char? GetTerminatorUnigram()
        {
            return null;
        }

        public override char? GetPrepadUnigram()
        {
            return '\0';
        }
    }
}
