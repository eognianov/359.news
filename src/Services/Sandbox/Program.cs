using Microsoft.EntityFrameworkCore;
using System;
using NewsSystem.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=EMKATA-ACER;Database=PressCenters;Trusted_Connection=True;MultipleActiveResultSets=true");

            var db = new ApplicationDbContext(optionsBuilder.Options);


            string link =
                "https://res.cloudinary.com/news0722/image/upload/v1562179725/190702141811-von-der-leyen-lagarde-split-large-tease_zdsyit.jpg";
            int index = link.LastIndexOf('/');

            Console.WriteLine(link.Substring(index));


        }
    }
}

