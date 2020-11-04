namespace NewsSystem.App.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using NewsSystem.Data.Common;
    using NewsSystem.Data.Common.Repositories;
    using NewsSystem.Data.Models;
    using NewsSystem.Web.Areas.Administration.Models;


    public class DashboardController : AdministrationController
    {
        private readonly IDeletableEntityRepository<News> newsRepository;

        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        private readonly IDbQueryRunner queryRunner;

        public DashboardController
            (
            IDeletableEntityRepository<News> newsRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDbQueryRunner queryRunner)
        {
            this.newsRepository = newsRepository;
            this.usersRepository = usersRepository;
            this.queryRunner = queryRunner;
        }

        [Route("/dash")]
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                UsersCount = this.usersRepository.All().Count(),
                CountNullNewsImageUrls = this.newsRepository.All().Count(x => string.IsNullOrWhiteSpace(x.ImageUrl)),
                CountNullNewsOriginalUrl = this.newsRepository.All().Count(x => string.IsNullOrWhiteSpace(x.OriginalUrl)),
                CountNullNewsRemoteId = this.newsRepository.All().Count(x => string.IsNullOrWhiteSpace(x.RemoteId)),
            };
            return this.View(viewModel);
        }
    }

}
