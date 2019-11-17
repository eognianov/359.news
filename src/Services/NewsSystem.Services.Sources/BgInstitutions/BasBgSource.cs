﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using AngleSharp.Dom;

namespace NewsSystem.Services.Sources.BgInstitutions
{
    public class BasBgSource : BaseSource
    {
        public override string BaseUrl { get; } = "http://www.bas.bg/";

        public override IEnumerable<RemoteNews> GetLatestPublications() =>
            this.GetPublications("академични-новини", ".fusion-recent-posts article.post h4 a", count: 5);

        public override IEnumerable<RemoteNews> GetAllPublications()
        {
            for (var page = 1; page <= 36; page++)
            {
                var news = this.GetPublications(
                    $"академични-новини/page/{page}",
                    ".fusion-recent-posts article.post h4 a");
                Console.WriteLine($"Page {page} => {news.Count} news");
                foreach (var remoteNews in news)
                {
                    yield return remoteNews;
                }
            }
        }

        internal override string ExtractIdFromUrl(string url)
        {
            var uri = new Uri(url.Trim().Trim('/'));
            return WebUtility.UrlDecode(
                uri.Segments[uri.Segments.Length - 4] + uri.Segments[uri.Segments.Length - 3]
                                                      + uri.Segments[uri.Segments.Length - 2]
                                                      + uri.Segments[uri.Segments.Length - 1]);
        }

        protected override RemoteNews ParseDocument(IDocument document, string url)
        {
            var titleElement = document.QuerySelector("h1.fusion-post-title");
            if (titleElement == null)
            {
                return null;
            }

            var title = titleElement.TextContent.Trim();

            var timeElement = document.QuerySelector(".updated.rich-snippet-hidden").NextElementSibling;
            var timeAsString = timeElement?.TextContent?.Trim();
            if (string.IsNullOrWhiteSpace(timeAsString))
            {
                return null;
            }

            var time = DateTime.ParseExact(timeAsString, "dddd, d MMMM yyyy", new CultureInfo("bg-BG"));

            var imageElement = document.QuerySelector(".post-content img");
            var imageUrl = imageElement?.GetAttribute("src") ?? "https://res.cloudinary.com/news0722/image/upload/v1563245104/Photos/default/institucii/bas.bg.png";

            var contentElement = document.QuerySelector(".post-content");
            contentElement.RemoveRecursively(imageElement);
            this.NormalizeUrlsRecursively(contentElement);
            var content = contentElement?.InnerHtml;

            return new RemoteNews(title, content, time, imageUrl);
        }
    }
}
