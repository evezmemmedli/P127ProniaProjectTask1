using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using P127ProniaProject.DAL;
using P127ProniaProject.Models;
using P127ProniaProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P127ProniaProject
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(_config.GetConnectionString("Default"));
            });

            services.AddIdentity<AppUser, IdentityRole>(opt =>
             {
                 opt.User.RequireUniqueEmail = false;
                 opt.Password.RequiredUniqueChars = 3;
                 opt.Password.RequiredLength = 8;
                 opt.Password.RequireNonAlphanumeric = false;
                 opt.Password.RequireDigit = true;
                 opt.Password.RequireUppercase = false;


                 opt.Lockout.MaxFailedAccessAttempts = 3;
                 opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                 opt.Lockout.AllowedForNewUsers = true;

                 opt.User.RequireUniqueEmail = false;
                 opt.User.AllowedUserNameCharacters = "qwertyuiopasdfghjkzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890_";





             }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
            services.AddScoped<LayoutService>();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=dashboard}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=home}/{action=Index}/{id?}");
            });
        }
    }
}
