namespace Tasks
{
    public class AddCommand
    {
        public AddCommand(string commandLine)
        {
            var split = commandLine.Split(" ".ToCharArray(), 2);
            Action = split[0];
            Content = split[1];
        }

        private string Action { get; }

        public bool ActionIsProject()
        {
            return Action == "project";
        }

        public bool ActionIsTask()
        {
            return Action == "task";
        }
        public string Content { get; }
    }
}