using System.Windows.Forms;
using MisterDoctor.Plugins.Classes;

namespace MisterDoctor.Forms
{
    public partial class FormSettingBoolean : Form
    {
        public FormSettingBoolean(Setting setting)
        {
            InitializeComponent();

            Setting = setting;
        }

        public Setting Setting { get; }
    }
}
