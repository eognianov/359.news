using System.Collections.Generic;

namespace NewsSystem.ViewModels
{
    public class TopNewsComponentViewModel
    {
        public IEnumerable<NewsViewModel> News { get; set; }
    }
}
