using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SubstitutionBot.Classes;
using SubstitutionBot.Helpers;
using SubstitutionBot.Properties;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace SubstitutionBot.Forms
{
    internal partial class FormMain : Form
    {
        private TwitchClient _twitchClient;
        private readonly AppSettings _settings = DbHelper.AppSettingsGet();

        private readonly Random _randGenerator = new Random(Thread.CurrentThread.ManagedThreadId);
        private readonly object _threadLock = new object();
        private DateTime? _coolDownTime;
        
        public FormMain()
        {
            InitializeComponent();
        }

        internal void Setup()
        {
            Icon = Resources.favicon;

            stripLabel1.Spring = true;
            stripLabel1.TextAlign = ContentAlignment.MiddleRight;
            stripLabel1.Font = new Font(stripLabel1.Font, FontStyle.Bold);

            statusStrip.SizingGrip = false;

            Text = "Twitch Substitution Bot";

            tokenToolStripMenuItem.Click += tokenMenu_Click;
            wordListToolStripMenuItem.Click += wordMenu_Click;
            aboutToolStripMenuItem.Click += aboutMenu_Click;

            txtChannel.TextChanged += txtChannel_TextChanged;
            btnConnect.Click += btnConnect_Click;

            txtChannel.Text = _settings.ChannelName;
            chkConnect.Checked = _settings.AutoConnect;
            txtProc.Text = _settings.ProcChance.ToString();
            txtCooldown.Text = _settings.CoolDown.ToString();
            
            UpdateFormStatus();
        }

        private void txtChannel_TextChanged(object sender, EventArgs e)
        {
            UpdateFormStatus();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            MinimumSize = Size;
            MaximumSize = Size;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            txtChannel.Text = txtChannel.Text.Trim();
            if (_twitchClient != null) Disconnect();
            else Connect();
        }

        private void tokenMenu_Click(object sender, EventArgs e)
        {
            ShowToken();
        }
        
        private void wordMenu_Click(object sender, EventArgs e)
        {
            ShowWords();
        }

        private void aboutMenu_Click(object sender, EventArgs e)
        {
            using (var aboutForm = new FormAbout())
            {
                aboutForm.ShowDialog(this);
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
            Disconnect();
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

        // Twitch client things

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

            if (DbHelper.WordRandom() == null)
            {
                ShowError("No Substitution Words Set");
                ShowWords();
            }

            var twitchCredentials = new ConnectionCredentials(token.Username, token.UserOAuthKey);

            _twitchClient = new TwitchClient();
            _twitchClient.OnMessageReceived += Twitch_OnMessageReceived;
            _twitchClient.OnConnected += Twitch_OnConnected;
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

            SaveSettings();
            UpdateFormStatus();
        }

        private void Twitch_OnConnected(object sender, OnConnectedArgs e)
        {
            _twitchClient.SendMessage(txtChannel.Text, "Connected");
        }

        private void Disconnect()
        {
            if (_twitchClient == null) return;

            foreach (var channel in _twitchClient.JoinedChannels)
            {
                _twitchClient.LeaveChannel(channel);
            }

            _twitchClient?.Disconnect();
            _twitchClient = null;

            UpdateFormStatus();
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
                stripLabel1.Text = "Disconnected";
                stripLabel1.ForeColor = Color.DarkRed;

                btnConnect.Text = "Connect";
                txtChannel.Enabled = Enabled;
                btnConnect.Enabled = !string.IsNullOrEmpty(txtChannel.Text.Trim());
            }
            else
            {
                stripLabel1.Text = $"Connected as [{_twitchClient.TwitchUsername}]";
                stripLabel1.ForeColor = Color.DarkGreen;

                btnConnect.Text = "Disconnect";
                txtChannel.Enabled = false;
            }
        }

        private void Twitch_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var message = e.ChatMessage;
            if (message == null) return;
            if (message.IsMe) return;

            lock (_threadLock)
            {
                // Cooldown ?
                if (_coolDownTime.HasValue)
                {
                    if (_coolDownTime > DateTime.Now) return;
                    _coolDownTime = null;
                }

                // Min is Inclisive
                // Max is Exclusive

                var newValue = _randGenerator.Next(1, 101); // 1 to 100
                if (newValue > _settings.ProcChance) return;

                // Sanity Checks on message

                var userMessage = message.Message.Trim();
                if (string.IsNullOrEmpty(userMessage)) return;

                var messageParts = userMessage.Trim().Split(' ');
                _coolDownTime = DateTime.Now.AddSeconds(_settings.CoolDown);
                
                var hasReplaced = false;
                while (!hasReplaced)
                {
                    var replaceIndex = _randGenerator.Next(userMessage.Length);
                    if (string.IsNullOrEmpty(messageParts[replaceIndex])) continue;

                    messageParts[replaceIndex] = DbHelper.WordRandom().Value;
                    hasReplaced = true;
                }

                _twitchClient.SendMessage(message.Channel, string.Join(" ", messageParts));
            }
        }

        private void ShowError(string message)
        {
            MessageBoxEx.Show(this, message.Trim(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
