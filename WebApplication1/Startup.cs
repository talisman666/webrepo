using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication1.DAL.Entities;
using WebApplication1.DAL.Data;
using WebApplication1.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using WebApplication1.Extentions;
using Microsoft.Extensions.Logging;
using WebApplication1.Extensions;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(opt =>
            {
                opt.Cookie.HttpOnly = true;
                opt.Cookie.IsEssential = true;
            });
            services.AddScoped<Cart>(sp => CartService.GetCart(sp));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            //        .AddRazorRuntimeCompilation();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })

            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDistributedMemoryCache();
            services.AddSession(opt =>
            {
                opt.Cookie.HttpOnly = true;
                opt.Cookie.IsEssential = true;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
         //   services.AddScoped<Cart>(sp => CartService.GetCart(sp));

            services.AddControllersWithViews();
            services.AddRazorPages();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              ApplicationDbContext context,
                              UserManager<ApplicationUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              ILoggerFactory logger)
        {
            logger.AddFile("Logs/log-{Date}.txt");
            app.UseFileLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //logger.AddFile("Logs/log-{Date}.txt");
            //app.UseFileLogging();

            DbInitializer.Seed(context, userManager, roleManager)
                .GetAwaiter()
                .GetResult();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
