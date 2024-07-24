namespace Tasks
{
    public class CheckCommand
    {
        private readonly string _idString;

        public CheckCommand(string idString, bool done)
        {
            _idString = idString;
            Done = done;
        }

        public bool Done { get; private set; }

        public int Id => int.Parse(_idString);
    }
}