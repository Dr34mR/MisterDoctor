namespace SubstitutionBot.Classes
{
    public class MessagePart
    {
        public string Value { get; set; }

        public bool IsNoun { get; set; }

        public bool IsWord { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
