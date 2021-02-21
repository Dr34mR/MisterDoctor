using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MisterDoctor.Classes;
using MisterDoctor.Helpers;
using MisterDoctor.Managers;
using MisterDoctor.Plugins;
using MisterDoctor.Properties;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable LocalizableElement

namespace MisterDoctor.Forms
{
    public partial class FormPlugins : Form
    {
        public FormPlugins()
        {
            InitializeComponent();
        }

        public void Setup()
        {
            Icon = Resources.favicon;
            Text = "Plugins";

            StartPosition = FormStartPosition.CenterParent;

            MaximumSize = Size;

            MaximizeBox = false;
            MinimizeBox = false;

            FormBorderStyle = FormBorderStyle.FixedDialog;

            var idColumn = new DataGridViewTextBoxColumn
            {
                Name = nameof(Plugin.UniqueId),
                HeaderText = nameof(Plugin.UniqueId)
            };

            var seqColumn = new DataGridViewTextBoxColumn
            {
                Name = "Sequence",
                HeaderText = "Sequence"
            };

            var nameColumn = new DataGridViewTextBoxColumn
            {
                Name = nameof(Plugin.Name),
                HeaderText = nameof(Plugin.Name)
            };

            var settingsColumn = new DataGridViewButtonColumn
            {
                Name = "Settings",
                HeaderText = "Settings",
                UseColumnTextForButtonValue = true,
                Text = "..."
            };

            var enableColumn = new DataGridViewButtonColumn
            {
                Name = nameof(PluginState.Enabled),
                HeaderText = nameof(PluginState.Enabled)
            };

            mainGrid.Columns.Add(idColumn);
            mainGrid.Columns.Add(seqColumn);
            mainGrid.Columns.Add(nameColumn);
            mainGrid.Columns.Add(settingsColumn);
            mainGrid.Columns.Add(enableColumn);

            mainGrid.Columns[nameof(Plugin.UniqueId)].Visible = false;

            mainGrid.Columns["Sequence"].MinimumWidth = 25;
            mainGrid.Columns["Sequence"].Width = 25;

            mainGrid.Columns[nameof(Plugin.Name)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            mainGrid.Columns["Settings"].MinimumWidth = 25;
            mainGrid.Columns["Settings"].Width = 25;

            mainGrid.Columns["Enabled"].MinimumWidth = 60;
            mainGrid.Columns["Enabled"].Width = 60;
            
            mainGrid.ReadOnly = true;

            mainGrid.RowHeadersVisible = false;
            mainGrid.ColumnHeadersVisible = false;

            mainGrid.AllowUserToAddRows = false;
            mainGrid.AllowUserToDeleteRows = false;
            mainGrid.AllowUserToOrderColumns = false;
            mainGrid.AllowUserToResizeColumns = false;
            mainGrid.AllowUserToResizeRows = false;

            mainGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            mainGrid.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            mainGrid.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            mainGrid.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None; 
            mainGrid.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            
            mainGrid.SelectionChanged += DataGrid_SelectionChanged;

            mainGrid.BackgroundColor = Color.White;
            mainGrid.BorderStyle = BorderStyle.Fixed3D;
            
            mainGrid.CellClick += mainGrid_CellClick;

            btnUp.Enabled = false;
            btnDown.Enabled = false;

            PopulateRows();

            btnUp.Click += btnUp_Click;
            btnDown.Click += btnDown_Click;
            btnClose.Click += btnClose_Click;

            CancelButton = btnClose;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            var row = mainGrid.CurrentRow;
            if (row == null) return;

            var guid = (Guid) row.Cells[nameof(Plugin.UniqueId)].Value;
            var plugin = PluginManager.LoadedPlugins.First(i => i.UniqueId == guid);

            PluginManager.MovePluginUp(plugin);

            PopulateRows();

            // SelectRow(guid);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            var row = mainGrid.CurrentRow;
            if (row == null) return;

            var guid = (Guid) row.Cells[nameof(Plugin.UniqueId)].Value;
            var plugin = PluginManager.LoadedPlugins.First(i => i.UniqueId == guid);

            PluginManager.MovePluginDown(plugin);

            PopulateRows();

            // SelectRow(guid);
        }

        //private void SelectRow(Guid guid)
        //{ 
        //    if (guid == Guid.Empty) return;

        //    foreach (var row in mainGrid.Rows.OfType<DataGridViewRow>())
        //    {
        //        row.Selected = false;
        //    }

        //    var matchingRow = mainGrid.Rows
        //        .OfType<DataGridViewRow>()
        //        .FirstOrDefault(i => (Guid) i.Cells[nameof(Plugin.UniqueId)].Value == guid);

        //    if (matchingRow == null) return;

        //    matchingRow.Selected = true;
        //    matchingRow.Cells[nameof(Plugin.Name)].Selected = true;

        //    SetButtonStates();
        //}

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DataGrid_SelectionChanged(object sender, EventArgs e)
        {
            SetButtonStates();
        }

        private void SetButtonStates()
        {
            var currentRow = mainGrid.CurrentRow;
            if (currentRow == null)
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }

            var seqString = (string) currentRow.Cells["Sequence"].Value;
            var seqNumber = int.Parse(seqString);

            btnUp.Enabled = seqNumber > 0;
            btnDown.Enabled = seqNumber < PluginManager.LoadedPlugins.Count - 1;
        }

        private void PopulateRows()
        {
            mainGrid.Rows.Clear();

            foreach (var enabledPlugin in PluginManager.LoadedPlugins)
            {
                var newRow = new DataGridViewRow();

                newRow.Cells.Add(new DataGridViewTextBoxCell
                {
                    Value = enabledPlugin.UniqueId
                });

                newRow.Cells.Add(new DataGridViewTextBoxCell
                {
                    Value = PluginManager.PluginSequence(enabledPlugin).ToString()
                });

                newRow.Cells.Add(new DataGridViewTextBoxCell
                {
                    Value = $"{enabledPlugin.Name} {enabledPlugin.Version} by {enabledPlugin.Author}"
                });

                newRow.Cells.Add(new DataGridViewButtonCell
                {
                    UseColumnTextForButtonValue = true
                });

                newRow.Cells.Add(new DataGridViewButtonCell
                {
                    Value = PluginManager.IsPluginEnabled(enabledPlugin) ? "Enabled" : "Disabled"
                });

                mainGrid.Rows.Add(newRow);
            }
        }

        private void mainGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == mainGrid.Columns["Settings"]?.Index)
            {
                var row = mainGrid.Rows[e.RowIndex];
                var guid = (Guid) row.Cells[nameof(Plugin.UniqueId)].Value;

                var plugin = PluginManager.LoadedPlugins.First(i => i.UniqueId == guid);

                var settingsForm = new FormSettings();
                settingsForm.Setup(plugin);
                settingsForm.ShowDialog(this);
                settingsForm.Dispose();

                DbHelper.SavePluginSettings(plugin, plugin.Settings);
            }

            // ReSharper disable once InvertIf
            if (e.ColumnIndex == mainGrid.Columns["Enabled"]?.Index)
            {
                var row = mainGrid.Rows[e.RowIndex];
                var guid = (Guid) row.Cells[nameof(Plugin.UniqueId)].Value;

                var plugin = PluginManager.LoadedPlugins.First(i => i.UniqueId == guid);

                var currentState = PluginManager.IsPluginEnabled(plugin);

                var newState = !currentState;

                PluginManager.SetPluginState(plugin, newState);

                row.Cells["Enabled"].Value = newState ? "Enabled" : "Disabled";
            }
        }
    }
}
