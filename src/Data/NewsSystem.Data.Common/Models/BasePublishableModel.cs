using System;

namespace NewsSystem.Data.Common.Models
{
    public abstract class BasePublishableModel<TKey> : BaseDeletableModel<TKey>, IPublishableEntity
    {
        public bool isPublished { get; set; }
        public DateTime? PublishedOn { get; set; }
    }
}
