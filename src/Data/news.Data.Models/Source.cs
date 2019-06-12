using news.Data.Common.Models;
using System.Collections.Generic;

namespace news.Data.Models
{
    public class Source : BaseDeletableModel<int>
    {
        public Source()
        {
            this.News = new HashSet<News>();
        }

        public string TypeName { get; set; }

        public string ShortName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
