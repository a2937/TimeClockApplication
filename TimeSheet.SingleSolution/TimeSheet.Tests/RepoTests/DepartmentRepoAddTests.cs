using System;
using System.Collections.Generic;
using System.Linq;
using TimeSheet.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using TimeSheet.Models.Repos;

namespace TimeSheet.Tests.RepoTests
{
    [Collection("TimeSheet")]
    public class DepartmentRepoAddTests : IDisposable
    {
        private readonly DepartmentRepo _repo;

        public DepartmentRepoAddTests()
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
            //_repo.Context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"TimeSheet.Departments\", RESEED, -1);");
        }

        [Fact]
        public void ShouldAddADepartment()
        {
            var Department = new Department { Name = "Foo" };
            var count = _repo.Add(Department);
            Assert.Equal(1, count);
            Assert.NotEmpty(Department.Id.ToString());
            Assert.Equal(1, _repo.Count);
        }
        [Fact]
        public void ShouldAddADepartmentAndNotSaveChanges()
        {
            var Department = new Department { Name = "Foo" };
            var count = _repo.Add(Department, false);
            Assert.Equal(0, count);
            //Assert.True(Department.Id < 0);
            Assert.Equal(0, _repo.Count);
        }

        [Fact]
        public void ShouldAddSeveralCategories()
        {
            var categories = new List<Department>()
            {
                new Department { Name = "Foo" },
                new Department { Name = "Bar" },
                new Department { Name = "FooBar" }
            };
            var count = _repo.AddRange(categories);
            Assert.Equal(3, count);
            Assert.Equal(3, _repo.GetAll().Count());
            Assert.Equal(3, _repo.Count);
        }

        [Fact]
        public void ShouldShowHasChanges()
        {
            var categories = new List<Department>()
            {
                new Department { Name = "Foo" },
                new Department { Name = "Bar" },
                new Department { Name = "FooBar" }
            };
            _repo.AddRange(categories, false);
            Assert.True(_repo.HasChanges);
            _repo.SaveChanges();
            Assert.False(_repo.HasChanges);
        }
    }

}
