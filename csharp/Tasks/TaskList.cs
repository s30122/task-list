using System;
using System.Collections.Generic;
using System.Linq;

namespace Tasks
{
    public class ExecuteContent
    {
        public ExecuteContent(string commandLine)
        {
            var split  =commandLine.Split(" ".ToCharArray(), 2);
            ExecuteType = ConvertToExecuteType(split[0]);
            Content = split[1];
        }

        private ExecuteTypeEnum ConvertToExecuteType(string s)
        {
            return Enum.Parse<ExecuteTypeEnum>(s, true);
        }

        public string Content { get; set; }

        public ExecuteTypeEnum ExecuteType { get; set; }

        public string CommandLine { get; private set; }
    }

    public sealed class TaskList
    {
        private const string QUIT = "quit";

        private readonly IDictionary<string, IList<Task>> tasks = new Dictionary<string, IList<Task>>();
        private readonly IConsole console;

        private long lastId = 0;

        public static void Main(string[] args)
        {
            new TaskList(new RealConsole()).Run();
        }

        public TaskList(IConsole console)
        {
            this.console = console;
        }

        public void Run()
        {
            while (true)
            {
                console.Write("> ");
                var command = console.ReadLine();
                if (command == QUIT)
                {
                    break;
                }

                Execute(new ExecuteCommand(command));
            }
        }

        private void Execute(ExecuteCommand executeCommand)
        {
            switch (executeCommand.ActionType)
            {
                case ActionTypeEnum.Show:
                    Show();
                    break;
                case ActionTypeEnum.Add:
                    Add(new ExecuteContent(executeCommand.Command));
                    break;
                case ActionTypeEnum.Check:
                    Check(executeCommand.Command);
                    break;
                case ActionTypeEnum.UnCheck:
                    Uncheck(executeCommand.Command);
                    break;
                case ActionTypeEnum.Help:
                    Help();
                    break;
                default:
                    Error(executeCommand.ActionType.ToString());
                    break;
            }
        }

        private void Show()
        {
            foreach (var project in tasks)
            {
                console.WriteLine(project.Key);
                foreach (var task in project.Value)
                {
                    console.WriteLine("    [{0}] {1}: {2}", (task.Done ? 'x' : ' '), task.Id, task.Description);
                }

                console.WriteLine();
            }
        }

        private void Add(ExecuteContent executeContent)
        {
            var subcommandRest = executeContent.CommandLine.Split(" ".ToCharArray(), 2);
            var subcommand = subcommandRest[0];
            if (subcommand == "project")
            {
                AddProject(subcommandRest[1]);
            }
            else if (subcommand == "task")
            {
                var projectTask = subcommandRest[1].Split(" ".ToCharArray(), 2);
                AddTask(projectTask[0], projectTask[1]);
            }
        }

        private void AddProject(string name)
        {
            tasks[name] = new List<Task>();
        }

        private void AddTask(string project, string description)
        {
            if (!tasks.TryGetValue(project, out IList<Task> projectTasks))
            {
                Console.WriteLine("Could not find a project with the name \"{0}\".", project);
                return;
            }

            projectTasks.Add(new Task { Id = NextId(), Description = description, Done = false });
        }

        private void Check(string idString)
        {
            SetDone(idString, true);
        }

        private void Uncheck(string idString)
        {
            SetDone(idString, false);
        }

        private void SetDone(string idString, bool done)
        {
            int id = int.Parse(idString);
            var identifiedTask = tasks
                .Select(project => project.Value.FirstOrDefault(task => task.Id == id))
                .Where(task => task != null)
                .FirstOrDefault();
            if (identifiedTask == null)
            {
                console.WriteLine("Could not find a task with an ID of {0}.", id);
                return;
            }

            identifiedTask.Done = done;
        }

        private void Help()
        {
            console.WriteLine("Commands:");
            console.WriteLine("  show");
            console.WriteLine("  add project <project name>");
            console.WriteLine("  add task <project name> <task description>");
            console.WriteLine("  check <task ID>");
            console.WriteLine("  uncheck <task ID>");
            console.WriteLine();
        }

        private void Error(string command)
        {
            console.WriteLine("I don't know what the command \"{0}\" is.", command);
        }

        private long NextId()
        {
            return ++lastId;
        }
    }
}
