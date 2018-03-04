using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TimeClock.DAL.Models.Entities.Base;

namespace TimeClock.DAL.Models.Entities
{
    [Table("Employees", Schema = "User")]
    public class Employee : EntityBase
    {
        [DataType(DataType.Text), MaxLength(50), Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Text), MaxLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Excempt From Overtime")]
        public bool ExemptFromOvertime { get; set; }

        [DataType(DataType.Currency), Display(Name = "Hourly Wage")]
        public decimal HourlyWage { get; set; }

        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress), MaxLength(50), Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password), MaxLength(50)]
        public string Password { get; set; }

      
        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
    }
}
