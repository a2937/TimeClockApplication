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
    public class EmployeeRepo : RepoBase<Employee>, IEmployeeRepo
    {
        public EmployeeRepo(DbContextOptions<StoreContext> options) : base(options)
        {
        }
        public EmployeeRepo()
        {
        }
        public override IEnumerable<Employee> GetAll()
            => Table.OrderBy(x => x.LastName);

        public override IEnumerable<Employee> GetRange(int skip, int take)
            => GetRange(Table.OrderBy(x => x.LastName), skip, take);

        public IEnumerable<Employee> GetAllOverTimeEmployees() => Table.Where(x => x.ExemptFromOvertime == false);
    }
}
