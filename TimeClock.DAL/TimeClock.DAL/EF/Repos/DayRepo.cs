using System;
using System.Collections.Generic;
using System.Text;
using TimeClock.DAL.EF.Repos.Base;
using TimeClock.DAL.EF.Repos.Interfaces;
using TimeClock.DAL.Models.Entities;

namespace TimeClock.DAL.EF.Repos
{
    public class DayRepo : RepoBase<Day> , IDayRepo
    {
    }
}
