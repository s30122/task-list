namespace Tasks
{
    public class ExecuteCommand
    {
        private readonly string _commandLine;

        public ExecuteCommand(string commandLine)
        {
            _commandLine = commandLine;
        }

        private string[] SplitCommands => _commandLine.Split(" ".ToCharArray(), 2);

        public string Action => SplitCommands[0];

        public string RestCommand => SplitCommands[1];
    }
}