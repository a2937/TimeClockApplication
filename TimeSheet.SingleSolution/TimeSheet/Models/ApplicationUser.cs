using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TimeSheet.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    [Table("Employees", Schema = "TimeSheet")]
    public class ApplicationUser : IdentityUser
    {
        [DataType(DataType.Text), MaxLength(50), Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Text), MaxLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Text), MaxLength(1), Display(Name = "Middle Initial")]
        public string MiddleInitial { get; set; }

        [DataType(DataType.Currency), Display(Name = "Hourly Wage")]
        public decimal HourlyWage { get; set; }

        [Display(Name = "Except From Overtime")]
        public bool ExceptFromOvertime { get; set; }

        public Guid? DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
    }
}
