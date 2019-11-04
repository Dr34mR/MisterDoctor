using System;
using System.Windows.Forms;

namespace SubstitutionBot.Forms
{
    public partial class FormInput : Form
    {
        public string Value { get; set; }

        public FormInput()
        {
            InitializeComponent();
        }

        private void FormAddWord_Load(object sender, EventArgs e)
        {
            txtWord.KeyDown += txtWord_KeyDown;

            btnAdd.Click += btnAdd_Click;
            btnCancel.Click += btnCancel_Click;

            CancelButton = btnCancel;

            txtWord.Select();
            txtWord.Focus();
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
            var possibleVal = txtWord.Text.ToLower().Trim();
            if (string.IsNullOrEmpty(possibleVal)) Hide();
            Value = possibleVal;
            Hide();
        }
    }
}
