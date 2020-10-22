using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.ViewModels;

namespace NewsSystem.App.Components
{
    [ViewComponent(Name = "TopNews")]
    public class TopNewsViewComponent : ViewComponent
    {
        private readonly IDeletableEntityRepository<TopNews> topNewsRepository;

        public TopNewsViewComponent(IDeletableEntityRepository<TopNews> topNewsRepository)
        {
            this.topNewsRepository = topNewsRepository;
        }

        public IViewComponentResult Invoke()
        {
            var news =this.topNewsRepository.All().OrderByDescending(n=>n.Id).Select(tp=>new NewsViewModel
            {
                Id = tp.News.Id,
                ImageUrl = tp.News.ImageUrl,
                Title = tp.News.Title,
                Content = tp.News.Content,
                ContentLenght = 485
            }).ToList();

            var model = new NewsListViewModel
            {
                News = news
            };
            return  this.View(model);
        }
    }
}