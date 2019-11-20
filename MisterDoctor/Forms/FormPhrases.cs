using System;
using System.Windows.Forms;
using MisterDoctor.Classes;
using MisterDoctor.Managers;

namespace MisterDoctor.Forms
{
    public partial class FormPhrases : Form
    {
        public FormPhrases()
        {
            InitializeComponent();
        }

        private void FormPhrases_Load(object sender, EventArgs e)
        {
            Text = "Phrases";

            CancelButton = btnClose;

            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
            btnClose.Click += btnClose_Click;

            gridWords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gridWords.DataSource = new Substitution[0];

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

            SetPhrases();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var formAdd = new FormPhrasesNew();
            formAdd.ShowDialog(this);
            var phrase = formAdd.Return;
            formAdd.Dispose();

            if (phrase == null) return;
            PhraseManager.AddPhrase(phrase.Find, phrase.Reply);

            SetPhrases();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!(gridWords.CurrentRow?.DataBoundItem is Substitution phrase)) return;

            var result = MessageBoxEx.Show($"Remove '{phrase.Find}'?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            PhraseManager.RemovePhrase(phrase);
            SetPhrases();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void SetPhrases()
        {
            gridWords.DataSource = PhraseManager.GetPhrases();
            if (gridWords.Columns.Count > 0 && gridWords.Columns[0].Visible)
                gridWords.Columns[0].Visible = false;

            if (gridWords.Columns.Count > 1) gridWords.Columns[1].Width = 100;
        }
    }
}
