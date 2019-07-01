using NewsSystem.Data.Common.Models;

namespace NewsSystem.Data.Models
{
    public class NewsTag : BaseDeletableModel<int>
    {
        public int NewsId { get; set; }

        public virtual News News { get; set; }

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
