using System;
using System.Collections.Generic;
using MisterDoctor.Plugins;
using MisterDoctor.Plugins.Classes;

namespace MisterDoctor.Extensions
{
    public static class MessageExtensions
    {
        public static void UpdateWildcards(this MessageParts sendMessage, DigestMessage originalMessage)
        {
            var dict = new Dictionary<string, string>
            {
                {"$user", originalMessage.FromAccount},
                {"$channel", originalMessage.ChannelName},
                {"$bot", originalMessage.BotUsername},
                {"$time", DateTime.Now.ToString("h:mm tt")},
                {"$day", DateTime.Now.ToString("d")}
            };

            foreach (var thing in sendMessage)
            {
                if (!dict.TryGetValue(thing.Value.ToLower(), out var replace)) continue;
                thing.Value = replace;
            }
        }
    }
}
