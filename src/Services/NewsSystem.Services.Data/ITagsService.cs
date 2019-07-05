using System.Threading.Tasks;

namespace NewsSystem.Services.Data
{
    public interface ITagsService
    {
        Task UpdateTagsAsync(int id, string content);
    }
}
