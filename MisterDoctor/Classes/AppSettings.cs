namespace MisterDoctor.Classes
{
    internal class AppSettings
    {
        public int Id { get; set; }

        public string ChannelName { get; set; }
        public bool AutoConnect { get; set; }
        
        public int ProcChance { get; set; } = 8;
        public int CoolDown { get; set; } = 30;

        public int MaxMessageSize { get; set; } = 12;

        public string CommandIgnore { get; set; } = "!ignoreme";
        public string CommandUnignore { get; set; } = "!unignoreme";

        public string GoodBot { get; set; } = "( ͡° ͜ʖ ͡°)";

        public string BadBot { get; set; } = "ಥ_ಥ";

        public override string ToString()
        {
            return $"{Id} {ChannelName}";
        }
    }
}
