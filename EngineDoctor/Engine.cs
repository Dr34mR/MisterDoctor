using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using EngineDoctor.Classes;
using EngineDoctor.Extensions;
using EngineDoctor.Helpers;
using EngineDoctor.Managers;
using MisterDoctor.Plugins;
using MisterDoctor.Plugins.Classes;
using MisterDoctor.Plugins.Enums;
using Newtonsoft.Json.Linq;
using RestSharp;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using Constants = EngineDoctor.Helpers.Constants;
using UserType = TwitchLib.Client.Enums.UserType;
using Plugins = MisterDoctor.Plugins;

namespace EngineDoctor;

public static class Engine
{
    private const char CharPass = '\u25CF';

    private static readonly HashSet<string> _ignoredList = new();

    private static readonly object _threadlock = new();
    private static string _channelId = string.Empty;

    private static bool _closing;
    private static TwitchClient _twitchClient;

    public static bool AutoConnect { get; private set; }
    public static string BotClientId { get; private set; }
    public static string BotOAuthKey { get; private set; }
    public static string BotUsername { get; private set; }
    public static string ChannelName { get; private set; }

    public static BindingList<string> Messages { get; } = new();
    public static BindingList<string> ErrorMessages { get; } = new();

    public static List<Plugin> LoadedPlugins { get; } = PluginManager.LoadedPlugins;

    public static bool IsConnected
    {
        get
        {
            var connected = false;
            lock (_threadlock)
            {
                if (_twitchClient != null && _twitchClient.IsConnected)
                {
                    connected = true;
                }
            }

            return connected;
        }
    }
    
    public static void Setup(ConnectionSettings connectionSettings)
    {
        NounManager.Initialize();

        PluginManager.Initialize();

        PluginManager.SendMessageHandler += Plugin_SendTwitchMessage;
        PluginManager.TimeoutHandler += Plugin_TimeoutUser;

        LoadIgnoredList();
        
        UpdateSettings(connectionSettings);
    }

    public static void UpdateSettings(ConnectionSettings connectionSettings)
    {
        AutoConnect = connectionSettings.AutoConnect;
        ChannelName = connectionSettings.ChannelName;
        BotClientId = connectionSettings.BotClientId;
        BotUsername = connectionSettings.BotUsername;
        BotOAuthKey = connectionSettings.BotOAuthKey;
        
        DbHelper.SaveConnectionSettings(connectionSettings);
    }
    
    public static void Connect()
    {
        if (_twitchClient != null && _twitchClient.IsConnected)
        {
            Disconnect();
        }
        
        var channel = ChannelName;
        if (string.IsNullOrEmpty(channel))
        {
            ShowError("Connect to Channel can not be blank");
            return;
        }

        var userName = BotUsername;
        if (string.IsNullOrEmpty(userName))
        {
            ShowError("No Username Set");
            return;
        }

        var oAuth = BotOAuthKey;
        if (string.IsNullOrEmpty(oAuth))
        {
            ShowError("No OAuth Set");
            return;
        }

        if (!oAuth.StartsWith("oauth:", StringComparison.CurrentCultureIgnoreCase))
        {
            ShowError("OAuth Key must start with 'oauth:'");
            return;
        }

        var clientId = BotClientId;
        if (string.IsNullOrEmpty(clientId))
        {
            ShowError("No Client ID");
            return;
        }

        if (PluginManager.LoadedPlugins.Count < 1)
        {
            ShowError("No plugins loaded");
            return;
        }

        var twitchCredentials = new ConnectionCredentials(userName, oAuth);

        lock (_threadlock)
        {
            if (_twitchClient == null)
            {
                _twitchClient = new TwitchClient();
                _twitchClient.OnMessageReceived += Twitch_OnMessageReceived;
            }

            try
            {
                _twitchClient.Initialize(twitchCredentials, channel);
                _twitchClient.Connect();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }


        while (!_twitchClient.JoinedChannels.Any()) Thread.Sleep(200);

        _twitchClient.SendMessage(channel, "Connected");
    }

    public static void Disconnect()
    {
        if (_twitchClient == null) return;

        lock (_threadlock)
        {
            _twitchClient.LeaveChannel(ChannelName);
            _twitchClient.Disconnect();
            _twitchClient = null;
            _channelId = string.Empty;
        }

    }
    
    private static void menuIgnored_Click(object sender, EventArgs e)
    {
        var ignoredSetting = new Setting
        {
            Description = "List of usernames for ignored users",
            Name = "Ignored Users",
            Type = SettingType.StringList
        };

        var currentIgnored = DbHelper.GetIgnoredUsers();

        ignoredSetting.ValueStringList = currentIgnored.Select(i => i.Username).ToList();

        // Setup ignored users form
        // using var ignoredForm = new FormSettingStringList(ignoredSetting)
        // {
        //     StartPosition = FormStartPosition.CenterParent,
        //     Text = @"Ignored Users",
        //     FormBorderStyle = FormBorderStyle.FixedToolWindow,
        //     Size = new Size(315, 165)
        // };


        var returnList = new List<string>();

        returnList = returnList
            .Select(i => i.ToLower().Trim())
            .Where(i => !string.IsNullOrEmpty(i))
            .Distinct()
            .ToList();

        var newIgnored = new List<IgnoredUser>();

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var returnItem in returnList)
            newIgnored.Add(new IgnoredUser
            {
                Username = returnItem
            });

        DbHelper.SaveIgnoredUsers(newIgnored);
    }

    private static void Plugin_SendTwitchMessage(object sender, SendMessageArgs e)
    {
        SendMessage(e?.Reply, e?.OriginalMessage);
    }

    private static void SendMessage(string reply, DigestMessage originalMessage)
    {
        if (_closing) return;
        if (string.IsNullOrEmpty(reply)) return;
        if (_twitchClient == null) return;
        if (!_twitchClient.IsConnected) return;

        // Check for Word Substitution

        if (originalMessage != null)
        {
            var replyParts = new MessageParts(reply);
            replyParts.UpdateWildcards(originalMessage);
            reply = replyParts.ToString();
        }

        lock (_threadlock)
        {
            _twitchClient.SendMessage(ChannelName, reply);
        }
    }

    private static void Plugin_TimeoutUser(object sender, TimeoutArgs e)
    {
        if (_closing) return;
        if (e == null) return;
        if (string.IsNullOrEmpty(e.Username)) return;
        if (_twitchClient == null) return;
        if (!_twitchClient.IsConnected) return;

        _twitchClient.TimeoutUser(ChannelName, e.Username, e.Duration, e.Message);
    }

    private static void linkOAuth_LinkClicked()
    {
        var processInfo = new ProcessStartInfo("https://twitchapps.com/tmi/")
        {
            UseShellExecute = true,
            Verb = "open"
        };
        Process.Start(processInfo);
    }

    private static void linkClientId_LinkClicked()
    {
        var processInfo = new ProcessStartInfo("https://dev.twitch.tv/console/apps/")
        {
            UseShellExecute = true,
            Verb = "open"
        };
        Process.Start(processInfo);
    }

    private static void OnClose()
    {
        _closing = true;
        Disconnect();

        var appSettings = new ConnectionSettings
        {
            AutoConnect = AutoConnect,
            BotClientId = BotClientId,
            BotOAuthKey = BotOAuthKey,
            BotUsername = BotUsername,
            ChannelName = ChannelName,
        };

        DbHelper.SaveConnectionSettings(appSettings);
    }

    private static void LoadIgnoredList()
    {
        var ignoreList = DbHelper.GetIgnoredUsers();

        var lowerList = ignoreList
            .Select(i => i.Username.ToLower().Trim())
            .Where(i => !string.IsNullOrEmpty(i))
            .Distinct();

        foreach (var lowerItem in lowerList) _ignoredList.Add(lowerItem);
    }

  

    private static void Twitch_OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        if (_closing) return;
        var message = e.ChatMessage;
        if (message == null) return;
        if (message.IsMe) return;
        if (message.Username.Equals(_twitchClient.TwitchUsername, StringComparison.CurrentCultureIgnoreCase)) return;

        // Now we have the message :)

        var digestMessage = new DigestMessage
        {
            Message = new MessageParts(message.Message),
            FromAccount = message.DisplayName,
            IsFollower = IsFollowing(message.Username, message.Channel),
            IsSub = message.IsSubscriber,
            UserType = GetUserType(message),
            BotUsername = _twitchClient.TwitchUsername,
            ChannelName = message.Channel,
            IsIgnored = _ignoredList.Contains(message.DisplayName.ToLower())
        };

        // So first check to see if the user should be added / removed from the ignored list

        if (message.Message.StartsWith(Constants.IgnoreCommand, StringComparison.CurrentCultureIgnoreCase))
        {
            var lowerName = message.DisplayName.ToLower();

            if (_ignoredList.Contains(lowerName)) return;

            _ignoredList.Add(lowerName);

            var currentList = DbHelper.GetIgnoredUsers();

            if (currentList.Any(i => i.Username.Equals(lowerName, StringComparison.CurrentCultureIgnoreCase))) return;

            currentList.Add(new IgnoredUser
            {
                Username = lowerName
            });

            DbHelper.SaveIgnoredUsers(currentList);

            SendMessage(Constants.IgnoreResponse, digestMessage);

            return;
        }

        if (message.Message.StartsWith(Constants.UnignoreCommand, StringComparison.CurrentCultureIgnoreCase))
        {
            var lowerName = message.DisplayName.ToLower();

            if (!_ignoredList.Contains(lowerName)) return;

            _ignoredList.Remove(lowerName);

            var currentList = DbHelper.GetIgnoredUsers();

            var matchingUser = currentList.FirstOrDefault(i =>
                i.Username.Equals(lowerName, StringComparison.CurrentCultureIgnoreCase));

            if (matchingUser == null) return;

            currentList.Remove(matchingUser);

            DbHelper.SaveIgnoredUsers(currentList);

            SendMessage(Constants.UnignoreResponse, digestMessage);
            return;
        }

        // If not then we process them as per normal

        foreach (var part in digestMessage.Message.Where(i => i.IsWord)) part.IsNoun = NounManager.IsNoun(part.Value);

        PluginManager.MessageToPlugins(digestMessage);
    }

    private static Plugins.Enums.UserType GetUserType(ChatMessage message)
    {
        switch (message.UserType)
        {
            case UserType.Viewer when message.IsVip:
            {
                return Plugins.Enums.UserType.Vip;
            }
            case UserType.Admin:
            case UserType.Staff:
            case UserType.Moderator:
            case UserType.GlobalModerator:
            {
                return Plugins.Enums.UserType.Mod;
            }
            case UserType.Broadcaster:
            {
                return Plugins.Enums.UserType.Streamer;
            }
            // ReSharper disable once RedundantCaseLabel
            case UserType.Viewer:
            default:
            {
                return Plugins.Enums.UserType.Regular;
            }
        }
    }

    private static bool IsFollowing(string messageUsername, string channelName)
    {
        if (messageUsername.Equals(channelName, StringComparison.CurrentCultureIgnoreCase)) return true;

        // get https://api.twitch.tv/kraken/users/<userId>/follows/channels/<channelId>
        // Accept: application/vnd.twitchtv.v5+json
        // Client-ID: xxxxxxxxxxxx

        var client = new RestClient("https://api.twitch.tv/kraken/");
        var id = BotClientId;

        // Get the channels id number (if hasn't been pulled yet)

        if (string.IsNullOrEmpty(_channelId))
        {
            var channelRequest = new RestRequest($"users?login={channelName}");
            channelRequest.AddHeader("Accept", "application/vnd.twitchtv.v5+json");
            channelRequest.AddHeader("Client-ID", id);

            var channelResponse = client.GetAsync(channelRequest).Result;
            if (channelResponse.Content != null)
            {
                var crObj = JObject.Parse(channelResponse.Content);
                if (!crObj.ContainsKey("_total")) return false;
                if (crObj["_total"]?.ToObject<int>() != 1) return false;
                _channelId = ((JArray) crObj.GetValue("users"))?.First()["_id"]?.ToString() ?? string.Empty;
            }
        }

        if (string.IsNullOrEmpty(_channelId)) return false;

        // Get the users id number

        var userRequest = new RestRequest($"users?login={messageUsername}");
        userRequest.AddHeader("Accept", "application/vnd.twitchtv.v5+json");
        userRequest.AddHeader("Client-ID", id);

        var userResponse = client.GetAsync(userRequest).Result;
        var userId = string.Empty;

        if (userResponse.Content != null)
        {
            var urObj = JObject.Parse(userResponse.Content);
            if (!urObj.ContainsKey("_total")) return false;
            if (urObj["_total"]?.ToObject<int>() != 1) return false;
            userId = ((JArray) urObj.GetValue("users"))?.First()["_id"]?.ToString() ?? string.Empty;
        }

        if (string.IsNullOrEmpty(userId)) return false;

        // Get the follow information

        var followRequest = new RestRequest($"users/{userId}/follows/channels/{_channelId}");
        followRequest.AddHeader("Accept", "application/vnd.twitchtv.v5+json");
        followRequest.AddHeader("Client-ID", id);

        var followResponse = client.GetAsync(followRequest).Result;
        var statusCode = followResponse.StatusCode;

        return statusCode == HttpStatusCode.OK;
    }

    private static void ShowError(Exception ex)
    {
        ShowError(ex.Message);
    }

    private static void ShowError(string message)
    {
        ErrorMessages.Add(message);
    }
}