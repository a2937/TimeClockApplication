using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Models.ViewModels
{
    public class CheckViewModel
    {
        public decimal Payment { get; set; }

        public DateTime Date { get; set; }

        public string EmployeeFirstName { get; set; }

        public string EmployeeLastName { get; set; }

        public string PaymentInMoney { get; set; }

    }
}
