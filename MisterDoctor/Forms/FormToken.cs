using System;
using System.Diagnostics;
using System.Windows.Forms;
using MisterDoctor.Classes;

namespace MisterDoctor.Forms
{
    internal partial class FormToken : Form
    {
        public Token Token;
        public bool Save;

        public FormToken()
        {
            InitializeComponent();
        }

        private void FormToken_Load(object sender, EventArgs e)
        {
            Text = "Twitch Token Details";

            if (Token != null)
            {
                txtUsername.Text = Token.Username;
                txtOAuth.Text = Token.UserOAuthKey;
                txtClientId.Text = Token.ClientId;
            }

            CancelButton = btnCancel;

            lnkGetOAuth.LinkClicked += lnkGetOAuth_LinkClicked;
            lnkGetClient.LinkClicked += lnkGetClient_LinkClicked;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            txtUsername.TextChanged += Text_Changed;
            txtOAuth.TextChanged += Text_Changed;
            
            ToggleSave();
        }

        private static void lnkGetClient_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://dev.twitch.tv/console/apps/");
        }

        private static void lnkGetOAuth_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://twitchapps.com/tmi/");
        }

        private void Text_Changed(object sender, EventArgs e)
        {
            ToggleSave();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Sanity Checks

            var cleanName = txtUsername.Text.Trim();
            var cleanOKey = txtOAuth.Text.Trim();

            if (string.IsNullOrEmpty(cleanName))
            {
                MessageBoxEx.Show(this, $"{lblUsername.Text} can not be blank");
                return;
            }

            if (string.IsNullOrEmpty(cleanOKey))
            {
                MessageBoxEx.Show(this, $"{lblOAuth.Text} can not be blank");
                return;
            }

            if (!cleanOKey.StartsWith("oauth:", StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBoxEx.Show(this, $"{lblUsername.Text} must start with 'oauth:'");
                return;
            }
            
            // Now set the return

            Token = new Token
            {
                Username = txtUsername.Text.Trim(),
                UserOAuthKey = txtOAuth.Text.Trim(),
                ClientId = txtClientId.Text.Trim()
            };

            Save = true;
            Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void ToggleSave()
        {
            btnSave.Enabled = !string.IsNullOrEmpty(txtOAuth.Text.Trim()) &&
                              !string.IsNullOrEmpty(txtUsername.Text.Trim());
        }
    }
}
