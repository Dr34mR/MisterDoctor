using System;
using System.Windows.Forms;
using MisterDoctor.Classes;

namespace MisterDoctor.Forms
{
    internal partial class FormCommandsNew : Form
    {
        internal Command Return { get; set; }

        public FormCommandsNew()
        {
            InitializeComponent();
        }

        private void FormCommandsNew_Load(object sender, EventArgs e)
        {
            txtFind.KeyDown += txtWord_KeyDown;
            txtReply.KeyDown += txtWord_KeyDown;

            btnAdd.Click += btnAdd_Click;
            btnCancel.Click += btnCancel_Click;

            CancelButton = btnCancel;

            txtFind.Select();
            txtFind.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Check();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void txtWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            Check();
        }

        private void Check()
        {
            var cleanCmd = txtFind.Text.ToLower().Trim();
            var cleanResponse = txtReply.Text.Trim();

            if (cleanCmd.Contains(" "))
            {
                MessageBoxEx.Show("Commands can not contain spaces", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(cleanCmd))
            {
                Hide();
                return;
            }

            if (string.IsNullOrEmpty(cleanResponse))
            {
                Hide();
                return;
            }

            Return = new Command
            {
                Cmd = cleanCmd,
                Response = cleanResponse
            };
            Hide();
        }
    }
}
