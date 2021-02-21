using System.Collections.Generic;
using System.Linq;
using MisterDoctor.Plugins.Enums;

namespace MisterDoctor.Plugins.Classes
{
    public class Settings : List<Setting>
    {
        public void TrimValues()
        {
            foreach (var setting in this)
            {
                switch (setting.Type)
                {
                    case SettingType.String:
                    {
                        setting.ValueString = setting.ValueString?.Trim() ?? string.Empty;
                        break;
                    }
                    case SettingType.StringList:
                    {
                        setting.ValueStringList = setting.ValueStringList
                            .Where(i => !string.IsNullOrEmpty(i.Trim()))
                            .Select(i => i.Trim())
                            .OrderBy(i => i)
                            .ToList();
                        break;
                    }
                    case SettingType.KeyValueList:
                    {
                        var keyValue = setting.ValueKeyValues;
                        var newVals = new KeyValues();
                        foreach (var key in keyValue)
                        {
                            key.Key = key.Key.Trim();
                            key.Value = key.Value.Trim();

                            if (string.IsNullOrEmpty(key.Key)) continue;
                            if (string.IsNullOrEmpty(key.Value)) continue;

                            newVals.Add(key);
                        }
                        setting.ValueKeyValues = new KeyValues(newVals.OrderBy(i => i.Key));
                        break;
                    }
                }
            }
        }
    }
}