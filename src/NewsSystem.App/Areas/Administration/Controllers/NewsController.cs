using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.Services.Data;
using NewsSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace NewsSystem.App.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator,Editor,Reporter")]
    public class NewsController : AdministrationController
    {
        private readonly INewsService newsService;
        private readonly IDeletableEntityRepository<News> newsRepository;
        private readonly IImagesServices imagesServices;

        public IDeletableEntityRepository<TopNews> TopNewsRepository { get; }

        public NewsController(INewsService newsService, IDeletableEntityRepository<News> newsRepository,IDeletableEntityRepository<TopNews> topNewsRepository, IImagesServices imagesServices)
        {
            this.newsService = newsService;
            this.newsRepository = newsRepository;
            //TODO:topNews
            TopNewsRepository = topNewsRepository;
            this.imagesServices = imagesServices;
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        //HIGH: Update only if is editor, admin or author
        [HttpPost]
        public async Task<IActionResult> Create(NewsInputModel model)
        {
            if (ModelState.IsValid)
            {
                model.AuthorId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var result =await this.newsService.AddAsync(model);
                return Redirect(newsService.GetUrlById(result));
            }

            var errors = ModelState.Values.ToList();

            return View(model);
        }

        public async Task<IActionResult> Udpate(int id)
        {
            var news = await newsRepository.GetByIdWithDeletedAsync(id);
            var model = new NewsUpdataInputModel()
            {
                Id = news.Id,
                Content = news.Content,
                Title = news.Title,
                Signature = news.Signature,
                ImageUrl = news.ImageUrl,
                Category = news.Category
            };
            return this.View("Update", model);
        }

        //HIGH: Update only if is editor, admin or author
        [HttpPost]
        public async Task<IActionResult> Update(NewsUpdataInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.View("Update", input);
            }

            var originalNews =await newsRepository.GetByIdWithDeletedAsync(input.Id);
            if (originalNews==null)
            {
                return NotFound($"Unable to load user with ID '{input.Id}'.");
            }
            
            await newsService.UpdateAsync(input);
             return Redirect(newsService.GetUrlById(input.Id));
        }

        [Authorize(Roles = "Administrator,Editor")]
        public async Task<IActionResult> TopNews(int id, bool remove=false)
        {

            if (remove)
            {
                var topNews = TopNewsRepository.All().FirstOrDefault(tp => tp.NewsId == id);
                this.TopNewsRepository.HardDelete(topNews);
            }
            else
            {
                var topNews = new TopNews(){NewsId = id};
                await this.TopNewsRepository.AddAsync(topNews);
            }
            await this.TopNewsRepository.SaveChangesAsync();
            return this.RedirectToAction("Index", "Home", new { area = string.Empty });
        }
 
        [Authorize(Roles = "Administrator,Editor")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var news = this.newsRepository.All().FirstOrDefault(x => x.Id == id);
            this.newsRepository.Delete(news);
            await this.newsRepository.SaveChangesAsync();

            return this.RedirectToAction("List", "News", new { area = string.Empty });
        }

        [Authorize(Roles = "Administrator,Editor")]
        public async Task<IActionResult> UnDelete(int id, string returnUrl = null)
        {
            var news = this.newsRepository.AllWithDeleted().FirstOrDefault(x => x.Id == id);
            this.newsRepository.Undelete(news);
            await this.newsRepository.SaveChangesAsync();

            if (returnUrl!=null)
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("List", "News", new { area = string.Empty });
        }

        [Authorize(Roles = "Administrator,Editor")]
        public async Task<IActionResult> HardDelete(int id)
        {
            //NORMAL: Move deleted to another table
            var news = this.newsRepository.AllWithDeleted().FirstOrDefault(x => x.Id == id);
            this.newsRepository.HardDelete(news);
            await this.newsRepository.SaveChangesAsync();

            return this.RedirectToAction("List", "News", new { area = string.Empty });
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost]
        public async Task<IActionResult> Publish(int newsId, string returnUrl = null)
        {
            var news = await this.newsRepository.GetByIdWithDeletedAsync(newsId);
            if (news == null)
            {
                return NotFound($"Unable to load user with ID '{newsId}'.");
            }
            news.isPublished = true;
            news.PublishedOn = DateTime.UtcNow;
            await this.newsRepository.SaveChangesAsync();
            if (returnUrl!=null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new {area = string.Empty});
        }
    }
}