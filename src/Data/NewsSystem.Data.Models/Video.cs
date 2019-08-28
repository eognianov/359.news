using System;
using System.Collections.Generic;
using System.Text;
using NewsSystem.Data.Common.Models;

namespace NewsSystem.Data.Models
{
    public class Video:BaseModel<int>
    {
        public Video()
        {
            
        }
        public string Url { get; set; }

        public int NewsId { get; set; }

        public virtual News News  { get; set; }

        public string EmbededUrl()
        {
            var embededUrl = Url.Replace("watch?v=", "embed/");
            return embededUrl;
        }

    }
}
