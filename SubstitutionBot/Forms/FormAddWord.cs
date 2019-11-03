using System;
using System.Windows.Forms;

namespace SubstitutionBot.Forms
{
    public partial class FormAddWord : Form
    {
        public string Word { get; set; }

        public FormAddWord()
        {
            InitializeComponent();
        }

        private void FormAddWord_Load(object sender, EventArgs e)
        {
            Text = "Add Word";

            txtWord.KeyDown += txtWord_KeyDown;

            btnAdd.Click += btnAdd_Click;
            btnCancel.Click += btnCancel_Click;

            CancelButton = btnCancel;

            txtWord.Select();
            txtWord.Focus();
        }

        private void txtWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            AddWord();
        }

        private void AddWord()
        {
            var possibleVal = txtWord.Text.ToLower().Trim();
            if (string.IsNullOrEmpty(possibleVal)) Hide();
            Word = possibleVal;
            Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddWord();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
