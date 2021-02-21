using System;
using MisterDoctor.Plugins;
using MisterDoctor.Plugins.Classes;

namespace MisterDoctor.Classes
{
    public class SettingsDto
    {
        public Guid Id { get; set; }

        public Settings Settings { get; set; }

        // ReSharper disable once UnusedMember.Global
        public SettingsDto()
        {
            
        }

        public SettingsDto(Plugin plugin, Settings settings)
        {
            Id = plugin.UniqueId;
            Settings = settings;
        }
    }
}
