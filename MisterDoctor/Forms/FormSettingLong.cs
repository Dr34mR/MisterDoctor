using System.Windows.Forms;
using MisterDoctor.Plugins.Classes;

namespace MisterDoctor.Forms
{
    public partial class FormSettingLong : Form
    {
        public FormSettingLong(Setting setting)
        {
            InitializeComponent();

            Setting = setting;
        }

        public Setting Setting { get; }
    }
}
