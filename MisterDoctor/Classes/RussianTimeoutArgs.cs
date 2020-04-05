using System;

namespace MisterDoctor.Classes
{
    public class RussianTimeoutArgs : EventArgs
    {
        public string Username { get; set; }
        public TimeSpan Duration { get; set; }
        public string Message { get; set; }
    }
}