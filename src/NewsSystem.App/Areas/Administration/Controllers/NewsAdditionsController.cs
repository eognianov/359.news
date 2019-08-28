using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSystem.App.Controllers;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.ViewModels;

namespace NewsSystem.App.Areas.Administration.Controllers
{
    public class NewsAdditionsController:BaseController
    {
        private readonly IDeletableEntityRepository<News> newsRepository;
        private readonly IRepository<Video> videoRepository;

        public NewsAdditionsController(IDeletableEntityRepository<News> newsRepository, IRepository<Video> videoRepository)
        {
            this.newsRepository = newsRepository;
            this.videoRepository = videoRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddVideo(AddVideoInputModel model)
        {
            if (ModelState.IsValid)
            {
                var news = await newsRepository.GetByIdWithDeletedAsync(model.NewsId);
                news.Videos.Add(new Video{Url = model.VideoUrl});
                await newsRepository.SaveChangesAsync();
            }

            return Redirect(model.NewsUrl);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVideo(int videoId, string returnUrl)
        {
            var video = await videoRepository.All().FirstOrDefaultAsync(v => v.Id == videoId);
            if (video!=null)
            {
                videoRepository.Delete(video);
                await videoRepository.SaveChangesAsync();
            }
            return Redirect(returnUrl);
        }
    }
}
