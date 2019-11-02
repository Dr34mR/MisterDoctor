using System;
using System.Windows.Forms;
using SubstitutionBot.Classes;
using SubstitutionBot.Helpers;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace SubstitutionBot.Forms
{
    internal partial class FormMain : Form
    {
        private TwitchClient _twitchClient;

        private readonly AppSettings _settings = DbHelper.GetAppSettings();

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Text = "Twitch Substitution Bot";
            
            tokenToolStripMenuItem.Click += tokenMenu_Click;
            wordListToolStripMenuItem.Click += wordMenu_Click;

            btnConnect.Click += btnConnect_Click;

            txtChannel.Text = _settings.ChannelName;
            chkConnect.Checked = _settings.AutoConnect;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _settings.ChannelName = txtChannel.Text.Trim();
            _settings.AutoConnect = chkConnect.Checked;
            
            DbHelper.SetAppSettings(_settings);

            //

            if (_twitchClient != null) Disconnect();
            else Connect();
        }

        private void tokenMenu_Click(object sender, EventArgs e)
        {
            FormToken tokenForm = null;
            try
            {
                tokenForm = new FormToken
                {
                    Token = DbHelper.GetToken()
                };
                
                tokenForm.ShowDialog(this);

                if (tokenForm.Save) DbHelper.SetToken(tokenForm.Token);
            }
            finally
            {
                tokenForm?.Dispose();
            }
        }
        
        private void wordMenu_Click(object sender, EventArgs e)
        {
            var wordForm = new FormWords();
            wordForm.ShowDialog(this);
            wordForm.Dispose();
        }
        
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }

        // Twitch client things

        private void Disconnect()
        {
            if (_twitchClient == null) return;

            foreach (var channel in _twitchClient.JoinedChannels)
            {
                _twitchClient.LeaveChannel(channel);
            }

            _twitchClient?.Disconnect();
            _twitchClient = null;

        }

        private void Connect()
        {
            if (_twitchClient != null) return;

            var token = DbHelper.GetToken();

            if (token == null)
            {
                MessageBox.Show(this, "No Token Set");
                return;
            }

            if (string.IsNullOrEmpty(token.Username))
            {
                MessageBox.Show(this, "No Username Set");
            }

            if (string.IsNullOrEmpty(token.UserOAuthKey))
            {
                MessageBox.Show(this, "No OAuth Key Set");
                return;
            }

            if (!token.UserOAuthKey.StartsWith("oauth:", StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show(this, "OAuth Key must start with 'oauth:'");
                return;
            }

            var twitchCredentials = new ConnectionCredentials(token.Username, token.UserOAuthKey);
            
            _twitchClient = new TwitchClient();
            _twitchClient.OnMessageReceived += Twitch_OnMessageReceived;
            _twitchClient.OnConnected += Twitch_OnConnected;
            _twitchClient.Initialize(twitchCredentials, txtChannel.Text.Trim());
            _twitchClient.Connect();
        }
        
        private void Twitch_OnConnected(object sender, OnConnectedArgs e)
        {
            
        }

        private void Twitch_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var message = e.ChatMessage;
            if (message == null) return;
            if (message.IsMe) return;


            _twitchClient.SendMessage(message.Channel, $"Received {message.Message}");


        }
    }
}
