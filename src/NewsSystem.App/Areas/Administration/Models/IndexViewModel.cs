namespace NewsSystem.Web.Areas.Administration.Models
{
    using System.Collections.Generic;

    using NewsSystem.Data.Models;

    public class IndexViewModel
    {
        public int UsersCount { get; set; }

        public int CountNullNewsImageUrls { get; set; }

        public int CountNullNewsOriginalUrl { get; set; }

        public int CountNullNewsRemoteId { get; set; }

        public int NotProcessedTaskCount { get; set; }

        public int ProcessedTaskCount { get; set; }

        public IEnumerable<WorkerTask> LastWorkerTaskErrors { get; set; }

        public IEnumerable<WorkerTask> ProcessingWorkerTasks { get; set; }
    }
}
