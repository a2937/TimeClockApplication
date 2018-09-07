using TimeSheet.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Models
{
    [Table("WorkDays", Schema = "TimeSheet")]
    public class WorkDay : EntityBase
    {
        public string EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public ApplicationUser Employee { get; set; }

        [DataType(DataType.Date), Display(Name = "Date")]
        public DateTime Date { get; set; }

        [DataType(DataType.Time), Display(Name = "Sign in")]
        public DateTimeOffset? SignIn { get; set; }

        [DataType(DataType.Time), Display(Name = "Sign Out")]
        public DateTimeOffset? SignOut { get; set; }

        public bool IsVacationDay { get; set; }

        [Display(Name = "Hours Worked")]
        public int HoursWorked { get; set; }

        [DataType(DataType.Currency), Display(Name = "Estimated Pay")]
        public decimal EstimatedPay { get; set; }

        public bool IsApproved { get; set; }

        [DataType(DataType.Text), MaxLength(100)]
        public string Reason { get; set; }

        //public Guid? WorkScheduleId { get; set; }

        //[ForeignKey(nameof(WorkScheduleId))]
        //public WorkSchedule WorkSchedule { get; set; }
    }
}
