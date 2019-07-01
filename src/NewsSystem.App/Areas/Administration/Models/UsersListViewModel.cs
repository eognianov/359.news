using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
