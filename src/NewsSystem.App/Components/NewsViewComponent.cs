using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.ViewModels;
using NewsSystem.Mappings;

namespace NewsSystem.App.Components
{
    public class NewsViewComponent : ViewComponent
    {
        private readonly IPublishableEntityRepository<News> newsRepository;

        public NewsViewComponent(IPublishableEntityRepository<News> newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var news =await this.newsRepository.AllPublished().OrderByDescending(x => x.PublishedOn)
                .ThenByDescending(x => x.Id).Take(10).To<NewsViewModel>().ToListAsync();
            var viewModel = new NewsListViewModel() { News = news };
            return  this.View(viewModel);
        }
    }
}