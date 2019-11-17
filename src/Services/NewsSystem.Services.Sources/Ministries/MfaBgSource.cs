﻿using System;
using System.Collections.Generic;
using System.Globalization;
using AngleSharp.Dom;

namespace NewsSystem.Services.Sources.Ministries
{
    /// <summary>
    /// Министерство на външните работи.
    /// </summary>
    public class MfaBgSource : BaseSource
    {
        public override string BaseUrl { get; } = "https://www.mfa.bg/";

        public override IEnumerable<RemoteNews> GetLatestPublications() =>
            this.GetPublications("bg/news", ".main-news .news-item h2 a", "bg/news", count: 5);

        public override IEnumerable<RemoteNews> GetAllPublications()
        {
            for (var i = 1; i <= 300; i++)
            {
                var news = this.GetPublications($"bg/news?p={i}", ".main-news .news-item h2 a", "bg/news");
                Console.WriteLine($"Page {i} => {news.Count} news");
                foreach (var remoteNews in news)
                {
                    yield return remoteNews;
                }
            }
        }

        protected override RemoteNews ParseDocument(IDocument document, string url)
        {
            var titleElement = document.QuerySelector("h1.news-title");
            if (titleElement == null)
            {
                return null;
            }

            var title = titleElement.TextContent;

            var timeElement = document.QuerySelector(".news-item .date");
            var timeAsString = timeElement?.TextContent?.Trim();
            var time = DateTime.ParseExact(timeAsString, "dd MMMM yyyy", new CultureInfo("bg-BG"));

            var imageElement = document.QuerySelector(".news-item img.main-pic");
            var imageUrl = imageElement?.GetAttribute("src") ?? "https://res.cloudinary.com/news0722/image/upload/v1563245104/Photos/default/institucii/mfa.bg.png";

            var contentElement = document.QuerySelector(".news-item .content");
            this.NormalizeUrlsRecursively(contentElement);
            var content = contentElement?.InnerHtml;

            return new RemoteNews(title, content, time, imageUrl);
        }
    }
}
