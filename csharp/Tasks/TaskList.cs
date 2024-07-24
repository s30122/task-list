using System;
using System.Collections.Generic;
using System.Linq;

namespace Tasks
{
    public sealed class TaskList
    {
        private const string Quit = "quit";

        private readonly IDictionary<string, IList<Task>> _tasks = new Dictionary<string, IList<Task>>();
        private readonly IConsole _console;

        private long _lastId;

        public static void Main(string[] args)
        {
            new TaskList(new RealConsole()).Run();
        }

        public TaskList(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            while (true)
            {
                _console.Write("> ");
                var command = _console.ReadLine();

                if (command == Quit)
                {
                    break;
                }

                Execute(new ExecuteCommand(command));
            }
        }

        private void Execute(ExecuteCommand executeCommand)
        {
            switch (executeCommand.Action)
            {
                case "show":
                    Show();

                    break;

                case "add":
                    Add(new AddCommand(executeCommand.RestCommand));

                    break;

                case "check":
                    Check(executeCommand.RestCommand);

                    break;

                case "uncheck":
                    Uncheck(executeCommand.RestCommand);

                    break;

                case "help":
                    Help();

                    break;

                default:
                    Error(executeCommand.Action);

                    break;
            }
        }

        private void Show()
        {
            foreach (var project in _tasks)
            {
                _console.WriteLine(project.Key);

                foreach (var task in project.Value)
                {
                    _console.WriteLine("    [{0}] {1}: {2}", task.Done ? 'x' : ' ', task.Id, task.Description);
                }

                _console.WriteLine();
            }
        }

        private void Add(AddCommand addCommand)
        {
            if (addCommand.ActionIsProject())
            {
                var project = new Project(addCommand.Content);
                AddProject(project);
            }
            else if (addCommand.ActionIsTask())
            {
                var projectTask = new AddTaskCommand(addCommand.Content);
                AddTask(projectTask.ProjectName, projectTask.Description);
            }
        }

        private void AddProject(Project project)
        {
            _tasks[project.Name] = new List<Task>();
        }

        private void AddTask(string project, string description)
        {
            if (!_tasks.TryGetValue(project, out IList<Task> projectTasks))
            {
                Console.WriteLine("Could not find a project with the name \"{0}\".", project);

                return;
            }

            projectTasks.Add(new Task
            {
                Id = NextId(),
                Description = description,
                Done = false
            });
        }

        private void Check(string idString)
        {
            SetDone(new CheckCommand(idString, true));
        }

        private void Uncheck(string idString)
        {
            SetDone(new CheckCommand(idString, false));
        }

        private void SetDone(CheckCommand checkCommand)
        {
            var identifiedTask = _tasks
                                 .SelectMany(x => x.Value)
                                 .FirstOrDefault(x => x.Id == checkCommand.Id);

            if (identifiedTask == null)
            {
                _console.WriteLine("Could not find a task with an ID of {0}.", checkCommand.Id);

                return;
            }

            identifiedTask.Done = checkCommand.Done;
        }

        private void Help()
        {
            _console.WriteLine("Commands:");
            _console.WriteLine("  show");
            _console.WriteLine("  add project <project name>");
            _console.WriteLine("  add task <project name> <task description>");
            _console.WriteLine("  check <task ID>");
            _console.WriteLine("  uncheck <task ID>");
            _console.WriteLine();
        }

        private void Error(string command)
        {
            _console.WriteLine("I don't know what the command \"{0}\" is.", command);
        }

        private long NextId()
        {
            return ++_lastId;
        }
    }
}