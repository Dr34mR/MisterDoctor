using System;
using System.Collections.Generic;
using MisterDoctor.Classes;
using MisterDoctor.Helpers;

namespace MisterDoctor.Managers
{
    internal class CommandManager
    {
        private bool _initialized;
        private readonly Dictionary<string, List<string>> _commandList = new Dictionary<string, List<string>>();

        private static CommandManager Manager { get; } = new CommandManager();

        private CommandManager()
        {

        }

        internal static void Initialize()
        {
            if (Manager._initialized) return;

            var commandList = DbHelper.CommandsGet();
            if (commandList == null) return;

            foreach (var command in commandList)
            {
                switch (Manager._commandList.ContainsKey(command.Cmd))
                {
                    case true:

                        var responses = Manager._commandList[command.Cmd];
                        responses.Add(command.Response);
                        break;

                    case false:

                        var newList = new List<string> { command.Response };
                        Manager._commandList.Add(command.Cmd, newList);
                        break;
                }
            }

            Manager._initialized = true;
        }

        internal static void AddCommand(string command, string response)
        {
            var cleanCommand = command.ToLower().Trim();
            var cleanResponse = response.Trim();

            if (string.IsNullOrEmpty(cleanCommand)) return;
            if (string.IsNullOrEmpty(cleanResponse)) return;

            switch (Manager._commandList.ContainsKey(cleanCommand))
            {
                case true:

                    var responses = Manager._commandList[cleanCommand];
                    responses.Add(cleanResponse);
                    break;

                case false:

                    var newList = new List<string> { cleanResponse };
                    Manager._commandList.Add(cleanCommand, newList);
                    break;
            }

            DbHelper.CommandAdd(cleanCommand, cleanResponse);
        }

        internal static void RemoveCommand(Command command)
        {
            if (command == null) return;
            if (!Manager._commandList.ContainsKey(command.Cmd)) return;

            var responses = Manager._commandList[command.Cmd];
            if (responses.Count <= 1)
            {
                Manager._commandList.Remove(command.Cmd);
            }
            else
            {
                responses.Remove(command.Response);
            }

            DbHelper.CommandDelete(command);
        }

        internal static IEnumerable<Command> GetCommands()
        {
            return DbHelper.CommandsGet();
        }

        internal static string CheckMessage(MessageParts message, string username)
        {
            if (message == null) return string.Empty;
            
            if (message.Count < 2) return string.Empty;
            if (!message[0].Value.Equals("!")) return string.Empty;

            var command = message[1].Value.ToLower();

            if (!Manager._commandList.TryGetValue(command, out var responses)) return string.Empty;

            // Check the number of possible replies

            switch (responses.Count)
            {
                case 0:
                    return string.Empty;
                case 1:
                    return responses[0];
                
                default:
                    
                    // If there is more than one response then pick a random one
                    var randGen = new Random();
                    var randVal = randGen.Next(responses.Count);
                    return responses[randVal];
            }
        }
    }
}
