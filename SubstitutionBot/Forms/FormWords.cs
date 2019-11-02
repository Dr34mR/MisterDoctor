using System;
using System.Windows.Forms;
using SubstitutionBot.Classes;
using SubstitutionBot.Helpers;

namespace SubstitutionBot.Forms
{
    // TODO - Finish Words Implementation

    internal partial class FormWords : Form
    {
        private readonly Word[] _words = DbHelper.WordsGet();

        public FormWords()
        {
            InitializeComponent();
        }

        private void FormWords_Load(object sender, EventArgs e)
        {
            Text = "Substitution Words";

            CancelButton = btnCancel;

            btnCancel.Click += btnCancel_Click;
            gridWords.CellClick += gridWords_CellClick;

            gridWords.AllowUserToAddRows = false;
            gridWords.AllowUserToDeleteRows = false;
            gridWords.AllowUserToOrderColumns = false;
            gridWords.RowHeadersVisible = false;
            gridWords.ColumnHeadersVisible = false;

            SetWords();
        }

        private void gridWords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void SetWords()
        {
            if (_words != null)
            {
                gridWords.DataSource = _words;
                gridWords.Columns[0].Visible = false;
                gridWords.Columns[1].Width = gridWords.Width - 3;
            }
            else
            {
                gridWords.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
