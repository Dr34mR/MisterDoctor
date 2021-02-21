using System;
using System.Windows.Forms;

namespace MisterDoctor.Forms
{
    public partial class FormInput : Form
    {
        public string Value { get; private set; }

        public FormInput()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterParent;

            txtInput.KeyDown += txtInput_KeyDown;

            btnAdd.Click += btnAdd_Click;
            btnClose.Click += btnCancel_Click;

            CancelButton = btnClose;

            txtInput.Select();
            txtInput.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Check();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            Check();
        }

        private void Check()
        {
            var possibleVal = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(possibleVal))
            {
                Hide();
                return;
            }
            Value = possibleVal;
            Hide();
        }
    }
}
