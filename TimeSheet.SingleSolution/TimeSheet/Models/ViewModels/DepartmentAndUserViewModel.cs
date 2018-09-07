using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Models.ViewModels
{
    public class DepartmentAndUserViewModel
    {
        /*
        public Guid DepartmentId { get; set; }

        [DataType(DataType.Text), MaxLength(50)]
        public string DepartmentName { get; set; }

        
        [DataType(DataType.Text), Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Text), Display(Name = "Last Name")]
        public string LastName { get; set; }
        */
        public Guid DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }


        public IEnumerable<ApplicationUser> Members { get; set; }

        public Guid? SupervisorId { get; set; }

        public Supervisor Supervisor { get; set; }
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }

    public class DepartmentEditViewModel
    {
        [Required]
        public string DepartmentName { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid NewSuperVisorId { get; set; } // The id of the supervisor, not the user he is

        public string NewSuperVisorEmployeeId { get; set; } // The id of the supervisor in the users table

        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }

    }
}
