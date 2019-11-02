using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SubstitutionBot.Forms
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            Text = $"{typeof(Program).Assembly.GetName().Version.Major}." +
                   $"{typeof(Program).Assembly.GetName().Version.Minor}." +
                   $"{typeof(Program).Assembly.GetName().Version.Build}." +
                   $"{typeof(Program).Assembly.GetName().Version.Revision}";

            CancelButton = btnClose;
            linkGithub.LinkClicked += linkGithub_LinkClicked;
        }

        private void linkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/Dr34mR/TwitchSubstitutionBot");
        }
    }
}
