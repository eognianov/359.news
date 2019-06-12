﻿using news.Data.Common.Models;
using System.Collections.Generic;

namespace news.Data.Models
{
    public class MainNewsSource : BaseDeletableModel<int>
    {
        public MainNewsSource()
        {
            this.MainNews = new HashSet<MainNews>();
        }

        public string TypeName { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public virtual ICollection<MainNews> MainNews { get; set; }
    }
}
