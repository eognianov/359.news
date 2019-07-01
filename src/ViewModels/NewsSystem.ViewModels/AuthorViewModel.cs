using System;
using System.Collections.Generic;
using System.Text;
using NewsSystem.Data.Models;
using NewsSystem.Mappings;

namespace NewsSystem.ViewModels
{
    public class AuthorViewModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
