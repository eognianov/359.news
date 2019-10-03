using NewsSystem.Data.Models;
using System.Threading.Tasks;

namespace NewsSystem.Worker.Common
{
    public interface ITask
    {
        
        Task<string> DoWork(string parameters);

        WorkerTask Recreate(WorkerTask currentTask);
    }
}
