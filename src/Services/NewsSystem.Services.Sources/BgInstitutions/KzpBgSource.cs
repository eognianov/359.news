using System;
using System.Collections.Generic;
using System.Globalization;
using AngleSharp.Dom;

namespace NewsSystem.Services.Sources.BgInstitutions
{
    /// <summary>
    /// Комисия за защита на потребителите.
    /// </summary>
    public class KzpBgSource : BaseSource
    {
        public override string BaseUrl { get; } = "https://kzp.bg/";

        protected override bool UseProxy => true;

        public override IEnumerable<RemoteNews> GetLatestPublications() =>
            this.GetPublications("novini/1", ".blog-list li h3 a", count: 5);

        public override IEnumerable<RemoteNews> GetAllPublications()
        {
            for (var page = 1; page <= 145; page++)
            {
                var news = this.GetPublications($"novini/{page}", ".blog-list li h3 a");
                Console.WriteLine($"Page {page} => {news.Count} news");
                foreach (var remoteNews in news)
                {
                    yield return remoteNews;
                }
            }
        }

        protected override RemoteNews ParseDocument(IDocument document, string url)
        {
            var titleElement = document.QuerySelector("h1.page-inner-title");
            if (titleElement == null)
            {
                return null;
            }

            var title = titleElement.TextContent.Trim();

            var timeElement = document.QuerySelector(".blog-post .date");
            var timeAsString = timeElement?.TextContent?.Trim();
            if (string.IsNullOrWhiteSpace(timeAsString))
            {
                return null;
            }

            var time = DateTime.ParseExact(timeAsString, "dd.MM.yyyy", CultureInfo.InvariantCulture);

            var imageElement = document.QuerySelector(".blog-post img");
            var imageUrl = imageElement?.GetAttribute("src") ?? "https://res.cloudinary.com/news0722/image/upload/v1563245104/Photos/default/institucii/kzp.bg.jpg";

            var contentElement = document.QuerySelector(".blog-post article");
            this.NormalizeUrlsRecursively(contentElement);
            var content = contentElement?.InnerHtml;

            return new RemoteNews(title, content, time, imageUrl);
        }
    }
}
