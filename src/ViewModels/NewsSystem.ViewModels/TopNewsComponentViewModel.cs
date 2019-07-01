using System;
using System.Collections.Generic;
using System.Text;

namespace NewsSystem.ViewModels
{
    public class TopNewsComponentViewModel
    {
        public IEnumerable<NewsViewModel> News { get; set; }
    }
}
