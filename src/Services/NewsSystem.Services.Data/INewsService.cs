using System.Threading.Tasks;
using NewsSystem.Data.Models;
using NewsSystem.ViewModels;

namespace NewsSystem.Services.Data
{
    public interface INewsService
    {
        Task<bool> AddAsync(RemoteNews remoteNews, int sourceId);

        Task<bool> AddAsync(NewsInputModel input);

        Task UpdateAsync(int id, RemoteNews remoteNews);

        int Count();

        string GetSearchText(News news);
    }
}