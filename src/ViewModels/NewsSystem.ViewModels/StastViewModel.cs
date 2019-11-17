using System;
using System.Collections.Generic;
using System.Text;

namespace NewsSystem.ViewModels
{
    
    using System;
    using System.Collections.Generic;
    
    public class StastsViewModel
    {
        public IEnumerable<GroupByViewModel<DayOfWeek>> NewsByDayOfWeek { get; set; }

        public IEnumerable<GroupByViewModel<int>> NewsByMonth { get; set; }

        public IEnumerable<GroupByViewModel<int>> NewsByYear { get; set; }

        public int NewsCount { get; set; }

        public int NewsToday { get; set; }

        public int NewsYesterday { get; set; }

        public int NewsTheDayBeforeYesterday { get; set; }

        public int SourcesCount { get; set; }

        public MouthStatNormalise MouthStatNormalise { get; set; }

    }
}
