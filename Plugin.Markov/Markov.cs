using System;
using System.Collections.Generic;
using System.Linq;
using MisterDoctor.Plugins.Classes;
using MisterDoctor.Plugins.Enums;
using Plugin.Markov.TokenisationStrategies;

namespace Plugin.Markov
{
    public class Markov : MisterDoctor.Plugins.Plugin
    {
        public override Guid UniqueId { get; } = Guid.Parse("870CE7C5-AE67-4012-8FC2-5A351C8E7086");
        
        public override string Name { get; } = "Markov";
        
        public override string Version { get; } = "1.0";
        
        public override string Author { get; } = "Mister Doctor";

        public override string Description { get; } = "Generates sentences based on a markov chain. Uses https://github.com/chriscore/MarkovSharp";
        
        public override void Initialize()
        {
            _settings = GetDefaultSettings();
            IsInitialized = true;
        }

        private readonly Queue<string> _messageQueue = new ();

        public override bool IsInitialized { get; set; }

        public override Settings GetDefaultSettings()
        {
            return new()
            {
                new Setting
                {
                    Name = "Markov Command",
                    Type = SettingType.String,
                    ValueString = "!markov",
                    Description = "Command used to manually trigger a markov sentence"
                },
                new Setting
                {
                    Name = "Minimum Chat",
                    Type = SettingType.Int,
                    ValueInt = 20,
                    Description = "Minimum number of chat messages before a chain will be generated"
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

        private readonly object _threadLock = new();

        public override bool ReceiveMessage(DigestMessage message)
        {
            if (!IsInitialized) return false;
            if (message?.Message == null) return false;

            var wholeMessage = message.Message.ToString();

            var minimumCount = GetMinimumCount();

            lock (_threadLock)
            {
                if (!wholeMessage.StartsWith("!"))
                {
                    _messageQueue.Enqueue(wholeMessage);

                    while (_messageQueue.Count > minimumCount * 5)
                    {
                        _messageQueue.Dequeue();
                    }
                }

                var command = GetCommand();
                if (string.IsNullOrEmpty(command)) return false;
                if (!wholeMessage.StartsWith(command)) return false;

                if (_messageQueue.Count < minimumCount)
                {
                    SendMessage("Still collecting messages", message);
                    return true;
                }

                var queueItems = _messageQueue.ToArray();

                var model = new StringMarkov();
                model.Learn(queueItems);

                var response = model.Walk().First();

                if (string.IsNullOrEmpty(response)) return false;

                SendMessage(response, message);
            }
            
            return true;
        }

        private string GetCommand()
        {
            return Settings.FirstOrDefault(i => i.Name == "Markov Command")?.ValueString ?? string.Empty;
        }

        private int GetMinimumCount()
        {
            return Settings.FirstOrDefault(i => i.Name == "Minimum Chat")?.ValueInt ?? 20;
        }
    }
}
