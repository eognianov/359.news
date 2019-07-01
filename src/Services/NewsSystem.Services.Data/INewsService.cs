using System.Threading.Tasks;
using NewsSystem.Data.Models;
using NewsSystem.ViewModels;

namespace NewsSystem.Services.Data
{
    public interface INewsService
    {
        Task<bool> AddAsync(RemoteNews remoteNews, int sourceId);

        Task<int> AddAsync(NewsInputModel input);

        Task UpdateAsync(int id, RemoteNews remoteNews);

        Task<int> UpdateAsync(NewsUpdataInputModel input);

        int Count();

        string GetSearchText(News news);

        string GetUrlById(int id);
    }
}