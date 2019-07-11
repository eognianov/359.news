using System;
using System.Collections.Generic;
using System.Text;

namespace NewsSystem.Data.Common.Models
{
    public interface IPublishableEntity
    {
        bool isPublished { get; set; }

        DateTime? PublishedOn { get; set; }
    }
}
