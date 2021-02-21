using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MisterDoctor.Forms
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        public void Setup()
        {
            var version = typeof(Program).Assembly.GetName().Version;

            var major = version?.Major ?? 0;
            var minor = version?.Minor ?? 0;
            var build = version?.Build ?? 0;
            var revision = version?.Revision ?? 0;

            Text = $"{major}.{minor}.{build}.{revision}";

            CancelButton = btnClose;

            btnClose.Click += btnClose_Click;

            linkGithub.LinkClicked += linkGithub_LinkClicked;

            FormBorderStyle = FormBorderStyle.FixedDialog;

            MinimizeBox = false;
            MaximizeBox = false;

            StartPosition = FormStartPosition.CenterParent;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static void linkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var processInfo = new ProcessStartInfo("https://github.com/Dr34mR/MisterDoctor")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(processInfo);
        }
    }
}
