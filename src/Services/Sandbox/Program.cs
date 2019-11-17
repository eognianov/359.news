using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsSystem.Services.Clodinary;
using Microsoft.Extensions.Configuration;
using NewsSystem.Data;
using System.IO;
using System.Linq;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using NewsSystem.Common;
using NewsSystem.Data.Models;
using Video = NewsSystem.Data.Models.Video;
using System.Net;

namespace Sandbox
{
    class Program
    {
        public static void Main(string[] args)
        
        {
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var mssqlOptBuild = new DbContextOptionsBuilder<ApplicationDbContext>();
            mssqlOptBuild.UseSqlServer(configuration.GetConnectionString("Development"));

            var mssqlContetx = new ApplicationDbContext(mssqlOptBuild.Options);

            var pgSqlBidl = new DbContextOptionsBuilder<ApplicationDbContext>();
            pgSqlBidl.UseNpgsql(configuration.GetConnectionString("PostgreSQL"));
            
            var postgresContext = new ApplicationDbContext(pgSqlBidl.Options);

            var mssqlNews = mssqlContetx.News.ToList();

            var news = postgresContext.News.ToList();

            int count = 1;

            //skip 1128 -----------

            foreach (var n in mssqlNews)
            {
                Console.WriteLine("Obratova novina nomer  " + count);

                var newNews = new News
                {
                    Title = n.Title,
                    Category = NewsCategory.България,
                    Content = n.Content,
                    ModifiedOn = n.ModifiedOn,
                    ImageUrl = ResolveImageUrl(n.ImageUrl),
                    OriginalUrl = n.OriginalUrl,
                    Photos = new List<Photo>(),
                    RemoteId = n.RemoteId,
                    isPublished = true,
                    PublishedOn = n.CreatedOn,
                    SearchText = n.SearchText,
                    SourceId = n.SourceId,
                    Tags = new List<NewsTag>(),
                    Videos = new List<Video>()
                };

                Console.WriteLine(newNews.ImageUrl);
                postgresContext.Add(newNews);

                count++;
                if (count==25)
                {
                    postgresContext.SaveChanges();
                    count = 1;
                }

            }

        }

        private static string ResolveImageUrl(string newsImageUrl)
        {
            if (newsImageUrl.StartsWith("/images"))
            {
                return newsImageUrl;
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(newsImageUrl),
                EagerTransforms = new List<Transformation>
                {
                    new Transformation().Width(931).Height(378).Gravity("auto").Crop("fill"),
                    new Transformation().Width(846).Height(343).Gravity("auto").Crop("fill"),
                    new Transformation().Width(1086).Height(440).Gravity("auto").Crop("fill"),
                    new Transformation().Width(884).Height(358).Gravity("auto").Crop("fill"),
                    new Transformation().Width(688).Height(278).Gravity("auto").Crop("fill"),
                    new Transformation().Width(480).Height(195).Gravity("auto").Crop("fill"),
                    new Transformation().Width(1308).Height(567).Gravity("auto").Crop("fill"),
                    new Transformation().Width(1246).Height(505).Gravity("auto").Crop("fill"),

                },
                Folder = "photos/news/",
                UniqueFilename = true,
                AccessMode = "public",
            };

            var cloudinaryService = new CloudinaryService();

            var result = cloudinaryService.Upload(uploadParams);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return result.SecureUri.ToString();
            }

            return newsImageUrl;
        }
        
    }
}
