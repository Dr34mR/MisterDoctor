using System;
using System.Windows.Forms;
using MisterDoctor.Plugins.Classes;

namespace MisterDoctor.Forms
{
    public partial class FormSettingString : Form
    {
        public string ReturnValue { get; private set; }

        public FormSettingString(Setting setting)
        {
            InitializeComponent();

            Setting = setting;

            lblDescription.Text = setting.Description;
            txtValue.Text = setting.ValueString;

            ReturnValue = setting.ValueString;

            txtValue.TextChanged += txtValue_TextChanged;
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            var newText = txtValue.Text.Trim();

            ReturnValue = newText;
        }

        public Setting Setting { get; }
    }
}
