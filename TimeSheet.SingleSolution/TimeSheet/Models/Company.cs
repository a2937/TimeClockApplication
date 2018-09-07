using TimeSheet.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Models
{
    [Table("Companies", Schema = "TimeSheet")]
    public class Company : EntityBase
    {
        [DataType(DataType.Text), MaxLength(50)]
        public string CompanyName { get; set; }

        [DataType(DataType.Text), MaxLength(50), Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [DataType(DataType.Text), MaxLength(50)]
        public string City { get; set; }

        [DataType(DataType.Text), MaxLength(2), Display(Name = "State")]
        public string StateAbbreviated { get; set; }

        [DataType(DataType.Currency), Display(Name = "Wages Paid")]
        public decimal WagesPaid { get; set; }

        [InverseProperty(nameof(Department.Company))]
        public List<Department> Departments { get; set; }
    }
}
