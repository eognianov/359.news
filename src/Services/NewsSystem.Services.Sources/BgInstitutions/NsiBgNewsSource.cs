using System.Collections.Generic;
using AngleSharp.Dom;

namespace NewsSystem.Services.Sources.BgInstitutions
{
    public class NsiBgNewsSource : NsiBgBaseSource
    {
        public override IEnumerable<RemoteNews> GetLatestPublications() =>
            this.GetPublications("bg/events_list", ".view-content .views-field-title a", count: 5);

        protected override string GetContent(IHtmlCollection<IElement> imageAndContent)
        {
            return imageAndContent.Length == 1 ? string.Empty : imageAndContent[1].InnerHtml;
        }
    }
}
