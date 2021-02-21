using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using MisterDoctor.Plugins.Classes;
using MisterDoctor.Plugins.Enums;

namespace Plugin.Roulette
{
    public class Roulette : MisterDoctor.Plugins.Plugin
    {
        public override Guid UniqueId { get; } = Guid.Parse("92ADDBC9-4456-4833-AAF1-A92F5A394BA0");
        
        public override string Name { get; } = "Sh*t out-of luck";
        
        public override string Version { get; } = "1.0";
        
        public override string Author { get; } = "Mister Doctor";

        public override string Description { get; } = "Times out the winner of Roulette :)";
        
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
                    Name = "Command",
                    Type = SettingType.String,
                    ValueString = "!sol",
                    Description = "The command used to start / join roulette"
                },
                new Setting
                {
                    Name = "Cooldown (min)",
                    Type = SettingType.Int,
                    ValueInt = 20,
                    Description = "Cooldown in minutes between each rouette"
                },
                new Setting
                {
                    Name = "Signup (sec)",
                    Type = SettingType.Int,
                    ValueInt = 70,
                    Description = "Signup time in seconds for users to join"
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

        private readonly List<string> _usernames = new ();
        private bool _cooldown;
        private static DateTime? _cooldownEnds;
        private static bool _enabled;

        private const int MsBetweenMsg = 1200;

        private BackgroundWorker _bgWorker;
        private readonly object _threadLock = new ();

        public override bool ReceiveMessage(DigestMessage message)
        {
            if (!IsInitialized) return false;
            if (message?.Message == null) return false;

            var command = GetCommand();
            if (string.IsNullOrEmpty(command)) return false;

            var wholeMessage = message.Message.ToString();
            if (!wholeMessage.StartsWith(command, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            if (_cooldown)
            {
                if (_cooldownEnds == null) return false;

                var timeSpan = _cooldownEnds.Value - DateTime.Now;
                var roundedMinutes = Convert.ToInt32(timeSpan.TotalMinutes);
                
                string displayString;

                if (roundedMinutes >= 1)
                {
                    displayString = $"{roundedMinutes} minutes";
                }
                else
                {
                    var roundedSeconds = Convert.ToInt32(timeSpan.TotalSeconds);
                    displayString = $"{roundedSeconds} seconds";
                }

                SendMessage($"Shit out-of luck cooldown has {displayString} left", message);
                return true;
            }

            // Check if the message comming in is a trigger

            var triggerStart = false;
            lock (_threadLock)
            {
                if (!_enabled)
                {
                    triggerStart = true;
                    _enabled = true;
                }
            }

            if (triggerStart)
            {
                TriggerStartingMessages();
                _bgWorker = new BackgroundWorker
                {
                    WorkerSupportsCancellation = true
                };
                _bgWorker.DoWork += Worker_DoWork;
                _bgWorker.RunWorkerCompleted += Worker_TimeToRoulette;
                _bgWorker.RunWorkerAsync();
            }
            if (_usernames.Contains(message.FromAccount)) return true;
            _usernames.Add(message.FromAccount);

            return true;
        }

        private void TriggerStartingMessages()
        {
            SendMessage("SHIT OUT-OF LUCK HAS STARTED! PogChamp Winner gets timed out Jebaited");
            Thread.Sleep(MsBetweenMsg);
            SendMessage($"Type {GetCommand()} to join - Careful though, the more that enter, the higher the timeout.");
        }

        private void Worker_TimeToRoulette(object sender, RunWorkerCompletedEventArgs e)
        {
            _cooldown = true;

            _bgWorker.Dispose();

            var userList = _usernames.Distinct().ToList();

            var timeSpan = new TimeSpan();

            foreach (var _ in userList)
            {
                timeSpan = timeSpan.Add(new TimeSpan(0, 0, 30));
            }

            SendMessage("Now time to choose ...");
            Thread.Sleep(2 * MsBetweenMsg);

            var secs = timeSpan.TotalSeconds;
            var mins = timeSpan.TotalMinutes;

            SendMessage($"{userList.Count} entered for a timeout of {secs} seconds ({mins} minutes)");
            Thread.Sleep(2 * MsBetweenMsg);
            SendMessage("...");
            Thread.Sleep(2 *MsBetweenMsg);
            SendMessage("...");

            var random = new Random();
            var value = random.Next(0, userList.Count);
            var user = userList[value];

            Thread.Sleep(2 * MsBetweenMsg);
            SendMessage($"Fate has chosen : {user}");

            Thread.Sleep(2 * MsBetweenMsg);
            TriggerTimeout(user, timeSpan);

            _usernames.Clear();

            _cooldownEnds = DateTime.Now.AddMinutes(GetCooldownMins());

            _bgWorker = new BackgroundWorker {WorkerSupportsCancellation = true};
            _bgWorker.DoWork += Worker_Cooldown;
            _bgWorker.RunWorkerCompleted += Worker_CooldownCompleted;
            _bgWorker.RunWorkerAsync();
        }

        private void Worker_Cooldown(object sender, DoWorkEventArgs e)
        {
            var cooldownMins = GetCooldownMins();

            for (var i = 0; i < cooldownMins; i++)
            {
                Thread.Sleep(60000);
            }
        }

        private void Worker_CooldownCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _bgWorker.Dispose();

            _enabled = false;
            _cooldownEnds = null;
            _cooldown = false;

            SendMessage($"Shit out-of luck cooldown is over - start another one with {GetCommand()} Jebaited");
        }

        private void TriggerTimeout(string user, TimeSpan timeSpan)
        {
            var args = new TimeoutArgs
            {
                Username = user,
                Message = "We salute you!",
                Duration = timeSpan
            };

            SendTimeout(args);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var seconds = GetSignupSecs();

            Thread.Sleep(seconds * 1000);
        }

        public int GetSignupSecs()
        {
            return Settings.FirstOrDefault(i => i.Name == "Signup (sec)")?.ValueInt ?? 70;
        }

        public int GetCooldownMins()
        {
            return Settings.FirstOrDefault(i => i.Name == "Cooldown (min)")?.ValueInt ?? 20;
        }

        public string GetCommand()
        {
            return Settings.FirstOrDefault(i => i.Name == "Command")?.ValueString ?? string.Empty;
        }
    }
}
