using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Hosting;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.Services.Clodinary;
using NewsSystem.ViewModels;

namespace NewsSystem.Services.Data
{
    public class NewsService : INewsService
    {
        private readonly IDeletableEntityRepository<News> newsRepository;
        private readonly IImagesServices imagesServices;
        private readonly ICloudinaryServise cloudinaryServise;
        private readonly IHostingEnvironment environment;

        public NewsService(IDeletableEntityRepository<News> newsRepository, IImagesServices imagesServices, ICloudinaryServise cloudinaryServise)
        {
            this.newsRepository = newsRepository;
            this.imagesServices = imagesServices;
            this.cloudinaryServise = cloudinaryServise;
        }

        public async Task<bool> AddAsync(RemoteNews remoteNews, int sourceId)
        {
            if (this.newsRepository.AllWithDeleted()
                .Any(x => x.SourceId == sourceId && x.RemoteId == remoteNews.RemoteId))
            {
                // Already exists
                return false;
            }

            var news = new News
                         {
                             Title = remoteNews.Title?.Trim(),
                             OriginalUrl = remoteNews.OriginalUrl?.Trim(),
                             ImageUrl = remoteNews.ImageUrl?.Trim(),
                             Content = remoteNews.Content?.Trim(),
                             CreatedOn = remoteNews.PostDate,
                             SourceId = sourceId,
                             RemoteId = remoteNews.RemoteId?.Trim()
                         };
            news.SearchText = this.GetSearchText(news);

            await this.newsRepository.AddAsync(news);
            await this.newsRepository.SaveChangesAsync();
            return true;
        }

        public async Task<int> AddAsync(NewsInputModel input)
        {
            
            var news = new News
            {
                Title = input.Title?.Trim(),
                //ImageUrl = cloudinaryServise.Upload(input.Image.FileName, "NewsArticles").ToString(),
                ImageUrl = await  imagesServices.SaveImage(input.Image),
                Content = input.Content?.Trim(),
                CreatedOn = DateTime.UtcNow,
                AuthorId = input.AuthorId,
                Signature = input.Signature
            };

            news.SearchText = this.GetSearchText(news);

            await this.newsRepository.AddAsync(news);
            await this.newsRepository.SaveChangesAsync();
            return news.Id;
        }

        public async Task UpdateAsync(int id, RemoteNews remoteNews)
        {
            var news =await this.newsRepository.GetByIdWithDeletedAsync(id);
            if (news == null)
            {
                return;
            }

            if (remoteNews == null)
            {
                return;
            }

            news.Title = remoteNews.Title;
            news.OriginalUrl = remoteNews.OriginalUrl;
            news.ImageUrl = remoteNews.ImageUrl;
            news.Content = remoteNews.Content;
            news.RemoteId = remoteNews.RemoteId;
            news.SearchText = this.GetSearchText(news);
            //// We should not update the PostDate here

            await this.newsRepository.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(NewsUpdataInputModel input)
        {
            var originalNews =await this.newsRepository.GetByIdWithDeletedAsync(input.Id);
            if (originalNews == null)
            {
                return 0;
            }

            if (input.Title != originalNews.Title)
            {
                originalNews.Title = input.Title;
            }

            if (input.Image != null)
            {
                originalNews.ImageUrl = await imagesServices.SaveImage(input.Image);
            }

            if (input.Content!=originalNews.Content)
            {
                originalNews.Content = input.Content;
            }

            if (input.Signature!=originalNews.Signature)
            {
                originalNews.Signature = input.Signature;
            }

            await this.newsRepository.SaveChangesAsync();

            return originalNews.Id;
        }


        public int Count()
        {
            return this.newsRepository.All().Count();
        }

        public string GetSearchText(News news)
        {
            // Get only text from content
            var parser = new HtmlParser();
            var document = parser.ParseDocument($"<html><body>{news.Content}</body></html>");

            // Append title
            var text = news.Title + " " + document.Body.TextContent;

            // Split by whitespace characters and remove duplicate values as well as non-alphanumeric characters
            var regex = new Regex(@"[^\w\d ]", RegexOptions.Compiled);
            var words = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => regex.Replace(x, string.Empty)).Where(x => x.Length > 1).Distinct();

            // Combine all words
            return string.Join(" ", words);
        }

        public string GetUrlById(int id)
        {
            var title = newsRepository.GetByIdWithDeletedAsync(id).GetAwaiter().GetResult().Title;

            return $"/News/{id}/{new SlugGenerator().GenerateSlug(title)}";
        }
    }
}
