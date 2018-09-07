using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TimeSheet.Models.Repos;
using TimeSheet.Models;
using TimeSheet.Data;
using TimeSheet.EF;

namespace TimeSheet.Tests
{
    [Collection("TimeSheet")]
    public class DepartmentRepoExceptionTests : IDisposable
    {
        private readonly DepartmentRepo _repo;

        public DepartmentRepoExceptionTests()
        {
            _repo = new DepartmentRepo();
            CleanDatabase();
        }

        public void Dispose()
        {
            CleanDatabase();
            _repo.Dispose();
        }

        private void CleanDatabase()
        {
            _repo.Context.Database.ExecuteSqlCommand("Delete from TimeSheet.Departments");
           // _repo.Context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"TimeSheet.Departments\", RESEED, -1);");
        }

        [Fact]
        public void ShouldNotDeleteADepartmentFromSameContextWithConcurrencyIssue()
        {
            var Department = new Department {Name = "Foo"};
            _repo.Add(Department);
            Assert.Equal(1, _repo.Count);
            var ex = Assert.Throws<Exception>(() => _repo.Delete(Department.Id, null, false));
        }
        /*
        [Fact]
        public void ShouldNotDeleteOnConcurrencyIssue()
        {
            var Department = new Department {Name = "Foo"};
            _repo.Add(Department);
            _repo.Context.Database.ExecuteSqlCommand("Update TimeSheet.Departments set Name = 'Bar'");
            var ex = Assert.Throws<DbUpdateConcurrencyException>(() => _repo.Delete(Department.Id, Department.TimeStamp));
        }
        */

            /*
        [Fact]
        public void ShouldThrowRetryExeptionWhenCantConnect()
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString =
                @"Server=(localdb)\mssqllocaldb;Database=CottrillTimeSheet;user id=foo;password=bar;MultipleActiveResultSets=true;";
            //contextOptionsBuilder.UseSqlServer(connectionString, 
            //    o => o.EnableRetryOnFailure(2,new TimeSpan(0,0,0,0,100),new Collection<int>{ -2146232060 }));
            contextOptionsBuilder.UseSqlServer(connectionString,
                o => o.ExecutionStrategy(c => new MyExecutionStrategy(c, 5, new TimeSpan(0, 0, 0, 0, 30))));
            var repo = new DepartmentRepo(contextOptionsBuilder.Options);
            var Department = new Department {Name = "Foo"};
            var ex = Assert.Throws<RetryLimitExceededException>(()=> repo.Add(Department));
        }
        */

    }
}