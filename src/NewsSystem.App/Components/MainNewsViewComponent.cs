using Microsoft.AspNetCore.Mvc;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.ViewModels;
using System.Linq;
using NewsSystem.Mappings;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NewsSystem.App.Components
{
    [ViewComponent(Name="MainNews")]
    public class MainNewsViewComponent:ViewComponent
    {
        private readonly IDeletableEntityRepository<MainNews> mainNewsRepository;
        private readonly IDeletableEntityRepository<MainNewsSource> mainNewsSourcesRepository;


        public MainNewsViewComponent(IDeletableEntityRepository<MainNews> mainNewsRepository,
            IDeletableEntityRepository<MainNewsSource> mainNewsSourcesRepository)
        {
            this.mainNewsRepository = mainNewsRepository;
            this.mainNewsSourcesRepository = mainNewsSourcesRepository;
        }

        public IViewComponentResult Invoke()
        {
            var news = this.mainNewsSourcesRepository.All()
                .Select(x => x.MainNews.OrderByDescending(x => x.Id).FirstOrDefault())
                .OrderByDescending(x => x.CreatedOn).To<MainNewsViewModel>().ToList();

            var viewModel = new MainNewsComponentViewModel { MainNews = news };
            return  this.View(viewModel);
        }
    }
}
