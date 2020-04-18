using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using MisterDoctor.Classes;
using TwitchLib.Client.Models;

namespace MisterDoctor.Managers
{
    internal class RussianManager
    {
        public static event EventHandler<RussianMessageArgs> SendMessage;
        public static event EventHandler<RussianTimeoutArgs> SendTimeout;
        
        private static readonly RussianManager Instance = new RussianManager();

        // Logic Variables

        private const string Trigger = "!sol";
        private readonly List<string> _usernames = new List<string>();
        
        private static bool _cooldown;
        private static DateTime? _cooldownEnds;
        private static bool _enabled;

        private const int CooldownMinutes = 20;
        private const int SignupSeconds = 70;

        private const int MsBetweenMsg = 1200;

        private BackgroundWorker _bgWorker;
        private readonly object _threadLock = new object();

        private RussianManager()
        {
            
        }
        
        // Digesting

        internal static void DigestMessage(ChatMessage message)
        {
            if (!message.Message.Equals(Trigger, StringComparison.CurrentCultureIgnoreCase)) return;

            if (_cooldown)
            {
                if (_cooldownEnds == null) return;

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

                TriggerMessage($"Shit out-of luck cooldown has {displayString} left");
                return;
            }

            // Check if the message comming in is a trigger

            var triggerStart = false;
            lock (Instance._threadLock)
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
                Instance._bgWorker = new BackgroundWorker {WorkerSupportsCancellation = true};
                Instance._bgWorker.DoWork += Worker_DoWork;
                Instance._bgWorker.RunWorkerCompleted += Worker_TimeToRoulette;
                Instance._bgWorker.RunWorkerAsync();
            }
            if (Instance._usernames.Contains(message.Username)) return;
            Instance._usernames.Add(message.Username);
        }

        private static void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(SignupSeconds * 1000);
        }

        private static void Worker_TimeToRoulette(object sender, RunWorkerCompletedEventArgs e)
        {
            _cooldown = true;

            Instance._bgWorker.Dispose();

            var userList = Instance._usernames.Distinct().ToList();

            var timeSpan = new TimeSpan();

            foreach (var _ in userList)
            {
                timeSpan = timeSpan.Add(new TimeSpan(0, 0, 30));
            }

            TriggerMessage("Now time to choose ...");
            Thread.Sleep(2 * MsBetweenMsg);

            var secs = timeSpan.TotalSeconds;
            var mins = timeSpan.TotalMinutes;

            TriggerMessage($"{userList.Count} entered for a timeout of {secs} seconds ({mins} minutes)");
            Thread.Sleep(2 * MsBetweenMsg);
            TriggerMessage("...");
            Thread.Sleep(2 *MsBetweenMsg);
            TriggerMessage("...");

            var random = new Random();
            var value = random.Next(0, userList.Count);
            var user = userList[value];

            Thread.Sleep(2 * MsBetweenMsg);
            TriggerMessage($"Fate has chosen : {user}");
            Thread.Sleep(2 * MsBetweenMsg);
            TriggerTimeout(user, timeSpan);

            Instance._usernames.Clear();

            _cooldownEnds = DateTime.Now.AddMinutes(CooldownMinutes);

            Instance._bgWorker = new BackgroundWorker {WorkerSupportsCancellation = true};
            Instance._bgWorker.DoWork += Worker_Cooldown;
            Instance._bgWorker.RunWorkerCompleted += Worker_CooldownCompleted;
            Instance._bgWorker.RunWorkerAsync();
        }

        private static void Worker_Cooldown(object sender, DoWorkEventArgs e)
        {
            for (var i = 0; i < CooldownMinutes; i++)
            {
                Thread.Sleep(60000);
            }
        }

        private static void Worker_CooldownCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Instance._bgWorker.Dispose();

            _enabled = false;
            _cooldownEnds = null;
            _cooldown = false;

            TriggerMessage($"Shit out-of luck cooldown is over - start another one with {Trigger} Jebaited");
        }

        private static void TriggerStartingMessages()
        {
            TriggerMessage("SHIT OUT-OF LUCK HAS STARTED! PogChamp Winner gets timed out Jebaited");
            Thread.Sleep(MsBetweenMsg);
            TriggerMessage($"Type {Trigger} to join - Careful though, the more that enter, the higher the timeout.");
        }

        // Event Invokation

        private static void TriggerMessage(string message)
        {
            var args = new RussianMessageArgs
            {
                Message = message
            };

            var handler = SendMessage;
            handler?.Invoke(null, args);
        }
        
        private static void TriggerTimeout(string username, TimeSpan timeSpan)
        {
            var args = new RussianTimeoutArgs
            {
                Username = username,
                Message = "We salute you!",
                Duration = timeSpan
            };

            var handler = SendTimeout;
            handler?.Invoke(null, args);
        }
    }
}