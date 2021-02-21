using System;
using System.Collections.Generic;
using System.Linq;
using MisterDoctor.Plugins.Classes;
using MisterDoctor.Plugins.Enums;

namespace Plugin.Goodbot
{
    public class Goodbot : MisterDoctor.Plugins.Plugin
    {
        public override Guid UniqueId { get; } = Guid.Parse("9FC4245E-6F1F-41DB-886F-7CA18A6BD32E");
        
        public override string Name { get; } = "Goodbot";
        
        public override string Version { get; } = "1.0";
        
        public override string Author { get; } = "Mister Doctor";

        public override string Description { get; } = "A bot that replies with a positive / negative message when people mention it";
        
        public override void Initialize()
        {
            _settings = GetDefaultSettings();
            IsInitialized = true;
        }

        public override bool IsInitialized { get; set; }

        public override Settings GetDefaultSettings()
        {
            return new()
            {
                new Setting
                {
                    Name = "Good Response",
                    Type = SettingType.String,
                    ValueString = "(✿◠‿◠) $user",
                    Description = "Message to send if a user mentions the bot in a positive light"
                },
                new Setting
                {
                    Name = "Bad Response",
                    Type = SettingType.String,
                    ValueString = "ಥ_ಥ sorry $user",
                    Description = "Message to send if a user mentions the bot in a negative light"
                },
                new Setting
                {
                    Name = "Positive Words",
                    Type = SettingType.StringList,
                    ValueStringList = new List<string>
                    {
                        "yes",
                        "plz",
                        "nice",
                        "good",
                        "goodbot"
                    },
                    Description = "The positive words to detect"
                },
                new Setting
                {
                    Name = "Negative Words",
                    Type = SettingType.StringList,
                    ValueStringList = new List<string>
                    {
                        "no",
                        "why",
                        "bad",
                        "badbot"
                    },
                    Description = "The negative words to detect"
                }
            };
        }
        
        public override Settings Settings => _settings ?? GetDefaultSettings();

        private Settings _settings;

        public override void LoadSettings(Settings settings)
        {
            if (settings == null) return;
            _settings = settings;
            _settings.TrimValues();
        }

        public override bool ReceiveMessage(DigestMessage message)
        {
            if (!IsInitialized) return false;
            if (message?.Message == null) return false;

            var words = message.Message.Where(i => i.IsWord).ToList();

            var hasBotMention = words.Any(i => i.HasValue(message.BotUsername));

            if (!hasBotMention) return false;

            var goodWords = GetPositiveWords();
            var badWords = GetNegativeWords();

            var hasGood = goodWords.Any(goodWord => words.Any(i => i.HasValue(goodWord)));
            var hasBad = badWords.Any(badWord => words.Any(i => i.HasValue(badWord)));

            if (hasGood)
            {
                SendMessage(GetPositiveReply(), message);
                return true;
            }

            // ReSharper disable once InvertIf
            // ReSharper disable once RedundantJumpStatement

            if (hasBad)
            {
                SendMessage(GetNegativeReply(), message);
                return true;
            }

            return false;
        }

        private IEnumerable<string> GetNegativeWords()
        {
            var blankList = new List<string>();
            
            var hasValue = _settings.FirstOrDefault(i => i.Name == "Negative Words");
            
            if (hasValue == null) return blankList;

            return hasValue.ValueStringList ?? blankList;
        }

        private IEnumerable<string> GetPositiveWords()
        {
            var blankList = new List<string>();

            var hasValue = _settings.FirstOrDefault(i => i.Name == "Positive Words");

            if (hasValue == null) return blankList;

            return hasValue.ValueStringList ?? blankList;
        }

        private string GetPositiveReply()
        {
            var hasValue = _settings.FirstOrDefault(i => i.Name == "Good Response");

            if (hasValue == null) return string.Empty;

            return hasValue.ValueString ?? string.Empty;
        }

        private string GetNegativeReply()
        {
            var hasValue = _settings.FirstOrDefault(i => i.Name == "Bad Response");

            if (hasValue == null) return string.Empty;

            return hasValue.ValueString ?? string.Empty;
        }
    }
}
