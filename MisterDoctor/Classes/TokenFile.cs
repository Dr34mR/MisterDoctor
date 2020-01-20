namespace MisterDoctor.Classes
{
    internal class Token
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string UserOAuthKey { get; set; }
        
        public string ClientId { get; set; }
        
        public override string ToString()
        {
            return $"{Id} - {Username}";
        }
    }
}
