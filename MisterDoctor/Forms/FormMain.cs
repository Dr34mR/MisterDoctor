using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using MisterDoctor.Classes;
using MisterDoctor.Extensions;
using MisterDoctor.Helpers;
using MisterDoctor.Managers;
using MisterDoctor.Plugins;
using MisterDoctor.Plugins.Classes;
using MisterDoctor.Plugins.Enums;
using MisterDoctor.Properties;
using Newtonsoft.Json.Linq;
using RestSharp;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using UserType = TwitchLib.Client.Enums.UserType;

namespace MisterDoctor.Forms
{
    public partial class FormMain : Form
    {
        private TwitchClient _twitchClient;
        
        private readonly object _threadlock = new();
        
        private bool _closing;
        private string _channelId = string.Empty;

        private const char CharPass = '\u25CF';

        private readonly HashSet<string> _ignoredList = new();

        public FormMain()
        {
            InitializeComponent();
        }

        public void Setup()
        {
            MaximizeBox = false;

            StartPosition = FormStartPosition.CenterScreen;

            statusStrip1.SizingGrip = false;

            Icon = Resources.favicon;
            Text = Application.ProductName;

            var appSettings = DbHelper.ReadConnectionSettings();

            txtChannel.Text = appSettings.ChannelName.Trim();
            txtClient.Text = appSettings.BotClientId.Trim();
            txtOAuth.Text = appSettings.BotOAuthKey.Trim();
            txtUsername.Text = appSettings.BotUsername.Trim();

            txtOAuth.PasswordChar = CharPass;
            txtClient.PasswordChar = CharPass;

            statusStripConnected.TextAlign = ContentAlignment.MiddleRight;
            statusStripConnected.Font = new Font(statusStripConnected.Font, FontStyle.Bold);

            btnConnect.Click += btnConnect_Click;

            linkOAuth.LinkClicked += linkOAuth_LinkClicked;
            linkClientId.LinkClicked += linkClientId_LinkClicked;

            chkAutoConnect.Checked = appSettings.AutoConnect;

            FormClosing += Form_FormClosing;

            menuPlugins.Click += menuPlugins_Click;
            menuIgnored.Click += menuIgnored_Click;
            menuAbout.Click += menuAbout_Click;

            NounManager.Initialize();

            PluginManager.Initialize();

            PluginManager.SendMessageHandler += Plugin_SendTwitchMessage;
            PluginManager.TimeoutHandler += Plugin_TimeoutUser;

            LoadIgnoredList();

            UpdateFormStatus();
        }

        private void menuIgnored_Click(object sender, EventArgs e)
        {
            var ignoredSetting = new Setting
            {
                Description = "List of usernames for ignored users",
                Name = "Ignored Users",
                Type = SettingType.StringList
            };

            var currentIgnored = DbHelper.GetIgnoredUsers();

            ignoredSetting.ValueStringList = currentIgnored.Select(i => i.Username).ToList();

            using var ignoredForm = new FormSettingStringList(ignoredSetting)
            {
                StartPosition = FormStartPosition.CenterParent, 
                Text = @"Ignored Users",
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                Size = new Size(315,165)
            };

            ignoredForm.MaximumSize = ignoredForm.Size;
            ignoredForm.MinimumSize = ignoredForm.Size;

            ignoredForm.ShowDialog(this);

            var returnList = ignoredForm.ReturnValue;

            returnList = returnList
                .Select(i => i.ToLower().Trim())
                .Where(i => !string.IsNullOrEmpty(i))
                .Distinct()
                .ToList();

            var newIgnored = new List<IgnoredUser>();

            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach(var returnItem in returnList)
            {
                newIgnored.Add(new IgnoredUser
                {
                    Username = returnItem
                });
            }

            DbHelper.SaveIgnoredUsers(newIgnored);
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            Enabled = false;

            var aboutForm = new FormAbout();
            aboutForm.Setup();
            aboutForm.ShowDialog(this);
            aboutForm.Dispose();

            Enabled = true;
        }

        private void menuPlugins_Click(object sender, EventArgs e)
        {
            Enabled = false;

            var pluginForm = new FormPlugins();
            pluginForm.Setup();
            pluginForm.ShowDialog(this);
            pluginForm.Dispose();

            Enabled = true;
        }

        private void Plugin_SendTwitchMessage(object sender, SendMessageArgs e)
        {
            SendMessage(e?.Reply, e?.OriginalMessage);
        }
        
        private void SendMessage(string reply, DigestMessage originalMessage)
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
                _twitchClient.SendMessage(txtChannel.Text.Trim(), reply);
            }
        }

        private void Plugin_TimeoutUser(object sender, TimeoutArgs e)
        {
            if (_closing) return;
            if (e == null) return;
            if (string.IsNullOrEmpty(e.Username)) return;
            if (_twitchClient == null) return;
            if (!_twitchClient.IsConnected) return;

            _twitchClient.TimeoutUser(txtChannel.Text, e.Username, e.Duration, e.Message);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Enabled = false;
            if (_twitchClient != null) Disconnect();
            else Connect();
            Enabled = true;
        }

        private static void linkOAuth_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var processInfo = new ProcessStartInfo("https://twitchapps.com/tmi/")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(processInfo);
        }

        private static void linkClientId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var processInfo = new ProcessStartInfo("https://dev.twitch.tv/console/apps/")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(processInfo);
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            _closing = true;
            Enabled = false;

            Disconnect();

            var appSettings = new ConnectionSettings
            {
                AutoConnect = chkAutoConnect.Checked,
                BotClientId = txtClient.Text.Trim(),
                BotOAuthKey = txtOAuth.Text.Trim(),
                BotUsername = txtUsername.Text.Trim(),
                ChannelName = txtChannel.Text.Trim()
            };

            DbHelper.SaveConnectionSettings(appSettings);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            MinimumSize = Size;
            MaximumSize = Size;

            if (chkAutoConnect.Checked)
            {
                btnConnect.PerformClick();
            }

            linkClientId.Select();
        }

        private void LoadIgnoredList()
        {
            var ignoreList = DbHelper.GetIgnoredUsers();

            var lowerList = ignoreList
                .Select(i => i.Username.ToLower().Trim())
                .Where(i => !string.IsNullOrEmpty(i))
                .Distinct();

            foreach(var lowerItem in lowerList)
            {
                _ignoredList.Add(lowerItem);
            }
        }

        private void UpdateFormStatus()
        {
            if (_twitchClient == null)
            {
                statusStripConnected.Text = @"Disconnected";
                statusStripConnected.ForeColor = Color.DarkRed;

                btnConnect.Text = @"Connect";
                txtChannel.Enabled = true;
                txtClient.Enabled = true;
                txtOAuth.Enabled = true;
                txtUsername.Enabled = true;
            }
            else
            {
                statusStripConnected.Text = $@"Connected as [{_twitchClient.TwitchUsername}]";
                statusStripConnected.ForeColor = Color.DarkGreen;

                btnConnect.Text = @"Disconnect";
                txtChannel.Enabled = false;
                txtClient.Enabled = false;
                txtOAuth.Enabled = false;
                txtUsername.Enabled = false;
            }
        }

        private void Connect()
        {
            var channel = txtChannel.Text.Trim();
            if (string.IsNullOrEmpty(channel))
            {
                ShowError("Connect to Channel can not be blank");
                return;
            }

            var userName = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(userName))
            {
                ShowError("No Username Set");
                return;
            }

            var oAuth = txtOAuth.Text.Trim();
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

            var clientId = txtClient.Text.Trim();
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

            Enabled = false;

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

            UpdateFormStatus();

            while (!_twitchClient.JoinedChannels.Any())
            {
                System.Threading.Thread.Sleep(200);
            }

            _twitchClient.SendMessage(channel, "Connected");

            Enabled = true;
        }

        private void Disconnect()
        {
            if (_twitchClient == null) return;

            lock (_threadlock)
            {
                _twitchClient.LeaveChannel(txtChannel.Text.Trim());
                _twitchClient.Disconnect();
                _twitchClient = null;
                _channelId = string.Empty;
            }

            UpdateFormStatus();
        }

        private void Twitch_OnMessageReceived(object sender, OnMessageReceivedArgs e)
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

                if (currentList.Any(i => i.Username.Equals(lowerName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    return;
                }

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

                var matchingUser = currentList.FirstOrDefault(i => i.Username.Equals(lowerName, StringComparison.CurrentCultureIgnoreCase));

                if (matchingUser == null)
                {
                    return;
                }

                currentList.Remove(matchingUser);

                DbHelper.SaveIgnoredUsers(currentList);

                SendMessage(Constants.UnignoreResponse, digestMessage);
                return;
            }

            // If not then we process them as per normal

            foreach (var part in digestMessage.Message.Where(i => i.IsWord))
            {
                part.IsNoun = NounManager.IsNoun(part.Value);
            }

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

        private bool IsFollowing(string messageUsername, string channelName)
        {
            if (messageUsername.Equals(channelName, StringComparison.CurrentCultureIgnoreCase)) return true;

            // get https://api.twitch.tv/kraken/users/<userId>/follows/channels/<channelId>
            // Accept: application/vnd.twitchtv.v5+json
            // Client-ID: xxxxxxxxxxxx

            var client = new RestClient("https://api.twitch.tv/kraken/");
            var id = txtClient.Text.Trim();

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
                    _channelId = ((JArray)crObj.GetValue("users"))?.First()["_id"]?.ToString() ?? string.Empty;
                }
            }

            if (string.IsNullOrEmpty(_channelId))
            {
                return false;
            }

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
                userId = ((JArray)urObj.GetValue("users"))?.First()["_id"]?.ToString() ?? string.Empty;
            }

            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            // Get the follow information

            var followRequest = new RestRequest($"users/{userId}/follows/channels/{_channelId}");
            followRequest.AddHeader("Accept", "application/vnd.twitchtv.v5+json");
            followRequest.AddHeader("Client-ID", id);

            var followResponse = client.GetAsync(followRequest).Result;
            var statusCode = followResponse.StatusCode;

            return statusCode == HttpStatusCode.OK;
        }

        private void ShowError(Exception ex)
        {
            ShowError(ex.Message);
        }

        private void ShowError(string message)
        {
            MessageBoxEx.Show(this, message.Trim(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
