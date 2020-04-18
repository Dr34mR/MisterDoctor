namespace MisterDoctor.Classes
{
    internal class Command
    {
        public int Id { get; set; }

        public string Cmd { get; set; }

        public string Response { get; set; }

        public override string ToString()
        {
            return $"{Cmd} = {Response}";
        }
    }
}