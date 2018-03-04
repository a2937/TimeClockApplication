using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TimeClock.DAL.Models.Entities.Base;

namespace TimeClock.DAL.Models.Entities
{
    [Table("Weeks", Schema = "User")]
    public class Week : EntityBase
    {
        [DataType(DataType.Date), Display(Name = "Hourly Wage")]
        public DateTime WeekOf { get; set; }


    }
}
