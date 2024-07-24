namespace Tasks
{
    public class AddTaskCommand
    {
        private readonly string[] _command;

        public AddTaskCommand(string s)
        {
            _command = s.Split(" ".ToCharArray(), 2);
        }

        public string ProjectName => _command[0];

        public string Description => _command[1];
    }
}