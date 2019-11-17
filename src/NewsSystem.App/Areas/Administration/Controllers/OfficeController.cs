using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IDeletableEntityRepository<Source> sourceRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public OfficeController(IPublishableEntityRepository<News> newsRepository, IDeletableEntityRepository<Source> sourceRepository, IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.newsRepository = newsRepository;
            this.sourceRepository = sourceRepository;
            this.usersRepository = usersRepository;
        }

        [Route("/office")]
        [AllowAnonymous]
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
                DeletedPosts = deleted,
                Stats = this.GetStat(),
                Authors = this.usersRepository.All()
            };

            model.Sources = this.sourceRepository.All().OrderBy(x => x.ShortName).To<SourceViewModel>().ToList();


            return View(model);
        }

        public async Task<IActionResult> NotPublished(string period)
        {
            //MEDIUM: extraxt
            var today = DateTime.Today;
            var yestarday = today.Date.Subtract(new TimeSpan(1, 0,0,0));
            var week = today.Date.Subtract(new TimeSpan(7,0,0,0));
            var mouth = today.Date.Subtract(new TimeSpan(30,0,0,0));
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

                    case "mouth":
                    news = await this.newsRepository.AllNotPublished().Where(n=>n.CreatedOn.Date<=yestarday && n.CreatedOn>=mouth).OrderByDescending(x => x.PublishedOn)
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

        private  StastsViewModel GetStat()
        {
            var allDates = this.newsRepository.All().Select(x => x.CreatedOn.Date).ToList();
            var byDateOfWeek = allDates.GroupBy(x => x.Date.DayOfWeek)
                .Select(g => new GroupByViewModel<DayOfWeek> { Group = g.Key, Count = g.Count() }).ToList();
            var byMonth = allDates.GroupBy(x => x.Date.Month)
                .Select(g => new GroupByViewModel<int> { Group = g.Key, Count = g.Count() }).ToList();
            var byYear = allDates.GroupBy(x => x.Date.Year)
                .Select(g => new GroupByViewModel<int> { Group = g.Key, Count = g.Count() }).ToList();
            var newsCount = this.newsRepository.All().Count();
            var newsToday = this.newsRepository.All().Count(x => x.CreatedOn.Date == DateTime.Today);
            var newsYesterday = this.newsRepository.All().Count(x => x.CreatedOn.Date == DateTime.Today.AddDays(-1));
            var newsTheDayBeforeYesterday = this.newsRepository.All().Count(x => x.CreatedOn.Date == DateTime.Today.AddDays(-2));
            var model = new StastsViewModel
            {
                NewsByDayOfWeek = byDateOfWeek,
                NewsByMonth = byMonth,
                NewsByYear = byYear,
                NewsCount = newsCount,
                NewsToday = newsToday,
                NewsYesterday = newsYesterday,
                NewsTheDayBeforeYesterday = newsTheDayBeforeYesterday,
                MouthStatNormalise = GetNomalise(byMonth)
            };

            return model;
        }

        private MouthStatNormalise GetNomalise(List<GroupByViewModel<int>> byMonth)
        {
            var result = new MouthStatNormalise();

            foreach (var item in byMonth.OrderBy(x=>x.Group))
            {
                result.Mounths.Add(new CultureInfo("bg-BG").DateTimeFormat.GetMonthName(item.Group));
                result.NewsCount.Add(item.Count);
            }

            return result;
        }
    }
}