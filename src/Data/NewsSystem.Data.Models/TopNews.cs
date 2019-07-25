using NewsSystem.Data.Common.Models;

namespace NewsSystem.Data.Models
{
    public class TopNews:BaseDeletableModel<int>
    {
        public int NewsId { get; set; }
        public virtual News News { get; set; }
    }
}
