using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeSheet.Data;
using TimeSheet.Models;
using TimeSheet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Rewrite;
using TimeSheet.Infrastructure;
using TimeSheet.Models.Repos.Interfaces;
using TimeSheet.Models.Repos;

namespace TimeSheet
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        private IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<IPasswordValidator<ApplicationUser>,
          CustomPasswordValidator>();
            services.AddTransient<IUserValidator<ApplicationUser>,
                CustomUserValidator>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("CottrillTimeSheet")));



            services.AddIdentity<ApplicationUser, IdentityRole>(opts => {
                opts.User.RequireUniqueEmail = true;
                //opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddScoped<IDepartmentRepo, DepartmentRepo>();
            services.AddScoped<ISupervisorRepo, SupervisorRepo>();


            services.AddScoped<IWorkDayRepo, WorkDayRepo>();



            services.AddMvc();
            /*
             *
            services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordValidator>();
            services.AddTransient<IUserValidator<ApplicationUser>, CustomUserValidator>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("CottrillTimeSheet")));

            
            services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = true;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireDigit = true;
                opts.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

           */
            // var skipHTTPS = Configuration.GetValue<bool>("LocalTest:skipHTTPS");

            // From
            //https://docs.microsoft.com/en-us/aspnet/core/security/authorization/secure-data

            /*
            services.Configure<MvcOptions>(options =>
            {
                // Set LocalTest:skipHTTPS to true to skip SSL requrement in 
                // debug mode. This is useful when not using Visual Studio.
                if (Environment.IsDevelopment() && !skipHTTPS)
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }
            });
            */

            // Add application services.

            /*
            services.AddTransient<IEmailSender, EmailSender>();

        

            services.AddMvc();

            */
            //services.AddScoped<ApplicationDbContext>(_ => new ApplicationDbContext());
            // requires: using Microsoft.AspNetCore.Authorization;
            //  
            //using Microsoft.AspNetCore.Mvc.Authorization;

            /*
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            */


        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /*
           // 
            // * Force back to HTTPS
            // * https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl
           //   
           // var options = new RewriteOptions()
            //  .AddRedirectToHttps();
            //app.UseRewriter(options);
            */

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            ApplicationDbContext.CreateAdminAccount(app.ApplicationServices,
        Configuration).Wait();
            ApplicationDbContext.CreateAuditorAccount(app.ApplicationServices,
        Configuration).Wait();
            ApplicationDbContext.CreatePayrollSupervisorRole(app.ApplicationServices,
        Configuration).Wait();
            ApplicationDbContext.CreateManagerRole(app.ApplicationServices,
                 Configuration).Wait();
        }


    }
}
