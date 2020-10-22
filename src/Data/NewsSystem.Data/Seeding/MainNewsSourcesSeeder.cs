using NewsSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NewsSystem.Data.Seeding
{
    public class MainNewsSourcesSeeder : ISeeder
    {
        public void Seed(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var mainNewsSources = new List<(string name, string url, string typeName)>
            {
                ("БНТ", "https://news.bnt.bg",
                    "NewsSystem.Services.Sources.MainNews.NewsBntBgMainNewsProvider"),
                ("bTV", "https://btvnovinite.bg",
                    "NewsSystem.Services.Sources.MainNews.BtvNoviniteMainNewsProvider"),
                ("Nova", "https://nova.bg",
                    "NewsSystem.Services.Sources.MainNews.NovaBgMainNewsProvider"),
                ("Dnes.bg", "https://www.dnes.bg",
                    "NewsSystem.Services.Sources.MainNews.DnesBgMainNewsProvider"),
                //("Novini.bg", "https://novini.bg",
                //    "NewsSystem.Services.Sources.MainNews.NoviniBgMainNewsProvider"),
                ("Dnevnik.bg", "https://www.dnevnik.bg",
                    "NewsSystem.Services.Sources.MainNews.DnevnikBgMainNewsProvider"),
                ("BTA.bg", "http://www.bta.bg/bg",
                    "NewsSystem.Services.Sources.MainNews.BtaBgMainNewsProvider"),
                ("Mediapool.bg", "https://www.mediapool.bg",
                    "NewsSystem.Services.Sources.MainNews.MediapoolBgMainNewsProvider"),
            };

            foreach (var mainNewsSource in mainNewsSources)
            {
                if (!dbContext.MainNewsSources.Any(x => x.TypeName == mainNewsSource.typeName))
                {
                    dbContext.MainNewsSources.Add(
                        new MainNewsSource
                        {
                            TypeName = mainNewsSource.typeName,
                            Name = mainNewsSource.name,
                            Url = mainNewsSource.url,
                        });
                }
            }
        }
    }
}