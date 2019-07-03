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
    public class TopNewsViewComponent : ViewComponent
    {
        private readonly IDeletableEntityRepository<TopNews> topNewsRepository;

        public TopNewsViewComponent(IDeletableEntityRepository<TopNews> topNewsRepository)
        {
            this.topNewsRepository = topNewsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var news = this.topNewsRepository.All().Select(tp=>new NewsViewModel
            {
                Id = tp.News.Id,
                ImageUrl = tp.News.ImageUrl,
                Title = tp.News.Title,
                Content = tp.News.Content,
                ContentLenght = 485
            });

            var model = new NewsListViewModel
            {
                News = news
            };
            return  this.View(model);
        }
    }
}