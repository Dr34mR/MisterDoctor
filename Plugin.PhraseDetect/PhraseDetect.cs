using System;
using System.Linq;
using MisterDoctor.Plugins.Classes;
using MisterDoctor.Plugins.Enums;

namespace Plugin.PhraseDetect
{
    public class PhraseDetect : MisterDoctor.Plugins.Plugin
    {
        public override Guid UniqueId { get; } = Guid.Parse("F8C9ED2D-C244-41C8-82BD-B11BC62B9833");
        
        public override string Name { get; } = "Phrase Detect";
        
        public override string Version { get; } = "1.0";
        
        public override string Author { get; } = "Mister Doctor";

        public override string Description { get; } = "If a word is found, will respond with a phrase";
        
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
                    Name = "Cooldown (sec)",
                    Type = SettingType.Int,
                    ValueInt = 30,
                    Description = "Cooldown in seconds between messages"
                },
                new Setting
                {
                    Name = "Phrases",
                    Type = SettingType.KeyValueList,
                    ValueKeyValues = new KeyValues
                    {
                        new()
                        {
                            Key = "joe",
                            Value = "Joe Momma! Eyooo!"
                        }

                    },
                    Description = "Find word, respond with..."
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

            if (!message.IsFollower) return false;

            var phrases = GetPhrases();
            var response = string.Empty;

            foreach (var copyItem in message.Message)
            {
                var matchingSub = phrases.FirstOrDefault(i => i.Key.Equals(copyItem.Value, StringComparison.CurrentCultureIgnoreCase));
                if (matchingSub == null) continue;
                response = matchingSub.Value.Trim();
                if (string.IsNullOrEmpty(response)) continue;
                break;
            }

            if (string.IsNullOrEmpty(response)) return false;

            // Set the cooldown

            _coolDownTime = DateTime.Now.AddSeconds(GetCooldown());

            SendMessage(response, message);

            return true;
        }

        private int GetCooldown()
        {
            return Settings.FirstOrDefault(i => i.Name == "Cooldown (sec)")?.ValueInt ?? 60;
        }

        private KeyValues GetPhrases()
        {
            return Settings.FirstOrDefault(i => i.Name == "Phrases")?.ValueKeyValues ?? new KeyValues();
        }
    }
}
