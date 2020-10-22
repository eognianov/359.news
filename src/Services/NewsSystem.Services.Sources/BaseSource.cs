﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using NewsSystem.Common;

namespace NewsSystem.Services.Sources
{
    public abstract class BaseSource : ISource
    {
        protected BaseSource()
        {
            this.Parser = new HtmlParser();
        }

        public abstract string BaseUrl { get; }

        protected virtual bool UseProxy => false;

        protected virtual Encoding Encoding => null;

        protected virtual List<(HttpRequestHeader header, string value)> Headers => null;

        protected HtmlParser Parser { get; }

        public abstract IEnumerable<RemoteNews> GetLatestPublications();

        public virtual IEnumerable<RemoteNews> GetAllPublications()
        {
            return new List<RemoteNews>();
        }

        public RemoteNews GetPublication(string url)
        {
            IHtmlDocument document;
            try
            {
                document = this.Parser.ParseDocument(this.ReadStringFromUrl(url));
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    switch ((e.Response as HttpWebResponse)?.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                        case HttpStatusCode.InternalServerError:
                            return null;
                    }
                }

                throw;
            }

            var publication = this.ParseDocument(document, url);
            if (publication == null)
            {
                return null;
            }

            // Title
            publication.Title = publication.Title?.Trim();

            // Content
            publication.Content = publication.Content?.Trim();

            // Post date
            if (publication.PostDate > DateTime.Now)
            {
                publication.PostDate = DateTime.Now;
            }

            if (publication.PostDate.Date == DateTime.UtcNow.Date && publication.PostDate.Hour == 0
                                                                  && publication.PostDate.Minute == 0)
            {
                publication.PostDate = DateTime.Now;
            }

            // Original URL
            publication.OriginalUrl = url.Trim();

            // Image URL
            publication.ImageUrl = publication.ImageUrl?.Trim();
            if (publication.ImageUrl?.StartsWith("https://res.cloudinary.com/news0722/image/upload/v1563245104/Photos/default/institucii/") == false)
            {
                publication.ImageUrl = this.NormalizeUrl(publication.ImageUrl)?.Trim();
            }

            // Remote ID
            publication.RemoteId = this.ExtractIdFromUrl(url)?.Trim();

            return publication;
        }

        internal virtual string ExtractIdFromUrl(string url)
        {
            var uri = new Uri(url.Trim().Trim('/'));
            var lastSegment = uri.Segments[uri.Segments.Length - 1];
            return WebUtility.UrlDecode(lastSegment);
        }

        protected abstract RemoteNews ParseDocument(IDocument document, string url);

        protected IList<RemoteNews> GetPublications(string address, string anchorSelector, string urlShouldContain = "", int count = 0, bool throwOnEmpty = true)
        {
            var document = this.Parser.ParseDocument(this.ReadStringFromUrl($"{this.BaseUrl}{address}"));
            var links = document.QuerySelectorAll(anchorSelector)
                .Select(x => this.NormalizeUrl(x?.Attributes["href"]?.Value))
                .Where(x => x?.Contains(urlShouldContain) == true).Distinct().ToList();

            if (count > 0)
            {
                links = links.Take(count).ToList();
            }

            if (!links.Any() && throwOnEmpty)
            {
                throw new Exception("No publications found.");
            }

            var news = links.Select(this.GetPublication).Where(x => x != null).ToList();
            return news;
        }

        protected string GetUrlParameterValue(string url, string parameterName)
        {
            var matches = Regex.Matches(url, @"[\?&](([^&=]+)=([^&=#]*))", RegexOptions.Compiled);
            var parameters = matches.Cast<Match>().ToDictionary(
                m => Uri.UnescapeDataString(m.Groups[2].Value),
                m => Uri.UnescapeDataString(m.Groups[3].Value));
            return parameters[parameterName];
        }

        protected string ReadStringFromUrl(string url)
        {
            url = new Uri(url).GetLeftPart(UriPartial.Query); // Remove hash fragment
            if (this.UseProxy)
            {
                url = url.Replace("https://", "https://proxy.presscenters.com/_plain/https/")
                         .Replace("http://", "https://proxy.presscenters.com/_plain/http/");
            }

            var webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.UserAgent, GlobalConstants.DefaultUserAgent);
            if (this.Headers != null)
            {
                foreach (var (header, value) in this.Headers)
                {
                    webClient.Headers.Add(header, value);
                }
            }

            if (this.Encoding != null)
            {
                webClient.Encoding = this.Encoding;
            }

            var html = webClient.DownloadString(url);
            return html;
        }

        // TODO: Normalize using current url as base url instead of this.BaseUrl?
        protected string NormalizeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            if (!Uri.TryCreate(new Uri(this.BaseUrl), url, out var result))
            {
                return url;
            }

            return result.ToString();
        }

        protected void NormalizeUrlsRecursively(IElement element)
        {
            if (element == null)
            {
                return;
            }

            if (element.Attributes["href"] != null)
            {
                element.SetAttribute("href", this.NormalizeUrl(element.Attributes["href"].Value));
            }

            if (element.Attributes["src"] != null)
            {
                element.SetAttribute("src", this.NormalizeUrl(element.Attributes["src"].Value));
            }

            foreach (var node in element.Children)
            {
                this.NormalizeUrlsRecursively(node);
            }
        }
    }
}
