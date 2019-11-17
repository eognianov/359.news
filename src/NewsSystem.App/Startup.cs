﻿using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsSystem.Data;
using NewsSystem.Data.Models;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Repositories;
using NewsSystem.Data.Common;
using NewsSystem.Data.Seeding;
using NewsSystem.Mappings;
using System.Reflection;
using NewsSystem.Common;
using NewsSystem.Services.Contracts;
using NewsSystem.Services;
using NewsSystem.Services.Data;
using NewsSystem.ViewModels;
using NewsSystem.Services.Clodinary;
using Microsoft.AspNetCore.Identity.UI;

namespace NewsSystem.App
{
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
            

//            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
//                options.UseNpgsql(
//                    this.configuration.GetConnectionString("PostgreSQL")));

            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    this.configuration.GetConnectionString("Development")));


            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders()
                .AddDefaultUI(UIFramework.Bootstrap4);


            services.AddAuthorization();
            
            services
                .Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.Lax;
                    options.ConsentCookie.Name = "0722.ConsentCookie";
                });

            services
                .ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/Logout";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    options.Cookie.Name = "0722.newsAuth";
                });

            services.AddSingleton(this.configuration);

            // Identity stores
            services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
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

            services.AddResponseCompression();


            services.AddMvc(options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }
                
                )
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {

            //AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
            AutoMapperConfig.RegisterMappings(typeof(NewsListViewModel).GetTypeInfo().Assembly);

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userMgr = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleMgr = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                

                //                if (env.IsDevelopment())
                //                {
                //                    dbContext.Database.Migrate();
                //                }


           
                ApplicationDbContextSeeder.Seed(dbContext, serviceScope.ServiceProvider);

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
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("news",
                    "News/{id:int:min(1)}/{slug:required}",
                    new { controller = "News", action = "ById", });
                routes.MapRoute("news",
                    "News/{id:int:min(1)}",
                    new { controller = "News", action = "ById", });
            });
        }
    }
}
