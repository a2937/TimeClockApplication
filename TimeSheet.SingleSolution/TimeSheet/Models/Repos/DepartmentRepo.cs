using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TimeSheet.Data;
using TimeSheet.Models.Repos.Base;
using TimeSheet.Models.Repos.Interfaces;
using TimeSheet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Models.Repos
{
    public class DepartmentRepo : RepoBase<Department>, IDepartmentRepo
    {
        private UserManager<ApplicationUser> _userManager;
        private DbContextOptions<ApplicationDbContext> options;

        public DepartmentRepo(DbContextOptions<ApplicationDbContext> options, UserManager<ApplicationUser> userManager) : base(options)
        {
            _userManager = userManager;
        }

        public DepartmentRepo() : base()
        {

        }

        public DepartmentRepo(DbContextOptions<ApplicationDbContext> options)
        {
            this.options = options;
        }

        public override IEnumerable<Department> GetRange(int skip, int take)
           => GetRange(Table.OrderBy(x => x.Name), skip, take);

         public IEnumerable<Department> GetAllWithUsers()
         => Table.Include(x => x.AssignedEmployees);

        /*
        public IEnumerable<DepartmentAndUserViewModel> GetUsersWithDepartment(Guid id)
        =>  _userManager.Users.Where(x => x.DepartmentId == id).Select(item => GetRecord(item, item.Department));
           // Table.Include(x => x.AssignedEmployees).Where(x => x.Id == id).;
        

    internal DepartmentAndUserViewModel GetRecord( ApplicationUser user,Department p)
    {

       return new DepartmentAndUserViewModel
       {
           Department = p,
           FirstName = user.FirstName,
           LastName = user.LastName
       };

    }
    */

        /*
        return new DepartmentAndUserViewModel
        {
            DepartmentName = p.Name,
            FirstName = c.FirstName,
            LastName = c.LastName
        };
        */

        /*
    public IEnumerable<DepartmentAndUserViewModel> GetUsersForDepartment(string id)
         => Table
             .Where(p => p.Id == id)
             .Include(p => p.AssignedEmployees)
             .Select(
                    item => GetRecord(item, item.AssignedEmployees)
                    );
                    */
        //.OrderBy(x => x.);


        //  {
        // IIncludableQueryable<Department,List<ApplicationUser>> assignedEmployees = Table.Where(p => p.Id == id).Include(p => p.AssignedEmployees);

        //assignedEmployees.
        /*Table.Where(p => p.Id == id)
        .Include(p => p.AssignedEmployees)
        .Select(item => GetRecord(item, item.AssignedEmployees))
        .OrderBy(x => x.DepartmentName);
        */
        // }
        public override IEnumerable<Department> GetAll()
           => Table.OrderBy(x => x.Name);

        public Department GetOneWithUser(Guid id)
        => Table.Include(x => x.AssignedEmployees).FirstOrDefault(x => x.Id == id);


        public int RemoveFromDepartment(ApplicationUser user, Guid DepartmentId)
        {
            if(user == null)
            {
                return 0;
            }
             user.DepartmentId = null;
            Department department = Table.Include(x => x.AssignedEmployees).Where(x => x.Id == DepartmentId).First();

            SaveChanges();
            if (department.AssignedEmployees.Contains(user) == true)
            {
                department.AssignedEmployees.Remove(user);

                SaveChanges();

                //department.AssignedEmployees.Add(user);
                //SaveChanges();
            }
            return department.AssignedEmployees.Contains(user) ? 0 : 1;
            //department.AssignedEmployees.Remove(user);
            //user.DepartmentId = null; 
            //return SaveChanges();
        }


        public int AddToDepartment(ApplicationUser user, Guid DepartmentId)
        {
            if(user == null)
            {
                return 0;
            }
            Department department = Table.Include(x => x.AssignedEmployees).Where(x => x.Id == DepartmentId).First();
           // Department department = Table.Find(DepartmentId);
            
            if(department != null)
            {
                user.DepartmentId = DepartmentId;
                
                SaveChanges();
                if(department.AssignedEmployees.Contains(user) == false)
                {
                   // department.AssignedEmployees.Remove(user);

                   // SaveChanges();

                    department.AssignedEmployees.Add(user);

                    SaveChanges();
                }
                return department.AssignedEmployees.Contains(user) ? 1 : 0;
            }
            else
            {
                return 0;
            }
        }

        public bool UserIsInDepartment(ApplicationUser user, Guid id)
        {
            return user.DepartmentId.Equals(id);
            //return
        }
    }
}
