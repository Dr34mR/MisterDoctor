using System;
using System.Linq;
using LiteDB;
using MisterDoctor.Classes;

namespace MisterDoctor.Helpers
{
    internal static class DbHelper
    {
        private const string DbName = "Datastore.db";

        private const string CollectionTokens = "tokens";
        private const string CollectionSettings = "settings";
        private const string CollectionWords = "words";
        private const string CollectionIgnore = "ignore";
        private const string CollectionSubstitution = "subs";

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

                    origSetting.ProcChance = appSetting.ProcChance;
                    origSetting.CoolDown = appSetting.CoolDown;

                    origSetting.MaxMessageSize = appSetting.MaxMessageSize;

                    origSetting.CommandIgnore = appSetting.CommandIgnore;
                    origSetting.CommandUnignore = appSetting.CommandUnignore;

                    origSetting.GoodBot = appSetting.GoodBot;
                    origSetting.BadBot = appSetting.BadBot;

                    collection.Update(origSetting);
                }
            }
        }

        public static Word[] WordsGet()
        {
            using (var db = new LiteDatabase(DbName))
            {
                if (!db.CollectionExists(CollectionWords)) return null;

                var collection = db.GetCollection<Word>(CollectionWords);
                return collection.FindAll().OrderBy(i => i.Value).ToArray();
            }
        }

        public static void WordAdd(string word)
        {
            if (string.IsNullOrEmpty(word.Trim())) return;

            var objWord = new Word
            {
                Value = word.Trim()
            };
            
            using (var db = new LiteDatabase(DbName))
            {
                var collection = db.GetCollection<Word>(CollectionWords);
                var existing = collection.Find(i => i.Value.Equals(word.Trim(), StringComparison.CurrentCultureIgnoreCase));
                if (existing.Any()) return;

                collection.Insert(objWord);
            }
        }

        public static void WordDelete(Word word)
        {
            if (word == null) return;

            using (var db = new LiteDatabase(DbName))
            {
                if (!db.CollectionExists(CollectionWords)) return;

                var collection = db.GetCollection<Word>(CollectionWords);
                collection.Delete(word.Id);
            }
        }

        public static Word WordRandom()
        {
            var words = WordsGet();

            if (words == null) return null;
            if (words.Length == 0) return null;

            var rnd = new Random();
            var index = rnd.Next(words.Length);

            return words[index];
        }



        public static User[] IgnoreGet()
        {
            using (var db = new LiteDatabase(DbName))
            {
                if (!db.CollectionExists(CollectionIgnore)) return null;

                var collection = db.GetCollection<User>(CollectionIgnore);
                return collection.FindAll().OrderBy(i => i.Username).ToArray();
            }
        }

        public static void IgnoreAdd(string username)
        {
            var fixedLower = username.Trim().ToLower();
            if (string.IsNullOrEmpty(fixedLower)) return;

            var objWord = new User
            {
                Username = fixedLower
            };

            using (var db = new LiteDatabase(DbName))
            {
                var collection = db.GetCollection<User>(CollectionIgnore);
                var existing = collection.Find(i => i.Username.Equals(fixedLower, StringComparison.CurrentCultureIgnoreCase));
                if (existing.Any()) return;

                collection.Insert(objWord);
            }
        }

        public static void IgnoreDelete(string username)
        {
            if (username == null) return;
            var fixedLower = username.ToLower().Trim();

            using (var db = new LiteDatabase(DbName))
            {
                if (!db.CollectionExists(CollectionIgnore)) return;

                var collection = db.GetCollection<User>(CollectionIgnore);

                var existing = collection.Find(i => i.Username == fixedLower).ToArray();
                if (existing.Length == 0) return;
                var currentUser = existing.Single();

                collection.Delete(currentUser.Id);
            }
        }

        public static Substitution[] SubstitutionsGet()
        {
            using (var db = new LiteDatabase(DbName))
            {
                if (!db.CollectionExists(CollectionSubstitution)) return null;

                var collection = db.GetCollection<Substitution>(CollectionSubstitution);
                return collection.FindAll().OrderBy(i => i.Find).ToArray();
            }
        }

        public static void SubstitutionAdd(string find, string reply)
        {
            if (string.IsNullOrEmpty(find.Trim())) return;
            if (string.IsNullOrEmpty(reply.Trim())) return;

            var sub = new Substitution
            {
                Find = find.ToLower().Trim(),
                Reply = reply.Trim()
            };

            using (var db = new LiteDatabase(DbName))
            {
                var collection = db.GetCollection<Substitution>(CollectionSubstitution);
                var existing = collection.Find(i => i.Find.Equals(find.Trim(), StringComparison.CurrentCultureIgnoreCase));
                if (existing.Any()) return;

                collection.Insert(sub);
            }
        }

        public static void SubstitutionDelete(Substitution substitution)
        {
            if (substitution == null) return;

            using (var db = new LiteDatabase(DbName))
            {
                if (!db.CollectionExists(CollectionSubstitution)) return;

                var collection = db.GetCollection<Substitution>(CollectionSubstitution);
                collection.Delete(substitution.Id);
            }
        }
    }
}
