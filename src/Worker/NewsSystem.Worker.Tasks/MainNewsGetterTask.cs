namespace NewsSystem.Worker.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.Extensions.Logging;

    using NewsSystem.Common;
    using NewsSystem.Data.Common.Repositories;
    using NewsSystem.Data.Models;
    using NewsSystem.Services;
    using NewsSystem.Services.Clodinary;
    using NewsSystem.Services.Sources.MainNews;
    using NewsSystem.Worker.Common;

    public class MainNewsGetterTask : BaseTask<MainNewsGetterTask.Input, MainNewsGetterTask.Output>
    {
        private readonly IDeletableEntityRepository<MainNewsSource> mainNewsSourcesRepository;

        private readonly IDeletableEntityRepository<MainNews> mainNewsRepository;
        private readonly ICloudinaryService cloudinaryService;
        private readonly ILogger logger;

        public MainNewsGetterTask(
            IDeletableEntityRepository<MainNewsSource> mainNewsSourcesRepository,
            IDeletableEntityRepository<MainNews> mainNewsRepository,
            ILoggerFactory loggerFactory, ICloudinaryService cloudinaryService)
        {
            this.mainNewsSourcesRepository = mainNewsSourcesRepository;
            this.mainNewsRepository = mainNewsRepository;
            this.cloudinaryService = cloudinaryService;
            this.logger = loggerFactory.CreateLogger<MainNewsGetterTask>();
        }

        protected override async Task<Output> DoWork(Input input)
        {
            var updated = 0;
            string errors = null;
            foreach (var source in this.mainNewsSourcesRepository.All().ToList())
            {
                var lastNews =
                    this.mainNewsRepository.All().Where(x => x.SourceId == source.Id)
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefault();
                var instance = ReflectionHelpers.GetInstance<BaseMainNewsProvider>(source.TypeName);

                RemoteMainNews news = null;
                try
                {
                    news = instance.GetMainNews();
                }
                catch (Exception e)
                {
                    errors += $"Error in {source.TypeName}: {e.Message}; ";
                }

                if (news == null)
                {
                    errors += $"Null news in {source.TypeName}; ";
                    continue;
                }

                if (lastNews?.Title == news.Title)
                {
                    // The last news has the same title
                    this.logger.LogInformation($"Getting main news from {source.Name}. Nothing new.");
                    continue;
                }

                updated++;
                await this.mainNewsRepository.AddAsync(
                    new MainNews
                    {
                        Title = news.Title,
                        OriginalUrl = news.OriginalUrl,
                        ImageUrl = this.ResolveImageUrl(news.ImageUrl),
                        SourceId = source.Id,
                    });
                this.logger.LogInformation($"Getting main news from {source.Name}. New item added.");
            }

            await this.mainNewsRepository.SaveChangesAsync();
            return new Output { Updated = updated, Error = errors, Ok = errors == null };
        }

        protected override WorkerTask Recreate(WorkerTask currentTask, Input currentParameters, Output currentResult) =>
            new WorkerTask(currentTask, DateTime.UtcNow.AddSeconds(60));

        private string ResolveImageUrl(string newsImageUrl)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(newsImageUrl),
                Transformation = new Transformation().Width(408).Height(204).Gravity("auto").Crop("fill"),
                Folder = "photos/mainNews/",
                UniqueFilename = true,
                AccessMode = "public",
            };
            var result = this.cloudinaryService.Upload(uploadParams);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return result.SecureUri.ToString();
            }

            return newsImageUrl;
        }

        public class Input : BaseTaskInput
        {
        }

        public class Output : BaseTaskOutput
        {
            public int Updated { get; set; }
        }
    }
}