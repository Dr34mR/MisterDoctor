using System;
using System.Linq;
using System.Threading;
using MisterDoctor.Plugins.Classes;
using MisterDoctor.Plugins.Enums;

namespace Plugin.Commands
{
    public class Command : MisterDoctor.Plugins.Plugin
    {
        public override Guid UniqueId { get; } = Guid.Parse("31AB7321-9A57-4C35-A6FB-53B5DE9AC139");
        
        public override string Name { get; } = "Commands";
        
        public override string Version { get; } = "1.0";
        
        public override string Author { get; } = "Mister Doctor";

        public override string Description { get; } = "A bot that handles basic command response messages";
        
        public override void Initialize()
        {
            _settings = GetDefaultSettings();
            IsInitialized = true;
        }

        public override bool IsInitialized { get; set; }

        private readonly Random _randGen = new (Thread.CurrentThread.ManagedThreadId);

        public override Settings GetDefaultSettings()
        {
            return new()
            {
                new Setting
                {
                    Name = "Commands",
                    Type = SettingType.KeyValueList,
                    ValueKeyValues = new KeyValues
                    {
                        new ()
                        {
                            Key = "hello",
                            Value = "Hello $user"
                        }
                    },
                    Description = "Command and values"
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

            if (message.Message.Count < 2) return false;
            if (!message.Message[0].Value.Equals("!")) return false;

            var command = message.Message[1].Value.ToLower();

            var commands = GetCommands();

            var responses = commands.Where(i => i.Key.Equals(command, StringComparison.CurrentCultureIgnoreCase)).ToList();

            // Check the number of possible replies

            switch (responses.Count)
            {
                case 0:
                {
                    return false;
                }
                case 1:
                {
                    SendMessage(responses[0].Value, message);
                    break;
                }
                default:
                {
                    // If there is more than one response then pick a random one
                    var randVal = _randGen.Next(responses.Count);
                    SendMessage(responses[randVal].Value, message);
                    break;
                }
            }

            return true;
        }

        private KeyValues GetCommands()
        {
           return Settings.FirstOrDefault(i => i.Name == "Commands")?.ValueKeyValues ?? new KeyValues();
        }
    }
}
