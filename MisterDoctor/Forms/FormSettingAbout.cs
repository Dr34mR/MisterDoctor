using System;
using System.Drawing;
using System.Windows.Forms;
using MisterDoctor.Plugins;

namespace MisterDoctor.Forms
{
    public partial class FormSettingAbout : Form
    {
        public FormSettingAbout(Plugin plugin)
        {
            InitializeComponent();

            lblName.Text = plugin.Name;
            lblAuthor.Text = plugin.Author;
            lblExtra.Text = plugin.Description;

            lblAuthor.Font = new Font(lblAuthor.Font, FontStyle.Italic);

            btnExport.Click += btnExport_Click;
            btnImport.Click += btnImport_Click;
        }

        public event EventHandler<EventArgs> ExportSettings;

        public event EventHandler<EventArgs> ImportSettings;

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportSettings?.Invoke(this, EventArgs.Empty);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            ImportSettings?.Invoke(this, EventArgs.Empty);           
        }
    }
}