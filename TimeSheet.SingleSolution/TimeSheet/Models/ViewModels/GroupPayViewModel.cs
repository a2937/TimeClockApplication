using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Models.ViewModels
{
    public class GroupPayViewModel
    {

        public Guid DepartmentId { get; set; }

        public string DepartmentName { get; set; }

       
       public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        public decimal MoneyPaidToDepartment { get; set; }

    }
}
