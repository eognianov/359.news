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

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.configuration);

            services.AddTransient<ICloudinaryService, CloudinaryService>();

        }
    }
}
