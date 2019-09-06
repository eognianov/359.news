using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Ganss.XSS;
using NewsSystem.Common;
using NewsSystem.Data.Models;
using NewsSystem.Mappings;
using NewsSystem.Services;

namespace NewsSystem.ViewModels
{
    public class NewsToFacebookViewMode:IMapFrom<News>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url => $"{GlobalConstants.TempUrl}/News/{this.Id}/{new SlugGenerator().GenerateSlug(this.Title)}";

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string ShortContent => this.GetShortContent(250);

        public string GetShortContent(int maxLength)
        {
            // TODO: Extract as a service
            var htmlSanitizer = new HtmlSanitizer();
            var html = htmlSanitizer.Sanitize(this.Content);
            var strippedContent = WebUtility.HtmlDecode(html?.StripHtml() ?? string.Empty);
            strippedContent = strippedContent.Replace("\n", " ");
            strippedContent = strippedContent.Replace("\t", " ");
            strippedContent = Regex.Replace(strippedContent, @"\s+", " ").Trim();
            return strippedContent.Length <= maxLength ? strippedContent : strippedContent.Substring(0, maxLength) + "...";
        }
    }
}
