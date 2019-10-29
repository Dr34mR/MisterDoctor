using System;
using System.Windows.Forms;

namespace SubstitutionBot.Forms
{
    internal partial class FormWords : Form
    {
        public FormWords()
        {
            InitializeComponent();
        }

        private void FormWords_Load(object sender, EventArgs e)
        {
            Text = "Substitution Words";

            CancelButton = btnCancel;

            btnCancel.Click += btnCancel_Click;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
