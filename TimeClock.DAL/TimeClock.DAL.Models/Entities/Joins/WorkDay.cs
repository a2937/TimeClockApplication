using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TimeClock.DAL.Models.Entities.Base;

namespace TimeClock.DAL.Models.Entities.Joins
{
    [Table("WorkDays", Schema = "TimeSheet")]
    public class WorkDay : EntityBase
    {

        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        public DateTime? SignIn { get; set; }

        public DateTime? SignOut { get; set; }
    }
}
