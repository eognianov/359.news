using System;
using NewsSystem.Data.Models;
using NewsSystem.Mappings;

namespace NewsSystem.ViewModels
{
    public class MainNewsViewModel:IMapFrom<MainNews>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string ImageUrlOrDefault
        {
            get
            {
                return this.ImageUrl ?? "/images/mainnews/default.png";
            }
        }

        public string OriginalUrl { get; set; }

        public string SourceName { get; set; }

        public string SourceUrl { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
