using Microsoft.EntityFrameworkCore;
using TimeSheet.Data;
using TimeSheet.Models;
using System;
using System.Linq;
using Xunit;

namespace TimeSheet.Tests
{
    [Collection("TimeSheet")]
    public class DepartmentTests : IDisposable
    {
        private readonly ApplicationDbContext _db;

        public DepartmentTests()
        {
            _db = new ApplicationDbContext();
            CleanDatabase();
        }

        public void Dispose()
        {
            CleanDatabase();
            _db.Dispose();
        }

        private void CleanDatabase()
        {
            _db.Database.ExecuteSqlCommand("Delete from TimeSheet.Departments");
            //_db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"TimeSheet.Departments\", RESEED, -1);");
        }

        [Fact]
        public void FirstTest()
        {
            Assert.True(true);
        }

      
        [Fact]
        public void CanAddDepartmentTest()
        {
            Department department = new Department { Name = "Department of Redundancy" };
            _db.Departments.Add(department);
            Assert.Equal(EntityState.Added, _db.Entry(department).State);
            //Assert.True(department.Id == null);
            Assert.Null(department.TimeStamp);
            _db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, _db.Entry(department).State);
            Assert.NotNull(department.TimeStamp);
            Assert.Equal(1, _db.Departments.Count());
            //var data = new[] { new Department {Name="Test" } };
            //var mock = new Mock<IDepartmentRepo>();
            //mock.SetupGet(m => m.GetAll()).Returns(data);
            //TypeBroker.SetTestObject(mock.Object);

        }

        [Fact]
        public void ShouldAddADepartmentWithContext()
        {
            var department = new Department { Name = "Foo" };
            _db.Add(department);
            Assert.Equal(EntityState.Added, _db.Entry(department).State);
            //Assert.True(department.Id < 0);
            Assert.Null(department.TimeStamp);
            _db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, _db.Entry(department).State);
            //Assert.Equal(0, department.Id);
            Assert.NotNull(department.TimeStamp);
            Assert.Equal(1, _db.Departments.Count());
        }

        [Fact]
        public void ShouldGetAllDepartmentsOrderedByName()
        {
            _db.Departments.Add(new Department {Name = "Foo" });
            _db.Departments.Add(new Department{ Name = "Bar" });
            _db.SaveChanges();
            var categories = _db.Departments.OrderBy(c => c.Name).ToList();
            Assert.Equal(2, _db.Departments.Count());
            Assert.Equal("Bar", categories[0].Name);
            Assert.Equal("Foo", categories[1].Name);
        }

        [Fact]
        public void ShouldUpdateADepartment()
        {
            var department = new Department {Name = "Foo" };
            _db.Departments.Add(department);
            _db.SaveChanges();
            department.Name = "Bar";
            _db.Departments.Update(department);
            Assert.Equal(EntityState.Modified, _db.Entry(department).State);
            _db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, _db.Entry(department).State);
            ApplicationDbContext context;
            using (context = new ApplicationDbContext())
            {
                Assert.Equal("Bar", context.Departments.First().Name);
            }
        }
        /*
        [Fact]
        public void ShouldNotUpdateANonAttachedDepartment()
        {
            var department = new Department { Name = "Foo" };
            _db.Departments.Add(department);
            department.Name = "Bar";
            Assert.Throws<InvalidOperationException>(() => _db.Departments.Update(department));
        }
        */

        [Fact]
        public void ShouldDeleteADepartment()
        {
            var department = new Department { Name = "Foo" };
            _db.Departments.Add(department);
            _db.SaveChanges();
            Assert.Equal(1, _db.Departments.Count());
            _db.Departments.Remove(department);
            Assert.Equal(EntityState.Deleted, _db.Entry(department).State);
            _db.SaveChanges();
            Assert.Equal(EntityState.Detached, _db.Entry(department).State);
            Assert.Equal(0, _db.Departments.Count());
        }
        [Fact]
        public void ShouldDeleteADepartmentWithTimestampData()
        {
            var department = new Department { Name = "Foo" };
            _db.Departments.Add(department);
            _db.SaveChanges();
            var context = new ApplicationDbContext();
            var catToDelete = new Department { Id = department.Id, TimeStamp = department.TimeStamp };
            context.Entry(catToDelete).State = EntityState.Deleted;
            var affected = context.SaveChanges();
            Assert.Equal(1, affected);
        }
        [Fact]
        public void ShouldNotDeleteADepartmentWithoutTimestampData()
        {
            var department = new Department { Name = "Foo" };
            _db.Departments.Add(department);
            _db.SaveChanges();
            var context = new ApplicationDbContext();
            var catToDelete = new Department{ Id = department.Id };
            context.Departments.Remove(catToDelete);
            var ex = Assert.Throws<DbUpdateConcurrencyException>(() => context.SaveChanges());
            Assert.Equal(1, ex.Entries.Count);
            Assert.Equal(department.Id, ((Department)ex.Entries[0].Entity).Id);
        }
    }
}
