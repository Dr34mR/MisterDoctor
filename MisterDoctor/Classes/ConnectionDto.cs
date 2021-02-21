namespace MisterDoctor.Classes
{
    internal class ConnectionDto : ConnectionSettings
    {
        public int Id { get; set; }

        public ConnectionDto()
        {
            
        }

        public ConnectionDto(ConnectionSettings settings)
        {
            AutoConnect = settings.AutoConnect;
            BotClientId = settings.BotClientId ?? string.Empty;
            BotOAuthKey = settings.BotOAuthKey ?? string.Empty;
            BotUsername = settings.BotUsername ?? string.Empty;
            ChannelName = settings.ChannelName ?? string.Empty;
        }

        public ConnectionSettings ToSetting()
        {
            return new()
            {
                AutoConnect = AutoConnect,
                BotClientId = BotClientId ?? string.Empty,
                BotOAuthKey = BotOAuthKey ?? string.Empty,
                BotUsername = BotUsername ?? string.Empty,
                ChannelName = ChannelName ?? string.Empty
            };
        }
    }
}
