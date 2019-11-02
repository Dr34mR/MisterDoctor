namespace SubstitutionBot.Classes
{
    internal class AppSettings
    {
        public int Id { get; set; }

        public string ChannelName { get; set; }
        public bool AutoConnect { get; set; }
        public int ProcChance { get; set; } = 8;
        public int CoolDown { get; set; } = 30;

        public override string ToString()
        {
            return $"{Id} {ChannelName}";
        }
    }
}
