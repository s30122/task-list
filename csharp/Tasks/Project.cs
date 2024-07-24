namespace Tasks
{
    public class Project
    {
        public Project(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}