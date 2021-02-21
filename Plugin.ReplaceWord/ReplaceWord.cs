using System;
using System.Linq;
using System.Threading;
using MisterDoctor.Plugins.Classes;
using MisterDoctor.Plugins.Enums;

namespace Plugin.ReplaceWord
{
    public class ReplaceWord : MisterDoctor.Plugins.Plugin
    {
        public override Guid UniqueId { get; } = Guid.Parse("0CE818FC-858B-41B1-8825-E8DED4B68650");
        
        public override string Name { get; } = "Replace Word";
        
        public override string Version { get; } = "1.0";
        
        public override string Author { get; } = "Mister Doctor";

        public override string Description { get; } = "A bot that replaces a word with another";
        
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
                    Name = "Words",
                    Type = SettingType.KeyValueList,
                    ValueKeyValues = new KeyValues
                    {
                        new()
                        {
                            Key = "LUL",
                            Value = "KEKW"
                        }

                    },
                    Description = "Words that will be replaced by another"
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

            var substitutions = GetSubList();

            foreach (var copyItem in copyOfParts)
            {
                var matchingSub = substitutions.FirstOrDefault(i => i.Key.Equals(copyItem.Value, StringComparison.CurrentCultureIgnoreCase));
                if (matchingSub == null) continue;
                copyItem.Value = matchingSub.Value;
            }

            var messageToSend = copyOfParts.ToString();
            if (messageToSend == message.Message.ToString()) return false;

            // SEt the cooldown

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

        private KeyValues GetSubList()
        {
            return Settings.FirstOrDefault(i => i.Name == "Words")?.ValueKeyValues ?? new KeyValues();
        }
    }
}
