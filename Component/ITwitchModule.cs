using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace Component
{
    [InheritedExport(typeof(ITwitchModule))]
    public interface ITwitchModule
    {
        /// <summary>
        ///     The name of this bot component
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The form associated with this bot component
        /// </summary>
        Form Form { get; }

        /// <summary>
        ///     A message passed to the bot component
        /// </summary>
        /// <param name="message"></param>
        /// <returns>A reply message</returns>
        string DigestMessage(string message);
    }
}
