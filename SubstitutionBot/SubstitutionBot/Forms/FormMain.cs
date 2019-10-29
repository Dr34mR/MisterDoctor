using System;
using System.Windows.Forms;
using SubstitutionBot.Classes;
using SubstitutionBot.Helpers;

namespace SubstitutionBot.Forms
{
    internal partial class FormMain : Form
    {
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

        }
    }
}
