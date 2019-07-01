using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.ViewModels;
using NewsSystem.Mappings;

namespace NewsSystem.App.Components
{
    public class NewsViewComponent : ViewComponent
    {
        private readonly IDeletableEntityRepository<News> newsRepository;

        public NewsViewComponent(IDeletableEntityRepository<News> newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var news = this.newsRepository.All().OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.Id).Take(10).To<NewsViewModel>().ToList();
            var viewModel = new NewsListViewModel() { News = news };
            return  this.View(viewModel);
        }
    }
}