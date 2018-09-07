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
    public class DepartmentRepoDeleteTests : IDisposable
    {
        private readonly DepartmentRepo _repo;

        public DepartmentRepoDeleteTests()
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
           /// _repo.Context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"TimeSheet.Departments\", RESEED, -1);");
        }

        //private IList<Product> CreateProducts()
        //{
        //    var prods = new List<Product>
        //    {
        //        new Product() {CurrentPrice = 12.99M, ModelName = "Product 1", ModelNumber = "P1"},
        //        new Product() {CurrentPrice = 9.99M, ModelName = "Product 2", ModelNumber = "P2"},
        //    };
        //    return prods;
        //}

        [Fact]
        public void ShouldDeleteADepartmentEntityFromDbSet()
        {
            _repo.AddRange(new List<Department>
            {
                new Department {Name = "Foo" },
            });
            Assert.Equal(1, _repo.Count);
            var Department = _repo.GetFirst();
            var count = _repo.Delete(Department);
            Assert.Equal(1, count);
            Assert.Equal(0, _repo.Count);
        }

        [Fact]
        public void ShouldDeleteADepartmentRangeFromDbSet()
        {
            var categories = new List<Department>
            {
                new Department { Name = "Foo" },
                new Department {Name = "Bar" },
                new Department { Name = "FooBar" }
            };
            _repo.AddRange(categories);
            Assert.Equal(3, _repo.Count);
            var count = _repo.DeleteRange(categories);
            Assert.Equal(3, count);
            Assert.Equal(0, _repo.Count);
        }

        [Fact]
        public void ShouldDeleteADepartmentRangeAndPersistManuallyFromDbSet()
        {
            var categories = new List<Department>
            {
                new Department { Name = "Foo" },
                new Department { Name = "Bar" },
                new Department { Name = "FooBar" }
            };
            _repo.AddRange(categories);
            Assert.Equal(3, _repo.Count);
            var count = _repo.DeleteRange(categories, false);
            Assert.Equal(0, count);
            count = _repo.SaveChanges();
            Assert.Equal(3, count);
            Assert.Equal(0, _repo.Count);
        }

        [Fact]
        public void ShouldDeleteADepartmentEntityFromContext()
        {
            _repo.AddRange(new List<Department>
            {
                new Department { Name = "Foo" },
            });
            Assert.Equal(1, _repo.Count);
            var Department = _repo.GetFirst();
            _repo.Context.Remove(Department);
            var count = _repo.SaveChanges();
            Assert.Equal(1, count);
            Assert.Equal(0, _repo.Count);
        }

        [Fact]
        public void ShouldDeleteADepartmentEntityAndNotPersist()
        {
            _repo.AddRange(new List<Department>
            {
                new Department { Name = "Foo" },
            });
            Assert.Equal(1, _repo.Count);
            var Department = _repo.GetFirst();
            var count = _repo.Delete(Department, false);
            Assert.Equal(0, count);
            Assert.Equal(1, _repo.Count);
        }

        [Fact]
        public void ShouldDeleteADepartmentFromDifferentContext()
        {
            _repo.AddRange(new List<Department>
            {
                new Department { Name = "Foo" },
            });
            Assert.Equal(1, _repo.Count);
            var Department = _repo.GetFirst();
            DepartmentRepo repo = new DepartmentRepo();
            var count = repo.Delete(Department.Id, Department.TimeStamp, false);
            Assert.Equal(0, count);
            count = repo.Context.SaveChanges();
            Assert.Equal(1, count);
            Assert.Equal(0, repo.Count);
        }

        [Fact]
        public void ShouldDeleteADepartmentFromSameContext()
        {
            var Department = new Department { Name = "Foo" };
            _repo.Add(Department);
            Assert.Equal(1, _repo.Count);
            var count = _repo.Delete(Department.Id, Department.TimeStamp, false);
            Assert.Equal(0, count);
            count = _repo.SaveChanges();
            Assert.Equal(1, count);
            Assert.Equal(0, _repo.Count);
        }
    }

}
