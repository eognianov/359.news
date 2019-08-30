using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.Mappings;
using NewsSystem.ViewModels;

namespace NewsSystem.App.Areas.Administration.Controllers
{
    [Authorize]
    public class OfficeController : AdministrationController
    {
        private readonly IPublishableEntityRepository<News> newsRepository;

        public OfficeController(IPublishableEntityRepository<News> newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        [Route("/office")]
        public async Task<IActionResult> Index()
        {
            var news = await newsRepository.AllWithDeleted().ToListAsync();

            var all = news.Count();
            var published = news.Where(n => n.isPublished == true && n.IsDeleted==false).Count();
            var notPublished = news.Where(n => n.isPublished == false && n.IsDeleted == false).Count();
            var deleted = news.Where(n => n.IsDeleted == true).Count();

            var model = new DashboardIndexViewModel
            {
                AllPosts = all,
                PublishedPosts = published,
                NotPublishedPosts = notPublished,
                DeletedPosts = deleted
            };

            return View(model);
        }

        public async Task<IActionResult> NotPublished(string period)
        {
            //MEDIUM: extraxt
            var today = DateTime.Today;
            var yestarday = today.Date.Subtract(new TimeSpan(1, 0,0,0));
            var week = today.Date.Subtract(new TimeSpan(7,0,0,0));
            var news = new List<NewsViewModel>();
            var viewModel = new OfficeNewsListViewModel();

            var returnUrl = "/Administration/Office/NotPublished";


            switch (period)
            {
                case "yestarday":
                    news = await this.newsRepository.AllNotPublished().Where(n=>n.CreatedOn.Date==yestarday).OrderByDescending(x => x.PublishedOn)
                        .ThenByDescending(x => x.Id).To<NewsViewModel>().ToListAsync();
                    viewModel.News = news;
                    returnUrl += "?period=yestarday";

                    break;

                case "week":
                    news = await this.newsRepository.AllNotPublished().Where(n=>n.CreatedOn.Date<=yestarday && n.CreatedOn>=week).OrderByDescending(x => x.PublishedOn)
                        .ThenByDescending(x => x.Id).To<NewsViewModel>().ToListAsync();
                    viewModel.News = news;
                    returnUrl += "?period=week";

                    break;

                default:
                    news = await this.newsRepository.AllNotPublished().Where(n=>n.CreatedOn.Date==today).OrderByDescending(x => x.PublishedOn)
                        .ThenByDescending(x => x.Id).To<NewsViewModel>().ToListAsync();
                    viewModel.News = news;
                    break;
            }

            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View(viewModel);
        }

        public async Task<IActionResult> Published(string period)
        {
            var today = DateTime.Today;
            var yestarday = today.Date.Subtract(new TimeSpan(1,0,0,0));
            var week = today.Date.Subtract(new TimeSpan(7,0,0,0));
            var news = new List<NewsViewModel>();
            var viewModel = new OfficeNewsListViewModel();




            switch (period)
            {
                case "yestarday":
                    news = await this.newsRepository.AllPublished().Where(n=>n.PublishedOn.GetValueOrDefault().Date==yestarday).OrderByDescending(x => x.PublishedOn)
                        .ThenByDescending(x => x.Id).To<NewsViewModel>().ToListAsync();
                    viewModel.News = news;
                    break;

                case "week":
                    news = await this.newsRepository.AllPublished().Where(n=>n.PublishedOn.GetValueOrDefault().Date<=yestarday && n.PublishedOn.GetValueOrDefault().Date>=week).OrderByDescending(x => x.PublishedOn)
                        .ThenByDescending(x => x.Id).To<NewsViewModel>().ToListAsync();
                    viewModel.News = news;
                    break;

                default:
                    news = await this.newsRepository.AllPublished().Where(n=>n.PublishedOn.GetValueOrDefault().Date==today).OrderByDescending(x => x.PublishedOn)
                        .ThenByDescending(x => x.Id).To<NewsViewModel>().ToListAsync();
                    viewModel.News = news;
                    break;
            }
            
            return this.View(viewModel);
        }

        public async Task<IActionResult> Deleted(string period)
        {
            //MEDIUM: extraxt
            var today = DateTime.Today;
            var yestarday = today.Date.Subtract(new TimeSpan(1, 0,0,0));
            var week = today.Date.Subtract(new TimeSpan(7,0,0,0));
            var news = new List<NewsViewModel>();
            var viewModel = new OfficeNewsListViewModel();

            var returnUrl = "/Administration/Office/NotPublished";


            switch (period)
            {
                case "yestarday":
                    news = await this.newsRepository.AllWithDeleted().Where(n=>n.IsDeleted==true && n.DeletedOn.GetValueOrDefault().Date==yestarday).OrderByDescending(x => x.DeletedOn)
                        .ThenByDescending(x => x.Id).To<NewsViewModel>().ToListAsync();
                    viewModel.News = news;
                    returnUrl += "?period=yestarday";

                    break;

                case "week":
                    news = await this.newsRepository.AllWithDeleted().Where(n=>n.IsDeleted==true && n.DeletedOn.GetValueOrDefault().Date<=yestarday && n.DeletedOn.GetValueOrDefault().Date>=week).OrderByDescending(x => x.DeletedOn)
                        .ThenByDescending(x => x.Id).To<NewsViewModel>().ToListAsync();
                    viewModel.News = news;
                    returnUrl += "?period=week";

                    break;

                default:
                    news = await this.newsRepository.AllWithDeleted().Where(n=>n.DeletedOn.GetValueOrDefault().Date==today && n.IsDeleted==true).OrderByDescending(x => x.DeletedOn)
                        .ThenByDescending(x => x.Id).To<NewsViewModel>().ToListAsync();
                    viewModel.News = news;
                    break;
            }

            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View(viewModel);
        }
    }
}