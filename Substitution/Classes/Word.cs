namespace Substitution.Classes
{
    internal class Word
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
