using System;

namespace MisterDoctor.Classes
{
    public class PluginState
    {
        public Guid Id { get; set; }

        public int Sequence { get; set; }

        public bool Enabled { get; set; }
    }
}