using TimeSheet.Models.Repos.Base;
using TimeSheet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Models.Repos.Interfaces
{
    public interface IDepartmentRepo : IRepo<Department>
    {
        //IEnumerable<DepartmentAndUserViewModel> GetUsersWithDepartment(Guid id);

        IEnumerable<Department> GetAllWithUsers();

        Department GetOneWithUser(Guid id);

        int RemoveFromDepartment(ApplicationUser user, Guid DepartmentId);
        int AddToDepartment(ApplicationUser user, Guid DepartmentId);
        bool UserIsInDepartment(ApplicationUser user, Guid id);
    }
}
