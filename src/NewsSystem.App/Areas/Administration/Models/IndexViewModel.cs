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

    }
}
