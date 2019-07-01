using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewsSystem.Services.Data
{
    public interface IImagesServices
    {
        Task<string> SaveImage(IFormFile file);
    }
}
