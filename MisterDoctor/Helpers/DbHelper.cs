using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using MisterDoctor.Classes;
using MisterDoctor.Plugins;
using MisterDoctor.Plugins.Classes;

namespace MisterDoctor.Helpers
{
    internal class DbHelper
    {
        private const string DbName = "Datastore.db";

        private const string CollectionApplication = "application";

        private const string CollectionPlugins = "plugins";

        private const string CollectionStates = "pluginStates";

        private const string CollectionIgnored = "ignored";

        public static ConnectionSettings ReadConnectionSettings()
        {
            using var db = new LiteDatabase(DbName);

            var collection = db.GetCollection<ConnectionDto>(CollectionApplication);
            var settings = collection.FindAll();

            var settingDto = settings.SingleOrDefault() ?? new ConnectionDto();

            return settingDto.ToSetting();
        }

        public static void SaveConnectionSettings(ConnectionSettings settings)
        {
            using var db = new LiteDatabase(DbName);

            var collection = db.GetCollection<ConnectionDto>(CollectionApplication);
            var dbSettings = collection.FindAll();

            var origSetting = dbSettings.SingleOrDefault();

            if (origSetting == null)
            {
                var dto = new ConnectionDto(settings);
                collection.Insert(dto);
            }
            else
            {
                var dto = new ConnectionDto(settings)
                {
                    Id = origSetting.Id
                };
                collection.Update(dto);
            }
        }

        public static Settings ReadPluginSettings(Plugin plugin)
        {
            using var db = new LiteDatabase(DbName);

            var collection = db.GetCollection<SettingsDto>(CollectionPlugins);
            var allSettings = collection.FindAll();

            var matchingSetting = allSettings.FirstOrDefault(i => i.Id == plugin.UniqueId);

            var returnSettings = matchingSetting == null ? plugin.GetDefaultSettings() : matchingSetting.Settings;

            returnSettings = plugin.CleanSettings(returnSettings);
            returnSettings.TrimValues();

            return returnSettings;
        }

        public static void SavePluginSettings(Plugin plugin, Settings pluginSettings)
        {
            using var db = new LiteDatabase(DbName);

            pluginSettings.TrimValues();
            pluginSettings = plugin.CleanSettings(pluginSettings);

            var collection = db.GetCollection<SettingsDto>(CollectionPlugins);
            var allSettings = collection.FindAll();

            var matchingSetting = allSettings.FirstOrDefault(i => i.Id == plugin.UniqueId);

            if (matchingSetting == null)
            {
                var newDto = new SettingsDto(plugin, pluginSettings);
                collection.Insert(newDto);
            }
            else
            {
                matchingSetting.Settings = pluginSettings;
                collection.Update(matchingSetting);
            }
        }

        public static PluginStates ReadPluginStates()
        {
            using var db = new LiteDatabase(DbName);

            var collection = db.GetCollection<PluginState>(CollectionStates);
            var allStates = collection.FindAll();

            var returnStates = new PluginStates();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var state in allStates.OrderBy(i => i.Sequence))
            {
                returnStates.Add(state);
            }

            return returnStates;
        }

        public static void SavePluginState(PluginStates pluginStates)
        {
            using var db = new LiteDatabase(DbName);

            var collection = db.GetCollection<PluginState>(CollectionStates);

            var onlyValidStates = pluginStates.Where(i => i.Id != Guid.Empty);

            foreach (var state in onlyValidStates)
            {
                collection.Upsert(state);
            }
        }

        public static List<IgnoredUser> GetIgnoredUsers()
        {
            using var db = new LiteDatabase(DbName);

            var collection = db.GetCollection<IgnoredUser>(CollectionIgnored);

            var allIgnored = collection.FindAll().ToList();

            return allIgnored;
        }

        public static void SaveIgnoredUsers(IEnumerable<IgnoredUser> ignoreList)
        {
            using var db = new LiteDatabase(DbName);

            var cleanList = new List<IgnoredUser>();
            foreach(var ignoreItem in ignoreList)
            {
                ignoreItem.Username = ignoreItem.Username.Trim();
                if (string.IsNullOrEmpty(ignoreItem.Username)) continue;
                cleanList.Add(ignoreItem);
            }

            var collection = db.GetCollection<IgnoredUser>(CollectionIgnored);

            collection.DeleteAll();

            foreach (var user in cleanList)
            {
                collection.Upsert(user);
            }
        }
    }
}
