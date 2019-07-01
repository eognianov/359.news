using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NewsSystem.Services.Data
{
    public class ImagesServices:IImagesServices
    {
        private readonly IHostingEnvironment environment;

        public ImagesServices(IHostingEnvironment environment)
        {
            this.environment = environment;
        }
        public async Task<string> SaveImage(IFormFile file)
        {


            var uploadDir = "/media/photos/news/uploads/";

            var uploadPath = environment.WebRootPath + uploadDir;

            
            try
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                using (FileStream filestream = System.IO.File.Create(uploadPath+file.FileName))
                {
                    await file.CopyToAsync(filestream);
                    filestream.Flush();
                    return uploadDir + file.FileName;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }        
    }
}
