using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.Services.Clodinary;
using NewsSystem.ViewModels;

namespace NewsSystem.Services.Data
{
    public class NewsService : INewsService
    {
        private readonly IDeletableEntityRepository<News> newsRepository;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IImagesServices imagesServices;

        public NewsService(IDeletableEntityRepository<News> newsRepository, ICloudinaryService cloudinaryService, IImagesServices imagesServices)
        {
            this.newsRepository = newsRepository;
            this.cloudinaryService = cloudinaryService;
            this.imagesServices = imagesServices;
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
                ImageUrl = this.ResolveImageUrl(remoteNews.ImageUrl),
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
                //ImageUrl = cloudinaryService.Upload(input.Image.FileName, "NewsArticles").ToString(),
                ImageUrl = await  imagesServices.SaveImage(input.Image),
                //ImageUrl = this.ResolveImageUrl(input.Image.FileName),
                Content = input.Content?.Trim(),
                CreatedOn = DateTime.UtcNow,
                AuthorId = input.AuthorId,
                Signature = input.Signature,
                Category = input.Category
            };


            news.SearchText = this.GetSearchText(news);

            await this.newsRepository.AddAsync(news);
            await this.newsRepository.SaveChangesAsync();
            return news.Id;
        }

        public async Task UpdateAsync(int id, RemoteNews remoteNews)
        {
            var news = await this.newsRepository.GetByIdWithDeletedAsync(id);
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
            news.ImageUrl = this.ResolveImageUrl(remoteNews.ImageUrl);
            news.Content = remoteNews.Content;
            news.RemoteId = remoteNews.RemoteId;
            news.SearchText = this.GetSearchText(news);
            //// We should not update the PostDate here

            await this.newsRepository.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(NewsUpdataInputModel input)
        {
            var originalNews = await this.newsRepository.GetByIdWithDeletedAsync(input.Id);
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
                originalNews.ImageUrl =  this.ResolveImageUrl(input.ImageUrl);
            }

            if (input.Content != originalNews.Content)
            {
                originalNews.Content = input.Content;
            }

            if (input.Signature != originalNews.Signature)
            {
                originalNews.Signature = input.Signature;
            }

            if (input.Category != originalNews.Category)
            {
                originalNews.Category = input.Category;
            }

            if (input.CustomDateValue)
            {
                var inputDate = input.CustomDate.Date;
                var originalDate = originalNews.CreatedOn;

                var customDate = new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, originalDate.Hour, originalDate.Minute, originalDate.Second);

                originalNews.CreatedOn = customDate;
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
            text = text.ToLower();

            // Remove all non-alphanumeric characters
            var regex = new Regex(@"[^\w\d]", RegexOptions.Compiled);
            text = regex.Replace(text, " ");

            // Split words and remove duplicate values
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(x => x.Length > 1).Distinct();

            // Combine all words
            return string.Join(" ", words);
        }

        public string GetUrlById(int id)
        {
            var title = newsRepository.GetByIdWithDeletedAsync(id).GetAwaiter().GetResult().Title;

            return $"/News/{id}/{new SlugGenerator().GenerateSlug(title)}";
        }

        private string ResolveImageUrl(string newsImageUrl)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(newsImageUrl),
                EagerTransforms = new List<Transformation>
                {
                    
                    new Transformation().Width(884).Height(358).Gravity("auto").Crop("fill"),
                    new Transformation().Width(480).Height(195).Gravity("auto").Crop("fill"),
                    new Transformation().Width(1308).Height(567).Gravity("auto").Crop("fill")

                },
                Folder = "photos/news/",
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
    }
}
