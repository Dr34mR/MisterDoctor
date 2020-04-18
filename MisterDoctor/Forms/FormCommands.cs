using System;
using System.Windows.Forms;
using MisterDoctor.Classes;
using MisterDoctor.Managers;

namespace MisterDoctor.Forms
{
    public partial class FormCommands : Form
    {
        public FormCommands()
        {
            InitializeComponent();
        }

        private void FormCommands_Load(object sender, EventArgs e)
        {
            Text = "Commands";

            CancelButton = btnClose;

            CancelButton = btnClose;

            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
            btnClose.Click += btnClose_Click;

            gridWords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gridWords.DataSource = new Command[0];

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

            SetCommands();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var formAdd = new FormCommandsNew();
            formAdd.ShowDialog(this);
            var command = formAdd.Return;
            formAdd.Dispose();

            if (command == null) return;
            CommandManager.AddCommand(command.Cmd, command.Response);

            SetCommands();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!(gridWords.CurrentRow?.DataBoundItem is Command command)) return;

            var result = MessageBoxEx.Show($"Remove '{command.Cmd}'?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            CommandManager.RemoveCommand(command);
            SetCommands();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void SetCommands()
        {
            gridWords.DataSource = CommandManager.GetCommands();
            if (gridWords.Columns.Count > 0 && gridWords.Columns[0].Visible)
                gridWords.Columns[0].Visible = false;

            if (gridWords.Columns.Count > 1) gridWords.Columns[1].Width = 100;
        }
    }
}