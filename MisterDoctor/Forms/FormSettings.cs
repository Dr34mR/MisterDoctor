using System;
using System.Windows.Forms;
using MisterDoctor.Classes;

namespace MisterDoctor.Forms
{
    public partial class FormSettings : Form
    {
        internal AppSettings Settings;
        internal bool Cancelled;

        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            txtMaxWords.Text = Settings.MaxMessageSize.ToString();
            txtIgnore.Text = Settings.CommandIgnore;
            txtUnignore.Text = Settings.CommandUnignore;
            txtGoodbot.Text = Settings.GoodBot;
            txtBadbot.Text = Settings.BadBot;

            txtMaxWords.KeyDown += txtWord_KeyDown;
            txtIgnore.KeyDown += txtWord_KeyDown;

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            CancelButton = btnCancel;

            txtMaxWords.Select();
            txtMaxWords.Focus();

            Cancelled = true;
        }

        private void txtWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            Save();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void Save()
        {
            var cleanMaxWords = txtMaxWords.Text.Trim();
            var cleanIgnore = txtIgnore.Text.ToLower().Trim();
            var cleanUnignore = txtUnignore.Text.ToLower().Trim();
            var cleanGoodbot = txtGoodbot.Text.Trim();
            var cleanBadbot = txtBadbot.Text.Trim();

            if (!int.TryParse(cleanMaxWords, out var intMax))
            {
                ShowError("Max words in message must be a number");
                return;
            }

            if (intMax < 5)
            {
                ShowError("Max words in message must be at least 5");
            }

            if (string.IsNullOrEmpty(cleanIgnore))
            {
                ShowError("Ignore Command must not be blank");
                return;
            }

            if (string.IsNullOrEmpty(cleanUnignore))
            {
                ShowError("Unignore Command must not be blank");
                return;
            }

            if (string.IsNullOrEmpty(cleanGoodbot))
            {
                ShowError("Goodbot Response must not be blank");
                return;
            }

            if (string.IsNullOrEmpty(cleanBadbot))
            {
                ShowError("Badbot Response must not be blank");
                return;
            }

            Settings.MaxMessageSize = intMax;
            Settings.CommandIgnore = cleanIgnore;
            Settings.CommandUnignore = cleanUnignore;
            Settings.GoodBot = cleanGoodbot;
            Settings.BadBot = cleanBadbot;

            Cancelled = false;
            Hide();
        }

        private void ShowError(string message)
        {
            MessageBoxEx.Show(this, message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
