using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TimeClock.DAL.Models.Entities.Base;

namespace TimeClock.DAL.Models.Entities.Joins
{
    public class WorkSchedule : EntityBase
    {

        public int DayId { get; set; }

        [ForeignKey(nameof(DayId))]
        public Day Day { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        public DateTime SignIn { get; set; }

        public DateTime SignOut { get; set; }

        public int HoursWorked { get; set; } // Should be Calculated from sign in and sign out
    }
}
