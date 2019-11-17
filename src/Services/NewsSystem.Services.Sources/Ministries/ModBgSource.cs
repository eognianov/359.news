﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AngleSharp.Dom;
using NewsSystem.Common;

namespace NewsSystem.Services.Sources.Ministries
{
    /// <summary>
    /// Министерство на отбраната.
    /// </summary>
    public class ModBgSource : BaseSource
    {
        public override string BaseUrl => "https://mod.bg/";

        protected override bool UseProxy => true;

        public override IEnumerable<RemoteNews> GetLatestPublications() => this.GetNews($"{this.BaseUrl}bg/news.php", 5);

        public override IEnumerable<RemoteNews> GetAllPublications()
        {
            for (var date = DateTime.UtcNow; date >= new DateTime(2011, 1, 1); date = date.AddMonths(-1))
            {
                var news = this.GetNews($"{this.BaseUrl}bg/news_archive.php?fn_month={date.Month}&fn_year={date.Year}");
                Console.WriteLine($"{date:yyyy, MMM} => {news.Count} news");
                foreach (var remoteNews in news)
                {
                    yield return remoteNews;
                }
            }
        }

        internal override string ExtractIdFromUrl(string url) => this.GetUrlParameterValue(url, "fn_id");

        protected override RemoteNews ParseDocument(IDocument document, string url)
        {
            // Title
            var title = document.QuerySelector(".tablelist2 h2").TextContent;
            if (title == null)
            {
                return null;
            }

            // Time
            var timeElement = document.QuerySelector(".tablelist");
            var timeAsString = timeElement?.TextContent?.Trim();
            var time = DateTime.ParseExact(timeAsString, "dd.MM.yyyy", CultureInfo.InvariantCulture);

            // Image
            var imageElement = document.QuerySelector(".tablelist2 div p a[rel^='lightbox'] img");
            var imageUrl = imageElement?.GetAttribute("src");
            if (imageUrl == null)
            {
                imageUrl = "https://res.cloudinary.com/news0722/image/upload/v1563245104/Photos/default/institucii/mod.bg.jpg";
            }
            else if (!imageUrl.Contains("/bg/"))
            {
                imageUrl = "/bg/" + imageUrl;
            }

            // Content
            var contentElement = document.QuerySelector(".tablelist2 div p");
            var images = document.QuerySelectorAll("a[rel^='lightbox']");
            foreach (var image in images)
            {
                contentElement.RemoveRecursively(image);
            }

            this.NormalizeUrlsRecursively(contentElement);
            var content = contentElement?.InnerHtml;

            return new RemoteNews(title, content, time, imageUrl);
        }

        private IList<RemoteNews> GetNews(string address, int count = 0)
        {
            var document = this.Parser.ParseDocument(this.ReadStringFromUrl(address));
            var links = document.QuerySelectorAll("#cat1 .tablelist2 a").Select(x => x?.Attributes["href"]?.Value)
                .Where(x => x?.Contains("show(") == true).Select(
                    x => $"{this.BaseUrl}bg/news.php?fn_mode=fullnews&fn_id={x.GetStringBetween("show(", ");")}").ToList();
            if (count > 0)
            {
                links = links.Take(5).ToList();
            }

            var news = links.Select(this.GetPublication).Where(x => x != null).ToList();
            return news;
        }
    }
}
