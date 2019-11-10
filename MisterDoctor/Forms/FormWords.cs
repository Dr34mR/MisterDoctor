using System;
using System.Windows.Forms;
using MisterDoctor.Classes;
using MisterDoctor.Helpers;

namespace MisterDoctor.Forms
{
    internal partial class FormWords : Form
    {
        public FormWords()
        {
            InitializeComponent();
        }

        private void FormWords_Load(object sender, EventArgs e)
        {
            Text = "Substitute List";

            CancelButton = btnClose;

            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
            btnClose.Click += btnClose_Click;

            gridWords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gridWords.DataSource = new Word[0];

            gridWords.AllowUserToAddRows = false;
            gridWords.AllowUserToDeleteRows = false;
            gridWords.AllowUserToOrderColumns = false;
            gridWords.RowHeadersVisible = false;
            gridWords.ColumnHeadersVisible = false;

            gridWords.AllowUserToResizeRows = false;
            gridWords.AllowUserToResizeColumns = false;
            gridWords.MultiSelect = false;

            gridWords.BackColor = BackColor;
            gridWords.BackgroundColor = BackColor;

            gridWords.BorderStyle = BorderStyle.None;

            gridWords.Columns[0].Visible = false;

            SetWords();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var formAdd = new FormInput { Text = "Add Word" };
            formAdd.ShowDialog(this);
            var returnWord = formAdd.Value;
            formAdd.Dispose();

            if (string.IsNullOrEmpty(returnWord)) return;
            DbHelper.WordAdd(returnWord);

            SetWords();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!(gridWords.CurrentRow?.DataBoundItem is Word word)) return;

            var result = MessageBoxEx.Show($"Remove '{word}'?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            DbHelper.WordDelete(word);
            SetWords();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void SetWords()
        {
            gridWords.DataSource = DbHelper.WordsGet();
            if (gridWords.Columns.Count > 0 && gridWords.Columns[0].Visible) 
                gridWords.Columns[0].Visible = false;
        }
    }
}