using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MisterDoctor.Plugins.Classes;

namespace MisterDoctor.Forms
{
    public partial class FormSettingKeyValue : Form
    {
        public FormSettingKeyValue(Setting setting)
        {
            InitializeComponent();

            Setting = setting;

            lblDescription.Text = setting.Description;

            foreach (var value in Setting.ValueKeyValues)
            {
                _propertyMapper.Add(value);
            }

            gridMain.DataSource = _propertyMapper;

            gridMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gridMain.AllowUserToAddRows = false;
            gridMain.AllowUserToOrderColumns = false;
            gridMain.ColumnHeadersVisible = false;
            gridMain.RowHeadersVisible = false;

            gridMain.AllowUserToResizeColumns = false;
            gridMain.AllowUserToResizeRows = false;

            gridMain.BackColor = BackColor;
            gridMain.BackgroundColor = BackColor;

            gridMain.BorderStyle = BorderStyle.None;

            btnAdd.Click += btnAdd_Click;
            btnRemove.Click += btnRemove_Click;
        }

        public sealed override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (gridMain.CurrentRow?.DataBoundItem is not KeyValue currentItem) return;

            var message = $"Remove '{currentItem.Key}'";

            var result = MessageBox.Show(this, message, Setting.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            _propertyMapper.Remove(currentItem);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using var input = new FormInputDual
            {
                Text = Setting.Name
            };

            input.ShowDialog(this);

            var returnKey = input.Key?.Trim() ?? string.Empty;
            var returnVal = input.Value?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(returnKey)) return;
            if (string.IsNullOrEmpty(returnVal)) return;

            _propertyMapper.Add(new KeyValue
            {
                Key = returnKey,Value = returnVal
            });
        }

        public Setting Setting { get; }

        public KeyValues ReturnValue => new(_propertyMapper);

        private readonly BindingList<KeyValue> _propertyMapper = new ();
    }
}
