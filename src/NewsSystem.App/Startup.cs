namespace NewsSystem.App
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Security.Principal;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using NewsSystem.Common;
    using NewsSystem.Data;
    using NewsSystem.Data.Common;
    using NewsSystem.Data.Common.Repositories;
    using NewsSystem.Data.Models;
    using NewsSystem.Data.Repositories;
    using NewsSystem.Data.Seeding;
    using NewsSystem.Mappings;
    using NewsSystem.Services;
    using NewsSystem.Services.Clodinary;
    using NewsSystem.Services.CronJobs;
    using NewsSystem.Services.Contracts;
    using NewsSystem.Services.Data;
    using NewsSystem.ViewModels;

    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.PostgreSql;

    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config=>config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings().UsePostgreSqlStorage(
                    this.configuration.GetConnectionString("PostgreSQL-linode")));
            services.AddHangfireServer();

            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    this.configuration.GetConnectionString("PostgreSQL-linode")), ServiceLifetime.Transient);


            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        this.configuration.GetConnectionString("Development")));


            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            }).AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddResponseCompression();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAuthorization();
            
            services
                .Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.Lax;
                    options.ConsentCookie.Name = "078.ConsentCookie";
                });

            services
                .ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/Logout";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    options.Cookie.Name = "078.newsAuth";
                });

            services.AddSingleton(this.configuration);

            // Identity stores
            services.AddTransient<UserManager<ApplicationUser>>();

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IPublishableEntityRepository<>), typeof(EfPublishableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            //            services.AddTransient<IEmailSender, NullMessageSender>();
            //services.AddTransient<ISmsSender, NullMessageSender>();
            //            services.AddTransient<ISettingsService, SettingsService>();

            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<ISlugGenerator, SlugGenerator>();
            services.AddTransient<IImagesServices, ImagesServices>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IFacebookPage, FacebookPage>();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
        {

            //AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
            AutoMapperConfig.RegisterMappings(typeof(NewsListViewModel).GetTypeInfo().Assembly);

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userMgr = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleMgr = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();


                //if (env.IsDevelopment())
                //{
                //    dbContext.Database.Migrate();
                //}



                ApplicationDbContextSeeder.Seed(dbContext, serviceScope.ServiceProvider);
                this.SeedHangfireJobs(recurringJobManager, dbContext);

                var adminUserName = this.configuration["Admin:username"];

                if (!dbContext.Users.Any(u => u.UserName == adminUserName))
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = adminUserName,
                        Email = this.configuration["Admin:email"]
                    };

                    var result = userMgr.CreateAsync(adminUser, this.configuration["Admin:password"]).GetAwaiter().GetResult();
                    userMgr.AddToRoleAsync(adminUser, GlobalConstants.AdministratorRoleName).GetAwaiter().GetResult();
                }
            }

           if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 5 });
            app.UseHangfireDashboard(
                "/hangfire",
                new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("news", "News/{id:int:min(1)}/{slug:required}", new { controller = "News", action = "ById", });
                endpoints.MapControllerRoute("news", "News/{id:int:min(1)}", new { controller = "News", action = "ById", });
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private void SeedHangfireJobs(IRecurringJobManager recurringJobManager, ApplicationDbContext dbContext)
        {
            recurringJobManager.AddOrUpdate<DbCleanupJob>("DbCleanupJob", x => x.Work(), Cron.Weekly);
            recurringJobManager.AddOrUpdate<MainNewsGetterJob>("MainNewsGetterJob", x => x.Work(), "*/2 * * * *");
            var sources = dbContext.Sources.Where(x => !x.IsDeleted).ToList();
            foreach (var source in sources)
            {
                recurringJobManager.AddOrUpdate<GetLatestPublicationsJob>(
                    $"GetLatestPublicationsJob_{source.Id}_{source.ShortName}",
                    x => x.Work(source.TypeName),
                    "*/5 * * * *");
            }
        }

        public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();
                return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName);
            }
        }

    }
}
