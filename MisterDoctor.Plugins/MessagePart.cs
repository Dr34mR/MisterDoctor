using System;

namespace MisterDoctor.Plugins
{
    public class MessagePart
    {
        public string Value { get; set; } = string.Empty;

        public bool IsNoun { get; set; }

        public bool IsWord { get; set; }

        public bool HasValue(string value)
        {
            return Value.Equals(value, StringComparison.CurrentCultureIgnoreCase);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}