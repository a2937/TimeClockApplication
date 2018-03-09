using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TimeClock.DAL.Models.Entities.Base;

namespace TimeClock.DAL.Models.Entities.Joins
{
    [Table("WorkSchedules", Schema = "TimeSheet")]
    public class WorkSchedule : EntityBase
    {

        public int DayId { get; set; }

        [ForeignKey(nameof(DayId))]
        public Day Day { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; }

        [DataType(DataType.Currency), Display(Name = "Amount of money earned.")]
        public decimal MoneyEarned { get; set; }


        public bool Approved{get;set;}
		
		[DataType(DataType.Text), MaxLength(50)]
		public string Reason{get;set;}
		
    }
}
