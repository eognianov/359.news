using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(NewsSystem.App.Areas.Identity.IdentityHostingStartup))]
namespace NewsSystem.App.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}