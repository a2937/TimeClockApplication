using MidtermTimeClockCottrill.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MidtermTimeClockCottrill.Models
{
    [Table("WorkSchedule", Schema = "TimeSheet")]
    public class WorkSchedule : EntityBase
    {

        public List<WorkDay> WorkDays { get; set; }

        [DataType(DataType.Currency), Display(Name = "Estimated Week Pay")]
        public decimal EstimatedWeekPay { get; set; }
    }
}
