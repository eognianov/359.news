﻿using System;
using System.Collections.Generic;
using AngleSharp.Dom;

namespace NewsSystem.Services.Sources.Ministries
{
    /// <summary>
    /// Министерство на труда и социалната политика.
    /// </summary>
    //// TODO: Rename to MlspGovernmentBgSource
    public class MlspBgSource : BaseSource
    {
        public override string BaseUrl { get; } = "https://www.mlsp.government.bg/";

        protected override bool UseProxy => true;

        public override IEnumerable<RemoteNews> GetLatestPublications() =>
            this.GetPublications("index.php?section=PRESS", "#rub_co span a");

        public override IEnumerable<RemoteNews> GetAllPublications()
        {
            for (var date = DateTime.UtcNow; date >= new DateTime(2014, 11, 1); date = date.AddDays(-1))
            {
                var news = this.GetPublications(
                    $"index.php?section=PRESS2&vs=2&pr=3&lang=&dtT={date:dd}.{date:MM}.{date:yyyy}",
                    "#statiata span a");
                Console.WriteLine($"Date {date.ToShortDateString()} => {news.Count} news");
                foreach (var remoteNews in news)
                {
                    remoteNews.PostDate = date.Date;
                    yield return remoteNews;
                }
            }
        }

        internal override string ExtractIdFromUrl(string url) => this.GetUrlParameterValue(url, "prid");

        protected override RemoteNews ParseDocument(IDocument document, string url)
        {
            var titleElement = document.QuerySelector(".grid_2_3_long_bless h3");
            var title = titleElement?.TextContent;

            // No time provided in the news page
            var time = DateTime.Now;

            var imageElement = document.QuerySelector("#statiata img");
            var imageUrl = imageElement?.GetAttribute("src") ?? "https://www.mlsp.government.bg/server/php/files/mtsp_rsz%20(1).jpg";

            var contentElement = document.QuerySelector("#statiata");
            contentElement.RemoveRecursively(imageElement);
            this.NormalizeUrlsRecursively(contentElement);
            var content = contentElement?.InnerHtml;

            return new RemoteNews(title, content, time, imageUrl);
        }
    }
}
