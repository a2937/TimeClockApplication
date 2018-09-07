using Microsoft.EntityFrameworkCore;
using TimeSheet.Data;
using TimeSheet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace TimeSheet.Tests.ContextTests
{
    [Collection("TimeSheet")]
    public class WorkDayTest : IDisposable
    {
        private readonly ApplicationDbContext _db;


        public WorkDayTest()
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
            _db.Database.ExecuteSqlCommand("Delete from TimeSheet.WorkDays");
          
            //_db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"TimeSheet.Departments\", RESEED, -1);");
        }

        /*
        [Fact]
        public void CanAddWorkDayTest()
        {
           
            WorkDay department = new WorkDay {EmployeeId = "" };
            _db.WorkDays.Add(department);
            Assert.Equal(EntityState.Added, _db.Entry(department).State);
            //Assert.True(department.Id == null);
            Assert.Null(department.TimeStamp);
            _db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, _db.Entry(department).State);
            Assert.NotNull(department.TimeStamp);
            Assert.Equal(1, _db.WorkDays.Count());
            //var data = new[] { new Department {Name="Test" } };
            //var mock = new Mock<IDepartmentRepo>();
            //mock.SetupGet(m => m.GetAll()).Returns(data);
            //TypeBroker.SetTestObject(mock.Object);
        }
        */


    }
}
