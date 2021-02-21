using System;

namespace MisterDoctor.Plugins.Classes
{
    public class TimeoutArgs
    {
        public string Username { get;set; }
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(0);
        public string Message { get;set; }
    }
}
