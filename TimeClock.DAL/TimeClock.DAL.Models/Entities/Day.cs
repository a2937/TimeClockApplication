using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TimeClock.DAL.Models.Entities.Base;

namespace TimeClock.DAL.Models.Entities
{
    [Table("Days", Schema = "User")]
    public class Day : EntityBase
    {
        public DateTime Date { get; set; }


        public int WeekId { get; set; }

        [ForeignKey(nameof(WeekId))]
        public Week Week { get; set; }
    }
}
