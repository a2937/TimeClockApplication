using System;
using System.Collections.Generic;
using System.Text;
using TimeClock.DAL.EF.Repos.Base;
using TimeClock.DAL.Models.Entities;

namespace TimeClock.DAL.EF.Repos.Interfaces
{
    public interface IWeekRepo : IRepo<Week>
    {
        IEnumerable<Week> GetWeeks();
    }
}
