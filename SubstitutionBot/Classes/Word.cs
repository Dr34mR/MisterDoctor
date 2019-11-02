namespace SubstitutionBot.Classes
{
    internal class Word
    {
        private string _value;

        public int Id { get; set; }

        public string Value
        {
            get => _value;
            set => _value = value.Trim();
        }

    }
}
