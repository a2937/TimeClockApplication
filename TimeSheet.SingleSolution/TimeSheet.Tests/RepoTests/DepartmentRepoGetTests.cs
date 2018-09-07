using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using TimeSheet.Models;
using TimeSheet.Models.Repos;

namespace TimeSheet.Tests
{
    [Collection("TimeSheet")]
    public class DepartmentRepoGetTests : IDisposable
    {
        private readonly DepartmentRepo _repo;

        public DepartmentRepoGetTests()
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
        public void ShouldGetFirstDepartment()
        {
            var categories = new List<Department>()
            {
                new Department { Name = "Foo" },
                new Department { Name = "Bar" },
                new Department { Name = "FooBar" }
            };
            _repo.AddRange(categories);
            Assert.NotEmpty(categories[0].Id.ToString());
        }

        [Fact]
        public void ShouldGetDepartment()
        {
            var Department = new Department { Name = "Foo" };
            _repo.Add(Department);
            _repo.Find(Department.Id);
        }
    }

}
