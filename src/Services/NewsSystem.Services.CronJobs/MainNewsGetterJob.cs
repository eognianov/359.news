using NewsSystem.Common;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.Services.Sources.MainNews;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace NewsSystem.Services.CronJobs
{
    public class MainNewsGetterJob
    {
        private readonly IDeletableEntityRepository<MainNewsSource> mainNewsSourcesRepository;

        private readonly IDeletableEntityRepository<MainNews> mainNewsRepository;

        public MainNewsGetterJob(
            IDeletableEntityRepository<MainNewsSource> mainNewsSourcesRepository,
            IDeletableEntityRepository<MainNews> mainNewsRepository)
        {
            this.mainNewsSourcesRepository = mainNewsSourcesRepository;
            this.mainNewsRepository = mainNewsRepository;
        }

        public async Task Work()
        {
            string errors = null;
            foreach (var source in this.mainNewsSourcesRepository.All().ToList())
            {
                var lastNews =
                    this.mainNewsRepository.All().Where(x => x.SourceId == source.Id)
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefault();
                var instance = ReflectionHelpers.GetInstance<BaseMainNewsProvider>(source.TypeName);

                RemoteMainNews news;
                try
                {
                    news = instance.GetMainNews();
                }
                catch (Exception e)
                {
                    errors += $"Error in {source.TypeName}: {e.Message}; ";
                    continue;
                }

                if (news == null)
                {
                    errors += $"Null news in {source.TypeName}; ";
                    continue;
                }

                if (lastNews?.Title == news.Title && lastNews?.ImageUrl == news.ImageUrl)
                {
                    // The last news has the same title
                    continue;
                }

                await this.mainNewsRepository.AddAsync(
                    new MainNews
                    {
                        Title = news.Title,
                        OriginalUrl = news.OriginalUrl,
                        ImageUrl = news.ImageUrl,
                        SourceId = source.Id,
                    });
            }

            await this.mainNewsRepository.SaveChangesAsync();
            if (!string.IsNullOrWhiteSpace(errors))
            {
                throw new Exception(errors);
            }
        }
    }
}
