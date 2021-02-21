using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MisterDoctor.Plugins.Classes;

namespace MisterDoctor.Forms
{
    public partial class FormSettingStringList : Form
    {
        public FormSettingStringList(Setting setting)
        {
            InitializeComponent();

            Setting = setting;

            lblDescription.Text = setting.Description;

            foreach (var value in setting.ValueStringList)
            {
                _propertyMapper.Add(new StringValue(value));
            }

            gridMain.DataSource = _propertyMapper;

            gridMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gridMain.AllowUserToAddRows = true;
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using var input = new FormInput
            {
                Text = Setting.Name
            };

            input.ShowDialog(this);

            var returnVal = input.Value;
            if (string.IsNullOrEmpty(returnVal)) return;

            _propertyMapper.Add(new StringValue(returnVal));
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (gridMain.CurrentRow?.DataBoundItem is not StringValue currentItem) return;

            var message = $"Remove '{currentItem.Value}'?";

            var result = MessageBox.Show(this, message, Setting.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            _propertyMapper.Remove(currentItem);
        }

        public Setting Setting { get; }

        public List<string> ReturnValue
        {
            get
            {
                return _propertyMapper.Select(i => i.Value).ToList();
            }
        }

        private readonly BindingList<StringValue> _propertyMapper = new ();
    }

    internal class StringValue
    {
        public StringValue(string s)
        {
            Value = s;
        }

        public string Value { get; set; }
    }
}
