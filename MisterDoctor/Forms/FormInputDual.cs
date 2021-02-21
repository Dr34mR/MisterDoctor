using System;
using System.Windows.Forms;

namespace MisterDoctor.Forms
{
    public partial class FormInputDual : Form
    {
        public string Key { get; private set; }

        public string Value { get; private set; }

        public FormInputDual()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

            txtKey.KeyDown += txtInput_KeyDown;
            txtValue.KeyDown += txtInput_KeyDown;

            btnAdd.Click += btnAdd_Click;
            btnClose.Click += btnCancel_Click;

            CancelButton = btnClose;

            txtKey.Select();
            txtKey.Focus();
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
            var possibleKey = txtKey.Text.Trim();
            var possibleVal = txtValue.Text.Trim();
            if (string.IsNullOrEmpty(possibleKey) || string.IsNullOrEmpty(possibleVal))
            {
                Hide();
                return;
            }

            Key = possibleKey;
            Value = possibleVal;
            Hide();
        }
    }
}
