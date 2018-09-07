using TimeSheet.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Models
{
    [Table("Departments", Schema = "TimeSheet")]
    public class Department : EntityBase
    {
        [DataType(DataType.Text), MaxLength(50)]
        public string Name { get; set; }

        public Guid? SupervisorId { get; set; }

        [ForeignKey(nameof(SupervisorId))]
        public Supervisor Supervisor { get; set; }

        [InverseProperty(nameof(ApplicationUser.Department))]
        public List<ApplicationUser> AssignedEmployees { get; set; } = new List<ApplicationUser>();

        public Guid? CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
    }
}
