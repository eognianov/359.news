using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace NewsSystem.Services.Data
{
    public interface IImagesServices
    {
        Task<string> SaveImage(IFormFile file);
    }
}
