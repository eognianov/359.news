﻿using System;
using System.Collections.Generic;
using System.Globalization;
using AngleSharp.Dom;

namespace NewsSystem.Services.Sources.BgInstitutions
{
    public class GovernmentBgSource : BaseSource
    {
        public override string BaseUrl { get; } = "http://www.government.bg/";

        protected override bool UseProxy => true;

        public override IEnumerable<RemoteNews> GetLatestPublications() =>
            this.GetPublications("bg/prestsentar/novini", ".articles .item a", count: 8);

        public override IEnumerable<RemoteNews> GetAllPublications()
        {
            for (var i = 1; i <= 60; i++)
            {
                var news = this.GetPublications($"bg/prestsentar/novini?page={i}", ".articles .item a");
                Console.WriteLine($"Page {i} => {news.Count} news");
                foreach (var remoteNews in news)
                {
                    yield return remoteNews;
                }
            }
        }

        protected override RemoteNews ParseDocument(IDocument document, string url)
        {
            var titleElement = document.QuerySelector(".view h1");
            var title = titleElement.TextContent.Trim();

            var time = DateTime.Now;

            var imageElement = document.QuerySelector(".view .gallery img");
            var imageUrl = imageElement?.GetAttribute("src") ?? "https://res.cloudinary.com/news0722/image/upload/v1563245104/Photos/default/institucii/government.bg.png";

            var contentElement = document.QuerySelector(".view");
            contentElement.RemoveRecursively(titleElement);
            ////contentElement.RemoveRecursively(timeElement);
            contentElement.RemoveRecursively(document.QuerySelector(".view .gallery"));
            this.NormalizeUrlsRecursively(contentElement);
            var content = contentElement.InnerHtml.Trim();

            return new RemoteNews(title, content, time, imageUrl);
        }
    }
}
