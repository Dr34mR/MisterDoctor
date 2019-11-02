using System;
using System.Linq;
using LiteDB;
using SubstitutionBot.Classes;

namespace SubstitutionBot.Helpers
{
    internal static class DbHelper
    {
        private const string DbName = "Datastore.db";

        private const string CollectionTokens = "tokens";
        private const string CollectionSettings = "settings";
        private const string CollectionWords = "words";

        public static Token TokenGet()
        {
            using (var db = new LiteDatabase(DbName))
            {
                // Get Token Collection

                var collection = db.GetCollection<Token>(CollectionTokens);
                var tokens = collection.FindAll();

                return tokens.SingleOrDefault() ?? new Token();
            }
        }

        public static void TokenSet(Token token)
        {
            using (var db = new LiteDatabase(DbName))
            {
                // Get Token Collection

                var collection = db.GetCollection<Token>(CollectionTokens);
                var tokens = collection.FindAll();

                var origToken = tokens.SingleOrDefault();

                if (origToken == null)
                {
                    collection.Insert(token);
                }
                else
                {
                    origToken.Username = token.Username;
                    origToken.UserOAuthKey = token.UserOAuthKey;

                    collection.Update(origToken);
                }
            }
        }

        public static AppSettings AppSettingsGet()
        {
            using (var db = new LiteDatabase(DbName))
            {
                // Get Token Collection

                var collection = db.GetCollection<AppSettings>(CollectionSettings);
                var settings = collection.FindAll();

                return settings.SingleOrDefault() ?? new AppSettings();
            }
        }

        public static void AppSettingsSet(AppSettings appSetting)
        {
            using (var db = new LiteDatabase(DbName))
            {
                // Get Token Collection

                var collection = db.GetCollection<AppSettings>(CollectionSettings);
                var settings = collection.FindAll();
                var origSetting = settings.SingleOrDefault();

                if (origSetting == null)
                {
                    collection.Insert(appSetting);
                }
                else
                {
                    origSetting.ChannelName = appSetting.ChannelName;
                    origSetting.AutoConnect = appSetting.AutoConnect;

                    collection.Update(origSetting);
                }
            }
        }

        public static Word[] WordsGet()
        {
            using (var db = new LiteDatabase(DbName))
            {
                // Get Token Collection

                var collection = db.GetCollection<Word>(CollectionSettings);
                return collection.FindAll().ToArray();
            }
        }

        public static void WordAdd(Word word)
        {
            if (word == null) return;
            if (string.IsNullOrEmpty(word.Value.Trim())) return;

            using (var db = new LiteDatabase(DbName))
            {
                var collection = db.GetCollection<Word>(CollectionWords);
                var existing = collection.Find(i => i.Value.Equals(word.Value, StringComparison.CurrentCultureIgnoreCase));
                if (existing.Any()) return;

                collection.Insert(word);
            }
        }

        public static void WordDelete(Word word)
        {
            if (word == null) return;

            using (var db = new LiteDatabase(DbName))
            {
                var collection = db.GetCollection<Word>(CollectionWords);
                collection.Delete(word.Id);
            }
        }

        public static Word WordRandom()
        {
            var words = WordsGet();

            if (words.Length == 0) return null;

            var rnd = new Random();
            var index = rnd.Next(words.Length);

            return words[index];
        }
    }
}
