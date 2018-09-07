using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeSheet.Models;

namespace TimeSheet.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = configuration["UserData:AdminUser:Name"];
            string email = configuration["UserData:AdminUser:Email"];
            string password = configuration["UserData:AdminUser:Password"];
            string role = configuration["UserData:AdminUser:Role"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
                ApplicationUser user = new ApplicationUser
                {
                    UserName = username,
                    Email = email
                };
                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }

        public static async Task CreateAuditorAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = configuration["UserData:Auditor:Name"];
            string email = configuration["UserData:Auditor:Email"];
            string password = configuration["UserData:Auditor:Password"];
            string role = configuration["UserData:Auditor:Role"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
                ApplicationUser user = new ApplicationUser
                {
                    UserName = username,
                    Email = email
                };
                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }

        public static async Task CreatePayrollSupervisorRole(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string role = configuration["RoleData:PayrollSupervisor:Name"];

            if (await roleManager.FindByNameAsync(role) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public static async Task CreateManagerRole(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string role = configuration["RoleData:Manager:Name"];

            if (await roleManager.FindByNameAsync(role) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // EnableRetryOnFailure adds default SqlServerRetryingExecutionStrategy
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=CottrillTimeSheet;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);


           

            builder.Entity<WorkDay>(entity =>
            {
                entity.Property(e => e.SignIn)
                    .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(e => e.SignOut)
                    .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(e => e.HoursWorked)
                    .HasComputedColumnSql("DateDiff(hour,[SignIn], [SignOut])");

                //entity.Property(e => e.IsVacationDay)
                // .HasDefaultValueSql("0");

                //entity.Property(e => e.IsApproved)
                //.HasDefaultValueSql("0");

                entity.Property(e => e.EmployeeId)
                .HasDefaultValue("");

                entity.Property(e => e.Date)
                .HasDefaultValueSql("getutcdate()");

               
                entity.Property(e => e.EstimatedPay)
                  .HasComputedColumnSql("TimeSheet.GetEstimatedPay([EmployeeId])"); // Probably will need a function in there
                   

            });

           
        
        }

        public DbSet<ApplicationUser> Employees { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        //public DbSet<WorkSchedule> WorkSchedules { get; set; }
  
        public DbSet<Company> Companies { get; set; }

    }
}
