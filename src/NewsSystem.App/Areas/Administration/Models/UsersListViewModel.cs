using System.Collections.Generic;
using NewsSystem.Data.Models;

namespace NewsSystem.App.Areas.Administration.Models
{
    public class UsersListViewModel
    {

        public UsersListViewModel()
        {
            this.Users = new List<ApplicationUser>();
        }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
