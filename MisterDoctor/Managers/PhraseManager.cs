using System.Collections.Generic;
using System.Linq;
using MisterDoctor.Classes;
using MisterDoctor.Helpers;

namespace MisterDoctor.Managers
{
    internal class PhraseManager
    {
        private bool _initialized;
        private readonly Dictionary<string, string> _phraseList = new Dictionary<string, string>();

        private static PhraseManager Manager { get; } = new PhraseManager();

        private PhraseManager()
        {

        }

        internal static void Initialize()
        {
            if (Manager._initialized) return;

            var phraseList = DbHelper.SubstitutionsGet();
            if (phraseList == null) return;

            foreach (var phrase in phraseList)
            {
                Manager._phraseList.Add(phrase.Find, phrase.Reply);
            }

            Manager._initialized = true;
        }

        internal static void AddPhrase(string find, string replyWith)
        {
            var cleanFind = find.ToLower().Trim();
            var cleanReply = replyWith.Trim();

            if (string.IsNullOrEmpty(cleanFind)) return;
            if (string.IsNullOrEmpty(cleanReply)) return;

            if (Manager._phraseList.ContainsKey(cleanFind)) return;

            Manager._phraseList.Add(cleanFind, cleanReply);
            DbHelper.SubstitutionAdd(cleanFind, cleanReply);
        }

        internal static void RemovePhrase(Substitution substitution)
        {
            if (substitution == null) return;
            if (!Manager._phraseList.ContainsKey(substitution.Find)) return;

            Manager._phraseList.Remove(substitution.Find);
            DbHelper.SubstitutionDelete(substitution);
        }

        internal static IEnumerable<Substitution> GetPhrases()
        {
            return DbHelper.SubstitutionsGet();
        }

        internal static string CheckMessage(MessageParts message)
        {
            if (message == null) return string.Empty;

            var cleanedWords = message.Where(i => i.IsWord).Select(word => word.Value.ToLower()).ToList();

            foreach (var cleanWord in cleanedWords)
            {
                if (!Manager._phraseList.TryGetValue(cleanWord, out var reply)) continue;
                return reply;
            }

            return string.Empty;
        }
    }
}
