namespace NewsSystem.Worker.Common
{
    using System.Reflection;

    public interface ITasksAssemblyProvider
    {
        Assembly GetAssembly();
    }
}
