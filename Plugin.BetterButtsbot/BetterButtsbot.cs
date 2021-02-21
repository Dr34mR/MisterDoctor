using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MisterDoctor.Plugins.Classes;
using MisterDoctor.Plugins.Enums;

namespace Plugin.BetterButtsbot
{
    public class BetterButtsbot : MisterDoctor.Plugins.Plugin
    {
        public override Guid UniqueId { get; } = Guid.Parse("67EDAE16-9BF0-4A01-BD79-8F0E85B54977");

        public override string Name { get; } = "Better Buttsbot";
        
        public override string Version { get; } = "1.0";
        
        public override string Author { get; } = "Mister Doctor";

        public override string Description { get; } = "Random noun substitution from a list of words";
        
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
                    Name = "Proc Chance",
                    Type = SettingType.Int,
                    ValueInt = 30,
                    Description = "Percent (%) chance to proc from messages"
                },
                new Setting
                {
                    Name = "Cooldown (sec)",
                    Type = SettingType.Int,
                    ValueInt = 30,
                    Description = "Cooldown in seconds between random word substitution"
                },
                new Setting
                {
                    Name = "Max Words",
                    Type = SettingType.Int,
                    ValueInt = 12,
                    Description = "Maximum words in message for it to trigger on"
                },
                new Setting
                {
                    Name = "Wordlist",
                    Type = SettingType.StringList,
                    ValueStringList = new List<string>
                    {
                        "butt"
                    },
                    Description = "The list of words that will swap out for a noun"
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

        private DateTime? _coolDownTime;
        private bool _procNext;
        private readonly Random _randGenerator = new(Thread.CurrentThread.ManagedThreadId);

        public override bool ReceiveMessage(DigestMessage message)
        {
            if (!IsInitialized) return false;
            if (message?.Message == null) return false;
            if (message.IsIgnored) return false;

            if (_coolDownTime.HasValue)
            {
                if (_coolDownTime > DateTime.Now) return false;
                _coolDownTime = null;
            }

            if (!_procNext)
            {
                var procChance = GetProcChance();
                var newValue = _randGenerator.Next(1, 101);
                if (newValue > procChance) return false;
            }

            // If we passed the 'Random Check' then make sure it triggers next time

            _procNext = true;
            
            if (!message.IsFollower) return false;
            
            var copyOfParts = message.Message.CreateCopy();

            if (copyOfParts.WordCount() > GetMaxWords()) return false;
            if (!copyOfParts.HasNoun()) return false;
            var nounIndex = copyOfParts.NounIndexes();

            // Randomly pick a noun index + get a random word from the wordlist

            var replaceIndex = _randGenerator.Next(nounIndex.Count);
            var getReplacement = RandomWord();

            // Substitute if the replacement is ok

            if (!string.IsNullOrEmpty(getReplacement))
            {
                copyOfParts.ReplaceWord(nounIndex[replaceIndex], getReplacement);
            }

            var messageToSend = copyOfParts.ToString();
            if (messageToSend == message.Message.ToString()) return false;

            // Set the cooldown

            _coolDownTime = DateTime.Now.AddSeconds(GetCooldown());
            _procNext = false;

            SendMessage(messageToSend, message);

            return true;
        }

        private int GetProcChance()
        {
            return Settings.FirstOrDefault(i => i.Name == "Proc Chance")?.ValueInt ?? 101;
        }

        private int GetCooldown()
        {
            return Settings.FirstOrDefault(i => i.Name == "Cooldown (sec)")?.ValueInt ?? 60;
        }

        private int GetMaxWords()
        {
            return Settings.FirstOrDefault(i => i.Name == "Max Words")?.ValueInt ?? 12;
        }

        private string RandomWord()
        {
            var allWords = Settings.FirstOrDefault(i => i.Name == "Wordlist")?.ValueStringList ?? new List<string>();

            if (allWords.Count <= 0) return string.Empty;

            var index = _randGenerator.Next(allWords.Count);
            return allWords[index];
        }
    }
}
