using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Models.ViewModels
{
    public class GroupPayOverallViewModel
    {
        public IEnumerable<GroupPayViewModel> GroupPayments {get;set;}
    }
}
