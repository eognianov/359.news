using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSystem.App.Controllers;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.Services.Data;
using NewsSystem.ViewModels;

namespace NewsSystem.App.Areas.Administration.Controllers
{
    [Authorize]
    //HIGH: Admin, Editor or Author
    public class NewsAdditionsController:BaseController
    {
        private readonly IDeletableEntityRepository<News> newsRepository;
        private readonly IRepository<Video> videoRepository;
        private readonly IImagesServices imageService;
        private readonly IRepository<Photo> photoRepository;

        public NewsAdditionsController(IDeletableEntityRepository<News> newsRepository, IRepository<Video> videoRepository, IImagesServices imageService, IRepository<Photo> photoRepository)
        {
            this.newsRepository = newsRepository;
            this.videoRepository = videoRepository;
            this.imageService = imageService;
            this.photoRepository = photoRepository;
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

        [HttpPost]
        public async Task<IActionResult> AddPhoto(AddPhotoInputModel input)
        {
            if (ModelState.IsValid && input.Image!=null)
            {
                var photoUrl = await imageService.SaveImage(input.Image);
                var news = await newsRepository.GetByIdWithDeletedAsync(input.NewsId);
                news.Photos.Add(new Photo{Url = photoUrl});
                await newsRepository.SaveChangesAsync();
            }
            return Redirect(input.NewsUrl);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(string photoId, string returnUrl)
        {
            var photo = await photoRepository.All().FirstOrDefaultAsync(p => p.Id == photoId);
            if (photo!=null)
            {
                photoRepository.Delete(photo);
                await photoRepository.SaveChangesAsync();
            }
            return Redirect(returnUrl);
        }
    }
}
