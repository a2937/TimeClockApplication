using TimeSheet.Data;
using TimeSheet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Initializers
{
    public static class SampleData
    {

        public static IEnumerable<Department> GetDepartments() => new List<Department>
        {
            new Department {Name = "Research"},
            new Department {Name = "Animation"},
            new Department {Name = "Backend Development"},
            new Department {Name = "Storyboarding"},
            new Department {Name = "Quality Assurance"},
            new Department {Name = "Accounting"}
        };


        public static IEnumerable<ApplicationUser> GetAllCustomerRecords(ApplicationDbContext context) => new List<ApplicationUser>
        {
            new ApplicationUser()
            {
                Email = "spy@secrets.com",
                FirstName = "Super",
                LastName = "Spy"
            }
        };
    }
}
