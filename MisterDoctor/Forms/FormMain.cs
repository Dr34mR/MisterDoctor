using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using MisterDoctor.Classes;
using MisterDoctor.Helpers;
using MisterDoctor.Managers;
using MisterDoctor.Properties;
using Newtonsoft.Json.Linq;
using RestSharp;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using Timer = System.Timers.Timer;

namespace MisterDoctor.Forms
{
    internal partial class FormMain : Form
    {
        private TwitchClient _twitchClient;
        private readonly AppSettings _settings = DbHelper.AppSettingsGet();

        private readonly Random _randGenerator = new Random(Thread.CurrentThread.ManagedThreadId);
        private readonly object _threadLock = new object();
        private DateTime? _coolDownTime;

        private readonly Timer _timer = new Timer(1000);
        private bool _procNext;
        private bool _closing;

        private string _channelId = string.Empty;

        public FormMain()
        {
            InitializeComponent();
        }

        internal void Setup()
        {
            Icon = Resources.favicon;

            stripConnected.Spring = true;
            stripConnected.TextAlign = ContentAlignment.MiddleRight;
            stripConnected.Font = new Font(stripConnected.Font, FontStyle.Bold);

            statusStrip.SizingGrip = false;

            Text = "Mister Doctor";

            tokenToolStripMenuItem.Click += tokenMenu_Click;
            wordListToolStripMenuItem.Click += wordMenu_Click;
            phrasesToolStripMenuItem.Click += phraseMenu_Click;
            ignoreToolStripMenuItem.Click += ignoreMenu_Click;
            settingsToolStripMenuItem.Click += settingsMenu_Click;
            aboutToolStripMenuItem.Click += aboutMenu_Click;
            
            btnUpdate.Click += btnUpdate_Click;

            txtChannel.TextChanged += txtChannel_TextChanged;
            btnConnect.Click += btnConnect_Click;

            txtChannel.Text = _settings.ChannelName;
            chkConnect.Checked = _settings.AutoConnect;
            txtProc.Text = _settings.ProcChance.ToString();
            txtCooldown.Text = _settings.CoolDown.ToString();

            NounManager.Initialize();
            IgnoreManager.Initialize();
            PhraseManager.Initialize();

            RussianManager.SendMessage += Russian_SendMessage;
            RussianManager.SendTimeout += Russian_SendTimeout;

            if (UpdateHelper.UpdateAvailable()) Text += "  [UPDATE AVAILABLE]";

            UpdateFormStatus();

            _timer.AutoReset = true;
            _timer.Elapsed += timer_Elapsed;
            _timer.Start();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            MinimumSize = Size;
            MaximumSize = Size;

            if (chkConnect.Checked) btnConnect.PerformClick();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _closing = true;
            Enabled = false;

            _timer.Stop();
            _timer.Dispose();

            if (int.TryParse(txtProc.Text, out var intProc)) _settings.ProcChance = intProc;
            if (int.TryParse(txtCooldown.Text, out var intCoolDown)) _settings.CoolDown = intCoolDown;

            SaveSettings();
            Disconnect();
        }

        private void tokenMenu_Click(object sender, EventArgs e)
        {
            ShowToken();
        }

        private void wordMenu_Click(object sender, EventArgs e)
        {
            ShowWords();
        }

        private void phraseMenu_Click(object sender, EventArgs e)
        {
            var phrasesForm = new FormPhrases();
            phrasesForm.ShowDialog(this);
            phrasesForm.Dispose();
        }

        private void ignoreMenu_Click(object sender, EventArgs e)
        {
            var ignoreForm = new FormIgnore();
            ignoreForm.ShowDialog(this);
            ignoreForm.Dispose();
        }

        private void aboutMenu_Click(object sender, EventArgs e)
        {
            using (var aboutForm = new FormAbout())
            {
                aboutForm.ShowDialog(this);
            }
        }

        private void settingsMenu_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new FormSettings())
            {
                settingsForm.Settings = _settings;
                settingsForm.ShowDialog(this);
                if (!settingsForm.Cancelled) DbHelper.AppSettingsSet(_settings);
            }
        }

        private void ShowToken()
        {
            FormToken tokenForm = null;
            try
            {
                tokenForm = new FormToken
                {
                    Token = DbHelper.TokenGet()
                };

                tokenForm.ShowDialog(this);

                if (tokenForm.Save) DbHelper.TokenSet(tokenForm.Token);
            }
            finally
            {
                tokenForm?.Dispose();
            }
        }

        private void ShowWords()
        {
            var wordForm = new FormWords();
            wordForm.ShowDialog(this);
            wordForm.Dispose();
        }

        private void txtChannel_TextChanged(object sender, EventArgs e)
        {
            UpdateFormStatus();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            txtChannel.Text = txtChannel.Text.Trim();
            if (_twitchClient != null) Disconnect();
            else Connect();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtProc.Text, out var intProc))
            {
                ShowError("Proc Chance must be a number");
                return;
            }

            if (!int.TryParse(txtCooldown.Text, out var intCoolDown))
            {
                ShowError("Cooldown must be a number");
                return;
            }

            _settings.CoolDown = intCoolDown;
            _settings.ProcChance = intProc;

            SaveSettings();

            MessageBoxEx.Show(this, "Settings Updated", Application.ProductName, MessageBoxButtons.OK);
        }

        private void SaveSettings()
        {
            _settings.ChannelName = txtChannel.Text.Trim();
            _settings.AutoConnect = chkConnect.Checked;
            DbHelper.AppSettingsSet(_settings);
        }

        private void UpdateFormStatus()
        {
            if (_twitchClient == null)
            {
                stripConnected.Text = "Disconnected";
                stripConnected.ForeColor = Color.DarkRed;

                btnConnect.Text = "Connect";
                txtChannel.Enabled = Enabled;
                btnConnect.Enabled = !string.IsNullOrEmpty(txtChannel.Text.Trim());
            }
            else
            {
                stripConnected.Text = $"Connected as [{_twitchClient.TwitchUsername}]";
                stripConnected.ForeColor = Color.DarkGreen;

                btnConnect.Text = "Disconnect";
                txtChannel.Enabled = false;
            }

            btnUpdate.Enabled = !txtChannel.Enabled;
        }

        private void ShowError(string message)
        {
            MessageBoxEx.Show(this, message.Trim(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var textToSet = "No Cooldown";
            lock (_threadLock)
            {
                if (_procNext)
                {
                    textToSet = "Triggered - Waiting for next match";
                }
                else if (_coolDownTime != null)
                {
                    if (DateTime.Now > _coolDownTime) _coolDownTime = null;
                    else
                    {
                        var diff = _coolDownTime.Value - DateTime.Now;
                        textToSet = $"Cooldown {Math.Floor(diff.TotalSeconds)} second/s";
                    }
                }
            }
            stripCooldown.Text = textToSet;
        }

        // Twitch Things

        private void Connect()
        {
            if (_twitchClient != null) return;

            var token = DbHelper.TokenGet();

            if (token == null)
            {
                ShowError("No Token Set");
                ShowToken();
                return;
            }

            if (string.IsNullOrEmpty(token.Username))
            {
                ShowError("No Token Username Set");
                ShowToken();
                return;
            }

            if (string.IsNullOrEmpty(token.UserOAuthKey))
            {
                ShowError("No Token OAuth Key Set");
                ShowToken();
                return;
            }

            if (!token.UserOAuthKey.StartsWith("oauth:", StringComparison.CurrentCultureIgnoreCase))
            {
                ShowError("OAuth Key must start with 'oauth:'");
                ShowToken();
                return;
            }

            if (string.IsNullOrEmpty(token.ClientId))
            {
                ShowError("No Client ID");
                ShowToken();
                return;
            }

            if (DbHelper.WordRandom() == null)
            {
                ShowError("No Substitution Words Set");
                ShowWords();
                return;
            }

            if (!int.TryParse(txtProc.Text, out var intProc))
            {
                ShowError("Proc Chance must be a number");
                return;
            }

            if (!int.TryParse(txtCooldown.Text, out var intCoolDown))
            {
                ShowError("Cooldown must be a number");
                return;
            }

            _settings.CoolDown = intCoolDown;
            _settings.ProcChance = intProc;

            var twitchCredentials = new ConnectionCredentials(token.Username, token.UserOAuthKey);

            lock (_threadLock)
            {
                _twitchClient = new TwitchClient();
                _twitchClient.OnMessageReceived += Twitch_OnMessageReceived;
                try
                {
                    _twitchClient.Initialize(twitchCredentials, txtChannel.Text.Trim());
                    _twitchClient.Connect();
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                    return;
                }
            }

            SaveSettings();
            UpdateFormStatus();
        }

        private void Disconnect()
        {
            if (_twitchClient == null) return;

            lock (_threadLock)
            {
                _twitchClient.LeaveChannel(txtChannel.Text);
                _twitchClient?.Disconnect();
                _twitchClient = null;
            }

            UpdateFormStatus();
        }

        private void Russian_SendMessage(object sender, RussianMessageArgs e)
        {
            lock (_threadLock)
            {
                if (_twitchClient == null) return;
                if (!_twitchClient.IsConnected) return;
                
                _twitchClient.SendMessage(txtChannel.Text, e.Message);
            }
        }

        private void Russian_SendTimeout(object sender, RussianTimeoutArgs e)
        {
            lock (_threadLock)
            {
                if (_twitchClient == null) return;
                if (!_twitchClient.IsConnected) return;

                _twitchClient.TimeoutUser(txtChannel.Text, e.Username, e.Duration, e.Message);
            }
        }

        private void Twitch_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (_closing) return;
            var message = e.ChatMessage;
            if (message == null) return;
            if (message.IsMe) return;
            if (message.Username.Equals(_twitchClient.TwitchUsername, StringComparison.CurrentCultureIgnoreCase)) return;

            // Russian Roulette First

            RussianManager.DigestMessage(message);

            // Ignore List

            if (message.Message.StartsWith(_settings.CommandIgnore, StringComparison.CurrentCultureIgnoreCase))
            {
                if (IgnoreManager.IgnoreUser(message.Username)) return;
                IgnoreManager.AddIgnore(message.Username);

                lock (_threadLock)
                {
                    _twitchClient.SendMessage(message.Channel, $"Ok @{message.Username} :(");
                }
                return;
            }

            if (message.Message.StartsWith(_settings.CommandUnignore, StringComparison.CurrentCultureIgnoreCase))
            {
                if (!IgnoreManager.IgnoreUser(message.Username)) return;
                IgnoreManager.RemoveIgnore(message.Username);
                lock (_threadLock)
                {
                    _twitchClient.SendMessage(message.Channel, $"{DbHelper.WordRandom()}! @{message.Username} PogChamp");
                }
                return;
            }

            if (IgnoreManager.IgnoreUser(message.Username)) return;

            // Other Message Digests

            var userMessage = message.Message.Trim();
            if (string.IsNullOrEmpty(userMessage)) return;

            var parts = new MessageParts(userMessage);

            var wordCount = parts.WordCount();
            if (wordCount <= 1) return;
            if (wordCount > _settings.MaxMessageSize) return;

            // Now do the lock and checking

            lock (_threadLock)
            {
                // First check for a word in the message
                
                var send = GoodBadCheck(parts, _twitchClient.TwitchUsername);

                if (string.IsNullOrEmpty(send)) send = PhraseCheck(parts, message.Username);
                if (string.IsNullOrEmpty(send)) send = RandomReplace(message.Username, message.Channel, parts);
                if (string.IsNullOrEmpty(send)) return;
                
                var sendParts = new MessageParts(send);
                sendParts.UpdateWildcards(message);

                _twitchClient.SendMessage(message.Channel, sendParts.ToString());
            }
        }

        private string GoodBadCheck(MessageParts parts, string botName)
        {
            var happy = new[] {"yes", "goodbot", "plz", "nice"};
            var sad = new[] {"no", "badbot", "why", "bad"};

            var wordsOnly = parts.Where(i => i.IsWord).Select(i => i.Value.ToLower()).ToList();
            if (wordsOnly.Count < 2) return string.Empty;

            // Bot Check

            if (!wordsOnly.Any(i => i.Equals(botName.ToLower()))) return string.Empty;

            // Now check for happy or sad

            if (wordsOnly.Intersect(happy).Any()) return _settings.GoodBot;

            return wordsOnly.Intersect(sad).Any() ? _settings.BadBot : string.Empty;
        }

        private static string PhraseCheck(MessageParts parts, string username)
        {
            return PhraseManager.CheckMessage(parts, username);
        }

        private string RandomReplace(string user, string channel, MessageParts parts)
        {
            // Cooldown ?
            if (_coolDownTime.HasValue)
            {
                if (_coolDownTime > DateTime.Now) return string.Empty;
                _coolDownTime = null;
            }

            if (!_procNext)
            {
                // Min is Inclisive / Max is Exclusive
                var newValue = _randGenerator.Next(1, 101); // 1 to 100
                if (newValue > _settings.ProcChance) return string.Empty;
            }

            // If we passed the 'Random Check' then make sure it triggers next time

            _procNext = true;

            // First check if the user is following (check after proc)

            if (!IsFollowing(user, channel)) return string.Empty;

            // Get the indexes of the nouns

            if (!parts.HasNoun()) return string.Empty;
            var nounIndexes = parts.NounIndexes();

            // Randomly pick a noun index

            var originalMessage = parts.ToString();

            var replaceIndex = _randGenerator.Next(nounIndexes.Count);
            parts.ReplaceWord(nounIndexes[replaceIndex], DbHelper.WordRandom().Value);

            var messageToSend = parts.ToString();
            if (originalMessage.Equals(messageToSend, StringComparison.CurrentCultureIgnoreCase)) return string.Empty;
            
            _procNext = false;
            _coolDownTime = DateTime.Now.AddSeconds(_settings.CoolDown);

            return messageToSend;
        }

        private bool IsFollowing(string messageUsername, string channelName)
        {
            if (messageUsername.Equals(channelName, StringComparison.CurrentCultureIgnoreCase)) return true;

            // get https://api.twitch.tv/kraken/users/<userId>/follows/channels/<channelId>
            // Accept: application/vnd.twitchtv.v5+json
            // Client-ID: xxxxxxxxxxxx

            var client = new RestClient("https://api.twitch.tv/kraken/");
            var id = DbHelper.TokenGet().ClientId;

            // Get the channels id number (if hasn't been pulled yet)

            if (string.IsNullOrEmpty(_channelId))
            {
                var channelRequest = new RestRequest($"users?login={channelName}");
                channelRequest.AddHeader("Accept", "application/vnd.twitchtv.v5+json");
                channelRequest.AddHeader("Client-ID", id);

                var channelResponse = client.Get(channelRequest);

                var crObj = JObject.Parse(channelResponse.Content);
                if (!crObj.ContainsKey("_total")) return false;
                if (crObj["_total"].ToObject<int>() != 1) return false;

                _channelId = ((JArray)crObj.GetValue("users")).First()["_id"].ToString();
            }

            // Get the users id number

            var userRequest = new RestRequest($"users?login={messageUsername}");
            userRequest.AddHeader("Accept", "application/vnd.twitchtv.v5+json");
            userRequest.AddHeader("Client-ID", id);

            var userResponse = client.Get(userRequest);

            var urObj = JObject.Parse(userResponse.Content);
            if (!urObj.ContainsKey("_total")) return false;
            if (urObj["_total"].ToObject<int>() != 1) return false;
            var userId = ((JArray)urObj.GetValue("users")).First()["_id"].ToString();

            // Get the follow information

            var followRequest = new RestRequest($"users/{userId}/follows/channels/{_channelId}");
            followRequest.AddHeader("Accept", "application/vnd.twitchtv.v5+json");
            followRequest.AddHeader("Client-ID", id);

            var followResponse = client.Get(followRequest);

            var statusCode = followResponse.StatusCode;

            return statusCode == HttpStatusCode.OK;

        }
    }
}
