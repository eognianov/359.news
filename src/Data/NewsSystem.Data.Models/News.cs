using NewsSystem.Data.Common.Models;
using System.Collections.Generic;
using NewsSystem.Common;
using System;

namespace NewsSystem.Data.Models
{
    public class News : BaseDeletableModel<int>, IPublishableEntity
    {
        public News()
        {
            this.Tags = new HashSet<NewsTag>();
            this.Photos = new List<Photo>();
        }

        public string Title { get; set; }

        public string Content { get; set; }


        public string ImageUrl { get; set; }

        public string OriginalUrl { get; set; }

        public int? SourceId { get; set; }

        public virtual Source Source { get; set; }

        public string RemoteId { get; set; }

        public string SearchText { get; set; }

        public virtual ICollection<NewsTag> Tags { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual NewsSignature Signature { get; set; }
        public virtual NewsCategory Category { get; set; }

        public bool isPublished { get; set; }
        public DateTime? PublishedOn { get; set; }

    }
}
