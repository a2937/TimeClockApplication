using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeClock.DAL.EF.Repos.Base;
using TimeClock.DAL.EF.Repos.Interfaces;
using TimeClock.DAL.Models.Entities;

namespace TimeClock.DAL.EF.Repos
{
    public class DepartmentRepo : RepoBase<Department>, IDepartmentRepo
    {

        public IEnumerable<Department> GetEmployeesInDepartment(int departmentID) => Table.Where(x => x.Id == departmentID).Include(x => x.AssignedEmployees);
    }
}
