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

        public static Token GetToken()
        {
            using (var db = new LiteDatabase(DbName))
            {
                // Get Token Collection

                var collection = db.GetCollection<Token>(CollectionTokens);
                var tokens = collection.FindAll();

                return tokens.SingleOrDefault() ?? new Token();
            }
        }

        public static void SetToken(Token token)
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

        public static AppSettings GetAppSettings()
        {
            using (var db = new LiteDatabase(DbName))
            {
                // Get Token Collection

                var collection = db.GetCollection<AppSettings>(CollectionSettings);
                var settings = collection.FindAll();

                return settings.SingleOrDefault() ?? new AppSettings();
            }
        }

        public static void SetAppSettings(AppSettings appSetting)
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
    }
}
