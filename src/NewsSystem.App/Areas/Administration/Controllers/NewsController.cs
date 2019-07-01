using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsSystem.App.Areas.Administration.Models;
using NewsSystem.App.Controllers;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.Services.Data;
using NewsSystem.ViewModels;
using NewsSystem.Mappings;
using Microsoft.AspNetCore.Authorization;
using NewsSystem.Common;

namespace NewsSystem.App.Areas.Administration.Controllers
{    
    [Authorize(Roles = GlobalConstants.EditorRoleName)]
    [Authorize(Roles = GlobalConstants.ReporterRoleName)]
    public class NewsController :AdministrationController
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
                ImageUrl = news.ImageUrl
            };
            return this.View("Update", model);
        }

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

        public async Task<IActionResult> TopNews(int id)
        {
            var topNews = new TopNews(){NewsId = id};
            await this.TopNewsRepository.AddAsync(topNews);
            await this.TopNewsRepository.SaveChangesAsync();
            return this.RedirectToAction("List", "News", new { area = string.Empty });
        }
 
        public async Task<IActionResult> SoftDelete(int id)
        {
            var news = this.newsRepository.All().FirstOrDefault(x => x.Id == id);
            this.newsRepository.Delete(news);
            await this.newsRepository.SaveChangesAsync();

            return this.RedirectToAction("List", "News", new { area = string.Empty });
        }

        public async Task<IActionResult> HardDelete(int id)
        {
            var news = this.newsRepository.All().FirstOrDefault(x => x.Id == id);
            this.newsRepository.HardDelete(news);
            await this.newsRepository.SaveChangesAsync();

            return this.RedirectToAction("List", "News", new { area = string.Empty });
        }
    }
}