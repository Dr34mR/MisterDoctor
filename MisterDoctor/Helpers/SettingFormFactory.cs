using System;
using System.Windows.Forms;
using MisterDoctor.Forms;
using MisterDoctor.Plugins.Classes;
using MisterDoctor.Plugins.Enums;

namespace MisterDoctor.Helpers
{
    public static class SettingFormFactory
    {
        public static Form CreateForm(Setting setting)
        {
            return setting.Type switch
            {
                SettingType.Boolean => new FormSettingBoolean(setting),
                SettingType.Long => new FormSettingLong(setting),
                SettingType.Int => new FormSettingInt(setting),
                SettingType.String => new FormSettingString(setting),
                SettingType.StringList => new FormSettingStringList(setting),
                SettingType.KeyValueList => new FormSettingKeyValue(setting),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
