using TimeSheet.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Models
{
    public class Supervisor : EntityBase
    {
        public string EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public ApplicationUser Manager { get; set; }

        [InverseProperty(nameof(Department.Supervisor))]
        public List<Department> Departments { get; set; } = new List<Department>();
    }
}
