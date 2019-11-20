using System;
using System.Windows.Forms;
using MisterDoctor.Classes;

namespace MisterDoctor.Forms
{
    internal partial class FormPhrasesNew : Form
    {
        internal Substitution Return { get; set; }

        public FormPhrasesNew()
        {
            InitializeComponent();
        }

        private void FormPhrasesNew_Load(object sender, EventArgs e)
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
            var cleanFind = txtFind.Text.ToLower().Trim();
            var cleanReply = txtReply.Text.Trim();

            if (cleanFind.Contains(" "))
            {
                MessageBox.Show("Can only look for single words", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(cleanFind))
            {
                Hide();
                return;
            }

            if (string.IsNullOrEmpty(cleanReply))
            {
                Hide();
                return;
            }

            Return = new Substitution
            {
                Find = cleanFind,
                Reply = cleanReply
            };
            Hide();
        }
    }
}
