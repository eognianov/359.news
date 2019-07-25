using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSystem.Common;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.ViewModels;
using NewsSystem.Mappings;

namespace NewsSystem.App.Controllers
{
    public class NewsController : BaseController
    {
        private const int ItemsPerPage = 10;

        private readonly IPublishableEntityRepository<News> newsRepository;

        public NewsController(IPublishableEntityRepository<News> newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        public IActionResult List(int id, string search)
        {
            id = Math.Max(1, id);
            var skip = (id - 1) * ItemsPerPage;
            var query = this.newsRepository.AllPublished();
            var words = search?.Split(' ').Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length >= 2).ToList();
            if (words != null)
            {
                foreach (var word in words)
                {
                    query = query.Where(c => EF.Functions.FreeText(c.SearchText, word));
                    //// query = query.Where(x => x.SearchText.Contains(word));
                }
            }

            var news = query
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.Id)
                .Skip(skip)
                .Take(ItemsPerPage)
                .To<NewsViewModel>()
                .ToList();
            var newsCount = query.Count();
            var pagesCount = (int)Math.Ceiling(newsCount / (decimal)ItemsPerPage);
            var viewModel = new NewsListViewModel
            {
                News = news,
                CurrentPage = id,
                PagesCount = pagesCount,
                NewsCount = newsCount,
                Search = search,
            };
            return this.View(viewModel);
        }

        public IActionResult ById(int id, string slug)
        {
            var news = this.newsRepository.All().Where(x => x.Id == id).To<NewsViewModel>().FirstOrDefault();

            if (news == null || news.IsDeleted)
            {
                return NotFound();

            }
            else if(news.isPublished || 
                    User.IsInRole(GlobalConstants.EditorRoleName) || 
                    User.IsInRole(GlobalConstants.AdministratorRoleName) || 
                    (User.Identity.IsAuthenticated && news.Author?.UserName == User.Identity.Name))

            {
                return View(news);
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}