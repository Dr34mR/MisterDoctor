using System;
using System.Collections.Generic;
using System.Linq;
using MisterDoctor.Plugins.Classes;
using MisterDoctor.Plugins.Enums;

namespace Plugin.Welcome
{
    public class Welcome : MisterDoctor.Plugins.Plugin
    {
        public override Guid UniqueId { get; } = Guid.Parse("4B8B29A0-7CB1-41D9-965E-CCF8A13CE8CF");
        
        public override string Name { get; } = "Welcome Bot";
        
        public override string Version { get; } = "1.0";
        
        public override string Author { get; } = "Mister Doctor";

        public override string Description { get; } = "A bot that welcomes users it sees for the first time to the chat";
        
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
                    Name = "Generic Greeting",
                    Type = SettingType.String,
                    ValueString = "Hi $user, welcome to the stream!",
                    Description = "Generic welcome message to send to users that don't have a personal message"
                },
                new Setting
                {
                    Name = "Personal",
                    Type = SettingType.KeyValueList,
                    Description = "Personalised messages for users",
                    ValueKeyValues = new KeyValues
                    {
                        new()
                        {
                            Key = "Username",
                            Value = "Whoah! Username is in chat!"
                        },
                    }
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

        private HashSet<string> UsersSeen { get; } = new ();

        public override bool ReceiveMessage(DigestMessage message)
        {
            if (!IsInitialized) return false;
            if (message?.Message == null) return false;

            var userAsLower = message.FromAccount.ToLower();
            if (string.IsNullOrEmpty(userAsLower)) return false;

            // Check to see if the user has been seen before

            if (UsersSeen.Contains(userAsLower)) return false;

            UsersSeen.Add(userAsLower);

            // First check for personal messages

            var personalUsers = Settings.FirstOrDefault(i => i.Name == "Personal");
            if (personalUsers != null)
            {
                var personalMatch = personalUsers.ValueKeyValues.FirstOrDefault(i =>
                    i.Key.Equals(userAsLower, StringComparison.CurrentCultureIgnoreCase));

                if (!string.IsNullOrEmpty(personalMatch?.Value))
                {
                    SendMessage(personalMatch.Value, message);
                    return false;
                }
            }

            // Now check for a generic response

            var genericResponse = Settings.FirstOrDefault(i => i.Name == "Generic Greeting");

            // ReSharper disable once InvertIf
            if (!string.IsNullOrEmpty(genericResponse?.ValueString))
            {
                SendMessage(genericResponse.ValueString, message);
                return false;
            }

            return false;
        }
    }
}
