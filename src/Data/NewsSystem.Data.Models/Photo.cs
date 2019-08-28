using NewsSystem.Data.Common.Models;

namespace NewsSystem.Data.Models
{
    public class Photo:BaseModel<string> 
    {
        public string Url { get; set; }

        public int NewsId { get; set; }

        public virtual News News  { get; set; }

    }
}
