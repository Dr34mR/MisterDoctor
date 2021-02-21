using System;
using System.Windows.Forms;
using MisterDoctor.Plugins.Classes;

namespace MisterDoctor.Forms
{
    public partial class FormSettingInt : Form
    {
        public int ReturnValue { get; private set; }

        public FormSettingInt(Setting setting)
        {
            InitializeComponent();

            Setting = setting;

            ReturnValue = setting.ValueInt;

            lblText.Text = setting.Description;

            txtValue.Text = setting.ValueInt.ToString();

            txtValue.TextChanged += txtValue_TextChanged;
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            var newText = txtValue.Text.Trim();

            if (!int.TryParse(newText, out var newInt)) return;

            ReturnValue = newInt;
        }

        public Setting Setting { get; }
    }
}
