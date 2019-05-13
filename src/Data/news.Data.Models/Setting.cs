using System;
using System.Collections.Generic;
using System.Text;

namespace news.Data.Models
{
    using news.Data.Common.Models;

    public class Setting : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
