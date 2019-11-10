using System.Windows.Forms;
using Component;

namespace Substitution
{
    public class TwitchModule : ITwitchModule
    {
        public string Name { get; } = "Substitution";

        public Form Form
        {
            get
            {
                var mainForm = new Form();
                return mainForm;
            }
        }

        public string DigestMessage(string message)
        {
            return null;
        }

        public TwitchModule()
        {

        }
    }
}
