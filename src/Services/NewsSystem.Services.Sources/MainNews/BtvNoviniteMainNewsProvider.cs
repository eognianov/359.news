﻿namespace NewsSystem.Services.Sources.MainNews
{
    public class BtvNoviniteMainNewsProvider : BaseMainNewsProvider
    {
        public override string BaseUrl { get; } = "https://btvnovinite.bg";

        public override RemoteMainNews GetMainNews()
        {
            var document = this.GetDocument(this.BaseUrl);

            var titleElement = document.QuerySelector(".leading-articles .item .title");
            var title = titleElement.TextContent.Trim();

            var urlElement = document.QuerySelector(".leading-articles .item .link");
            var url = this.BaseUrl + urlElement.Attributes["href"].Value.Trim();

            var imageElement = document.QuerySelector(".leading-articles .item .image img");
            var imageUrl = "https:" + imageElement?.Attributes["src"]?.Value?.Trim();

            return new RemoteMainNews(title, url, imageUrl);
        }
    }
}
