using Microsoft.EntityFrameworkCore;
using NewsSystem.Data;
using Microsoft.Extensions.DependencyInjection;
using NewsSystem.Services.Clodinary;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox
{
    class Program
    {
        private readonly IConfiguration configuration;

        public Program(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=EMKATA-ACER;Database=PressCenters;Trusted_Connection=True;MultipleActiveResultSets=true");
            
            var db = new ApplicationDbContext(optionsBuilder.Options);
            var list = new List<string>();
            var mn = db.MainNews.ToArray();

//            foreach (var c in mn)
//            {
//                if (list.Contains(c.Title))
//                {
//                    
//                    NonSerializ
//                    NO PYnsole.WriteLine(c.Title+"xxxxxxxxxxxxxx");
//                    list.Add(c.Title);
//                    db.Remove(c);
//                    db.SaveChanges();
//
//                }
//                else
//                {
//                    list.Add(c.Title);
//                }
//
//            }
            //
//            string link =
//                @"https://res.cloudinary.com/news0722/image/upload/v1562179622/x220_BSP_LOGO_Novo.jpg.pagespeed.ic.W_svtQ-rnm_z7knxv.jpg";
//
//            var clServ = new CloudinaryServise();
//           var result =  clServ.Upload(link);
//           clServ =new CloudinaryServise();
//           result = clServ.Upload(
//               "https://res.cloudinary.com/news0722/image/upload/v1562179725/190702141811-von-der-leyen-lagarde-split-large-tease_zdsyit.jpg");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.configuration);

            services.AddTransient<ICloudinaryServise, CloudinaryServise>();

        }
    }
}
