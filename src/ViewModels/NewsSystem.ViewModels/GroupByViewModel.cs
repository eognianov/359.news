using System;
using System.Collections.Generic;
using System.Text;

namespace NewsSystem.ViewModels
{
    public class GroupByViewModel<T>
    {
        public T Group { get; set; }

        public int Count { get; set; }
    }
}
