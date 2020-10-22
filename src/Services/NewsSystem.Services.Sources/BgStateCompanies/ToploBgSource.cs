﻿using System;
using System.Collections.Generic;
using System.Globalization;
using AngleSharp.Dom;

namespace NewsSystem.Services.Sources.BgStateCompanies
{
    public class ToploBgSource : BaseSource
    {
        public override string BaseUrl { get; } = "https://toplo.bg/";

        public override IEnumerable<RemoteNews> GetLatestPublications() =>
            this.GetPublications("news", ".post a");

        internal override string ExtractIdFromUrl(string url)
        {
            var uri = new Uri(url.Trim('/'));
            return uri.Segments[^4] + uri.Segments[^3] + uri.Segments[^2] + uri.Segments[^1];
        }

        protected override RemoteNews ParseDocument(IDocument document, string url)
        {
            var titleElement = document.QuerySelector(".l9 .card-title strong");
            var title = titleElement.TextContent.Trim();

            var timeElement = document.QuerySelector(".l9 .post_author_date .post_content_date");
            var timeAsString = timeElement?.TextContent?.Trim();
            var time = DateTime.ParseExact(timeAsString, "dd MMMM, yyyy", CultureInfo.GetCultureInfo("bg-BG"));

            var contentElement = document.QuerySelector(".l9 .card-content .card-content");
            this.NormalizeUrlsRecursively(contentElement);
            var content = contentElement.InnerHtml.Trim();

            var imageElement = document.QuerySelector(".l9 .card-image img.img-blog");
            var imageUrl = imageElement?.GetAttribute("src");

            return new RemoteNews(title, content, time, imageUrl);
        }
    }
}
