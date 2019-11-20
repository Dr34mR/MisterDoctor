namespace MisterDoctor.Classes
{ 
    internal class Substitution
    {
        public int Id { get; set; }

        public string Find { get; set; }

        public string Reply { get; set; }

        public override string ToString()
        {
            return $"{Find} = {Reply}";
        }
    }
}