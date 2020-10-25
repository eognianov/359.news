namespace NewsSystem.Worker.Runner
{

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using NewsSystem.Data;
    using NewsSystem.Data.Common;
    using NewsSystem.Data.Common.Repositories;
    using NewsSystem.Data.Models;
    using NewsSystem.Data.Repositories;
    using NewsSystem.Data.Seeding;
    using NewsSystem.Services.Clodinary;
    using NewsSystem.Services.Data;
    using NewsSystem.Worker.Common;
    using NewsSystem.Worker.Tasks;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));
#if DEBUG
            isService = false;
#endif

            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }
            else
            {
                Console.OutputEncoding = Encoding.UTF8;
            }

            var builder = new HostBuilder().ConfigureLogging(
                x =>
                {
                    x.ClearProviders();
                    x.AddProvider(new OneLineConsoleLoggerProvider(!isService));
                }).ConfigureServices(ConfigureServices);

            if (isService)
            {
                await builder.RunAsServiceAsync();
            }
            else
            {
                await builder.RunConsoleAsync();
            }
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true).AddEnvironmentVariables().Build();
            services.AddSingleton<IConfiguration>(configuration);

            var loggerFactory = new LoggerFactory();

            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("PostgreSQL-linode")), ServiceLifetime.Transient);

            //services.AddDbContext<ApplicationDbContext>(
            //    options => options.UseSqlServer(configuration.GetConnectionString("Development"))
            //        .UseLoggerFactory(loggerFactory));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            // Seed data on application startup
            using (var serviceScope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                ApplicationDbContextSeeder.Seed(dbContext, serviceScope.ServiceProvider);
            }

            // Data services
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<ITagsService, TagsService>();
            services.AddTransient<IWorkerTasksDataService, WorkerTasksDataService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IImagesServices, ImagesServices>();

            // Register TaskRunnerHostedService
            services.AddTransient<ITasksAssemblyProvider, TasksAssemblyProvider>();
            var parallelTasksCount = int.Parse(configuration["TasksExecutor:ParallelTasksCount"]);
            for (var i = 0; i < parallelTasksCount; i++)
            {
                services.AddHostedService<TasksExecutor>();
            }
        }

        public class TasksAssemblyProvider : ITasksAssemblyProvider
        {
            public Assembly GetAssembly()
            {
                return typeof(DbCleanupTask).Assembly;
            }
        }
    }
}
