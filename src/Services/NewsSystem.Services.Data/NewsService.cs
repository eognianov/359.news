using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.ViewModels;

namespace NewsSystem.Services.Data
{
    public class NewsService : INewsService
    {
        private readonly IDeletableEntityRepository<News> newsRepository;
        private readonly IHostingEnvironment environment;

        public NewsService(IDeletableEntityRepository<News> newsRepository, IHostingEnvironment environment)
        {
            this.newsRepository = newsRepository;
            this.environment = environment;
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

        public async Task<bool> AddAsync(NewsInputModel input)
        {
            
            var news = new News
            {
                Title = input.Title?.Trim(),
                ImageUrl = await  SaveImage(input.Image),
                Content = input.Content?.Trim(),
                CreatedOn = DateTime.UtcNow,
                AuthorId = input.AuthorId,
                Signature = input.Signature
            };

            news.SearchText = this.GetSearchText(news);

            await this.newsRepository.AddAsync(news);
            await this.newsRepository.SaveChangesAsync();
            return true;
        }

        public async Task UpdateAsync(int id, RemoteNews remoteNews)
        {
            var news = this.newsRepository.AllWithDeleted().FirstOrDefault(x => x.Id == id);
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

        public async Task<string> SaveImage(IFormFile file)
        {


            var uploadDir = "//media//photos//news//uploads//";

            var uploadPath = environment.WebRootPath + uploadDir;

            
                try
                {
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    using (FileStream filestream = System.IO.File.Create(uploadPath+file.FileName))
                    {
                        await file.CopyToAsync(filestream);
                        filestream.Flush();
                        return uploadDir + file.FileName;
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
        }        

    }
}
