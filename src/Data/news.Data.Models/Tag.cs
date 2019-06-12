using news.Data.Common.Models;
using System.Collections.Generic;

namespace news.Data.Models
{
    public class Tag : BaseModel<int>
    {
        public Tag()
        {
            this.News = new HashSet<NewsTag>();
        }

        public string Name { get; set; }

        public virtual ICollection<NewsTag> News { get; set; }
    }
}
