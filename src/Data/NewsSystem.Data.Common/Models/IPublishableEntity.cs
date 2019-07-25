using System;

namespace NewsSystem.Data.Common.Models
{
    public interface IPublishableEntity
    {
        bool isPublished { get; set; }

        DateTime? PublishedOn { get; set; }
    }
}
