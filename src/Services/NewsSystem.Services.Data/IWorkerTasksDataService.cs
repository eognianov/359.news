namespace NewsSystem.Services.Data
{
    using System.Threading.Tasks;

    using NewsSystem.Data.Models;

    public interface IWorkerTasksDataService
    {
        WorkerTask GetForProcessing();

        Task UpdateAsync(WorkerTask workerTask);

        Task AddAsync(WorkerTask workerTask);
    }
}
