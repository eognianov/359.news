using System.Net.Http;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using NewsSystem.Common;

namespace NewsSystem.Services.Sources.MainNews
{
    public abstract class BaseMainNewsProvider
    {
        public abstract string BaseUrl { get; }

        public abstract RemoteMainNews GetMainNews();

        public IDocument GetDocument(string url)
        {
            var parser = new HtmlParser();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", GlobalConstants.DefaultUserAgent);
            var html = httpClient.GetStringAsync(url).GetAwaiter().GetResult();
            var document = parser.ParseDocument(html);
            return document;
        }
    }
}
