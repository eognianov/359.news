﻿using NewsSystem.Data.Common.Models;

namespace NewsSystem.Data.Models
{
    public class MainNews : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string OriginalUrl { get; set; }

        public int SourceId { get; set; }

        public virtual MainNewsSource Source { get; set; }
    }
}
