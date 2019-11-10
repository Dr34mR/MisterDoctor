using System;
using System.Windows.Forms;
using Substitution.Classes;
using Substitution.Managers;

namespace Substitution.Forms
{
    public partial class FormIgnore : Form
    {
        public FormIgnore()
        {
            InitializeComponent();
        }

        private void FormIgnore_Load(object sender, EventArgs e)
        {
            Text = "Ignore User List";

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

            SetUsers();
        }

        private void SetUsers()
        {
            gridWords.DataSource = IgnoreManager.GetUsers();
            if (gridWords.Columns.Count > 0 && gridWords.Columns[0].Visible)
                gridWords.Columns[0].Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var formAdd = new FormInput { Text = "Add Ignored User" };
            formAdd.ShowDialog(this);
            var returnUser = formAdd.Value?.ToLower();
            formAdd.Dispose();

            if (string.IsNullOrEmpty(returnUser)) return;

            if (returnUser.Contains(" "))
            {
                MessageBoxEx.Show(this, "Username can not contain spaces", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (IgnoreManager.IgnoreUser(returnUser)) return;
            IgnoreManager.AddIgnore(returnUser);

            SetUsers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!(gridWords.CurrentRow?.DataBoundItem is User user)) return;

            var result = MessageBoxEx.Show($"Remove '{user}'?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            IgnoreManager.RemoveIgnore(user.Username);
            SetUsers();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
