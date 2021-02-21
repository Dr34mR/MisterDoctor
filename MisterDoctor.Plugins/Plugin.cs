using System;
using System.ComponentModel.Composition;
using System.Linq;
using MisterDoctor.Plugins.Classes;

namespace MisterDoctor.Plugins
{
    [InheritedExport(typeof(Plugin))]
    public abstract class Plugin
    {
        /// <summary>
        ///     A unique id used for tracking the plugin
        /// </summary>
        public abstract Guid UniqueId { get; }

        /// <summary>
        ///     Name of the plugin
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     The version of the plugin
        /// </summary>
        public abstract string Version { get; }

        /// <summary>
        ///     The author of the plugin
        /// </summary>
        public abstract string Author { get; }

        /// <summary>
        ///     The description of the plugin
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        ///     An initialize routine called when the plugin is loaded
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        ///     Whether or not the plugin is initialized.
        ///     It may be initialized by its own constructor, or it may need to be called manually with an Initialize call.
        /// </summary>
        public abstract bool IsInitialized { get; set; }

        /// <summary>
        ///     Method for returning the default set of settings
        /// </summary>
        /// <returns></returns>
        public abstract Settings GetDefaultSettings();

        /// <summary>
        /// 
        /// </summary>
        public abstract Settings Settings { get; }

        /// <summary>
        ///     Load the settings into the plugin. These settings are saved and loaded into the plugin via the host application
        /// </summary>
        /// <param name="settings"></param>
        public abstract void LoadSettings(Settings settings);

        /// <summary>
        ///     A message sent by the host application
        /// </summary>
        /// <param name="message">The message received in chat</param>
        public abstract bool ReceiveMessage(DigestMessage message);

        /// <summary>
        ///     An event that the host application will hook into
        /// </summary>
        public event EventHandler<SendMessageArgs> SendMessageHandler;

        /// <summary>
        ///     An event that the host applicaiton will hook into
        /// </summary>
        public event EventHandler<TimeoutArgs> TimeoutHandler;

        /// <summary>
        ///     Internal routine to invoke the SendMessageHandler
        /// </summary>
        /// <param name="message">The reply to send</param>
        /// <param name="original">The original twitch message</param>
        public void SendMessage(string message, DigestMessage original)
        {
            if (string.IsNullOrEmpty(message)) return;
            SendMessageHandler?.Invoke(this, new SendMessageArgs { Reply = message, OriginalMessage = original });
        }

        /// <summary>
        ///     Internal routine to invoke the TimeoutHandler
        /// </summary>
        /// <param name="args"></param>
        public void SendTimeout(TimeoutArgs args)
        {
            if (string.IsNullOrEmpty(args?.Username)) return;

            TimeoutHandler?.Invoke(this, args);
        }

        /// <summary>
        ///     Internal routine to invoke the SendMessageHandler
        /// </summary>
        /// <param name="message">The reply to send</param>
        public void SendMessage(string message)
        {
            SendMessage(message, null);
        }

        public Settings CleanSettings()
        {
            return CleanSettings(Settings);
        }

        public Settings CleanSettings(Settings inSettings)
        {
            var defaults = GetDefaultSettings();
         
            // Remove settings that dont have a matching default key name

            inSettings.RemoveAll(i => defaults.All(d => d.Name != i.Name));

            // Add defaults that aren't in the passed in list

            inSettings.AddRange(defaults.Where(d => inSettings.All(i => i.Name != d.Name)));
         
            // Ensure the descriptions are up to date

            foreach (var setting in inSettings)
            {
                var matchingDefault = defaults.FirstOrDefault(i => i.Name == setting.Name);
                if (matchingDefault == null) continue;
                setting.Description = matchingDefault.Description;
            }

            return inSettings;
        }
    }
}
