using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TimeClock.DAL.Models.Entities;
using TimeClock.DAL.Models.Entities.Joins;

namespace TimeClock.DAL.EF
{
    public class StoreContext : DbContext
    {
        
       // private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public StoreContext()
        {
            
        }

        public StoreContext(DbContextOptions options) : base(options)
        {
            try
            {
                Database.Migrate();
            }
            catch (Exception ex)
            {
               // log.Error(ex.Message, ex);         
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // EnableRetryOnFailure adds default SqlServerRetryingExecutionStrategy
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=TimeClock;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.EmailAddress).HasName("IX_Employees").IsUnique();
            });
            modelBuilder.Entity<WorkSchedule>(entity =>
            {
                entity.Property(e => e.HoursWorked)
                    .HasDefaultValueSql("DATEDIFF ( Hours , SignIn , SignOut )");
            });
            modelBuilder.Entity<Day>(entity =>
            {
                entity.Property(e => e.Date)
                 .HasDefaultValueSql("getdate()");
            });
                

        }

        public virtual DbSet<Week> Weeks { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<WorkSchedule> WorkSchedules { get; set; }


        //Joins
    }
}
