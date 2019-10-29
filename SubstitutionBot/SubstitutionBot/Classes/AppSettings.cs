namespace SubstitutionBot.Classes
{
    internal class AppSettings
    {
        public int Id { get; set; }
        public string ChannelName { get; set; }
        public bool AutoConnect { get; set; }

        public override string ToString()
        {
            return $"{Id} {ChannelName}";
        }
    }
}
