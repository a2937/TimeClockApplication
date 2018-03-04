using System;
using System.Collections.Generic;
using System.Text;
using TimeClock.DAL.Models.Entities.Base;

namespace TimeClock.DAL.Models.Entities
{
    public class Day : EntityBase
    {
        DateTime Date { get; set; }
    }
}
