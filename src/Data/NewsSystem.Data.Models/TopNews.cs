using NewsSystem.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsSystem.Data.Models
{
    public class TopNews:BaseDeletableModel<int>
    {
        public int NewsId { get; set; }
        public virtual News News { get; set; }
    }
}
