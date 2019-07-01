using System;
using System.Collections.Generic;
using System.Text;

namespace NewsSystem.ViewModels
{
    public class MainNewsComponentViewModel
    {
        public IEnumerable<MainNewsViewModel> MainNews { get; set; }
    }
}
