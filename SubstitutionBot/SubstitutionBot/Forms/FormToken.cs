using System;
using System.Windows.Forms;
using SubstitutionBot.Classes;

namespace SubstitutionBot.Forms
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
                txtAppId.Text = Token.AppClientId;
                txtOAuth.Text = Token.UserOAuthKey;
            }

            CancelButton = btnCancel;

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            txtUsername.TextChanged += Text_Changed;
            txtAppId.TextChanged += Text_Changed;
            txtOAuth.TextChanged += Text_Changed;
            
            ToggleSave();
        }

        private void Text_Changed(object sender, EventArgs e)
        {
            ToggleSave();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Token = new Token
            {
                AppClientId = txtAppId.Text.Trim(),
                Username = txtUsername.Text.Trim(),
                UserOAuthKey = txtOAuth.Text.Trim()
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
            btnSave.Enabled = !string.IsNullOrEmpty(txtAppId.Text.Trim()) &&
                              !string.IsNullOrEmpty(txtOAuth.Text.Trim()) &&
                              !string.IsNullOrEmpty(txtUsername.Text.Trim());
        }
    }
}
