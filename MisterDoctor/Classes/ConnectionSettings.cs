namespace MisterDoctor.Classes
{
    public class ConnectionSettings
    { 
        public string ChannelName { get; set; } = string.Empty;

        public bool AutoConnect { get; set; }

        public string BotUsername { get; set; } = string.Empty;

        public string BotOAuthKey { get; set; } = string.Empty;

        public string BotClientId { get; set; } = string.Empty;
    }
}
