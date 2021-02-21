using MisterDoctor.Plugins.Enums;

namespace MisterDoctor.Plugins.Classes
{
    public class DigestMessage
    {
        public string ChannelName { get; set; }

        public string BotUsername { get; set; } = string.Empty;

        public string FromAccount { get; set; } = string.Empty;

        public UserType UserType { get; set; }
        
        public bool IsSub { get; set; }

        public bool IsFollower { get; set; }

        public bool IsIgnored { get; set; }

        public MessageParts Message { get; set; } = null;
    }
}
