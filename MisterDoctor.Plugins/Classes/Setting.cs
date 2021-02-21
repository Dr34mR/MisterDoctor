using System.Collections.Generic;
using MisterDoctor.Plugins.Enums;

namespace MisterDoctor.Plugins.Classes
{
    public class Setting
    {
        public string Name { get; set; } = string.Empty;

        public SettingType Type { get; set; } = SettingType.String;

        public bool ValueBool { get; set; } = false;

        public long ValueLong { get; set; } = 0L;

        public int ValueInt { get; set; } = 0;

        public string ValueString { get; set; } = string.Empty;

        public List<string> ValueStringList { get; set; } = new ();

        public KeyValues ValueKeyValues { get; set; } = new();

        public string Description { get; set; } = string.Empty;
    }
}