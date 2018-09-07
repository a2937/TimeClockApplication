using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using TimeSheet.Models.Repos;
using TimeSheet.Models;

namespace TimeSheet.Tests.RepoTests
{
    [Collection("TimeSheet")]
    public class DepartmentRepoUpdateTests : IDisposable
    {
        private readonly DepartmentRepo _repo;

        public DepartmentRepoUpdateTests()
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
        public void ShouldUpdateADepartmentEntity()
        {
            var Department = new Department {Name = "Foo" };
            _repo.AddRange(new List<Department>
            {
                Department,
            });
            Department.Name = "Bar";
            _repo.Update(Department, false);
            var count = _repo.SaveChanges();
            Assert.Equal(1, count);
            var repo = new DepartmentRepo();
            var cat = repo.GetFirst();
            Assert.Equal(cat.Name, Department.Name);

        }
        [Fact]
        public void ShouldUpdateARangeOfDepartmentEntities()
        {
            var categories = new List<Department>
            {
                new Department { Name = "Foo" },
                new Department { Name = "Bar" },
                new Department { Name = "FooBar" }
            };
            _repo.AddRange(categories);
            categories[0].Name = "Foo1";
            categories[1].Name = "Foo2";
            categories[2].Name = "Foo3";
            _repo.UpdateRange(categories, false);
            var count = _repo.SaveChanges();
            Assert.Equal(3, count);
            var repo = new DepartmentRepo();
            var cat = repo.GetFirst();
            Assert.Equal("Foo1", cat.Name);

        }
    }

}
