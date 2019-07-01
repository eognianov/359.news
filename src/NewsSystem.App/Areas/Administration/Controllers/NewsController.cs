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

        public IDeletableEntityRepository<TopNews> TopNewsRepository { get; }

        public NewsController(INewsService newsService, IDeletableEntityRepository<News> newsRepository,IDeletableEntityRepository<TopNews> topNewsRepository)
        {
            this.newsService = newsService;
            this.newsRepository = newsRepository;
            TopNewsRepository = topNewsRepository;
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
                await this.newsService.AddAsync(model);
                return this.RedirectToAction("Index", "Home");
            }


            return View(model);
        }

        public async Task<IActionResult> Udpate(int id)
        {
            //TODO: get news by id with specific method
            var news =newsRepository.All().FirstOrDefault(x => x.Id == id);
            var model = new NewsInputModel()
            {
                Content = news.Content,
                Title = news.Title
            };
            return this.View("Update",model);
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