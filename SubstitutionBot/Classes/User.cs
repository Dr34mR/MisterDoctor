namespace SubstitutionBot.Classes
{
    internal class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public override string ToString()
        {
            return Username;
        }
    }
}
