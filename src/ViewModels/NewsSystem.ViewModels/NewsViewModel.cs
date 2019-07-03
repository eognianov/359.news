﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Html.Parser;
using AutoMapper;
using NewsSystem.Data.Models;
using NewsSystem.Mappings;
using NewsSystem.Services;
using Ganss.XSS;
using NewsSystem.Common;

namespace NewsSystem.ViewModels
{
     public class NewsViewModel : IMapFrom<News>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string SanitizedContent
        {
            get
            {
                // Sanitize
                var htmlSanitizer = new HtmlSanitizer();
                htmlSanitizer.AllowedCssProperties.Remove("font-size");
                htmlSanitizer.AllowedSchemes.Add("mailto");
                htmlSanitizer.AllowDataAttributes = false;
                var html = htmlSanitizer.Sanitize(this.Content);

                // Parse document
                var parser = new HtmlParser();
                var document = parser.ParseDocument(html);

                // Add .table class for tables
                var tables = document.QuerySelectorAll("table");
                foreach (var table in tables)
                {
                    table.ClassName += " table table-striped table-bordered table-hover table-sm";
                }

                // Clear font size
                var fontElements = document.QuerySelectorAll("font");
                foreach (var fontElement in fontElements)
                {
                    if (fontElement.HasAttribute("size"))
                    {
                        fontElement.RemoveAttribute("size");
                    }
                }

                return document.ToHtml();
            }
        }

        public string ShortContent
        {
            get
            {
                if (this.ContentLenght!=0)
                {
                    return this.GetShortContent(this.ContentLenght);
                }
                return this.GetShortContent(235);
            }
        }

        public string ImageUrl { get; set; }

        public string OriginalUrl { get; set; }

        public string RemoteId { get; set; }

        public string SourceName { get; set; }

        public string SourceShortName { get; set; }

        public string SourceUrl { get; set; }

        public AuthorViewModel Author { get; set; }

        public NewsSignature Signature { get; set; }

        public int ContentLenght { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string ShorterOriginalUrl
        {
            get
            {
                if (this.OriginalUrl == null)
                {
                    return string.Empty;
                }

                if (this.OriginalUrl.Length <= 65)
                {
                    return this.OriginalUrl;
                }

                return $"{this.OriginalUrl.Substring(0, 30)}..{this.OriginalUrl.Substring(this.OriginalUrl.Length - 30, 30)}";
            }
        }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnAsString =>
            this.CreatedOn.Hour == 0 && this.CreatedOn.Minute == 0
                ? this.CreatedOn.ToString("ddd, dd MMM yyyy", new CultureInfo("bg-BG"))
                : this.CreatedOn.ToString("ddd, dd MMM yyyy HH:mm", new CultureInfo("bg-BG"));

        public string Url => $"/News/{this.Id}/{new SlugGenerator().GenerateSlug(this.Title)}";

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<News, NewsViewModel>().ForMember(
                m => m.Tags,
                opt => opt.MapFrom(x => x.Tags.Select(t => t.Tag.Name)));
        }

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

        public string GetSign()
        {var sing = "";
            switch (Signature)
            {
                case NewsSignature.Nick:
                    sing = Author.UserName;
                    break;
                case NewsSignature.Team:
                    sing = GlobalConstants.SystemName;
                    break;
                case NewsSignature.Name:
                default:
                    sing = $"{Author.FirstName} {Author.LastName}";
                    break;
            }

            return sing;
        }

    }
}
