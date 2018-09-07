using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeSheet.Data;
using TimeSheet.Models;
using TimeSheet.Models.Repos.Base;
using TimeSheet.Models.Repos.Interfaces;
using TimeSheet.Models.ViewModels;

namespace TimeSheet.Models.Repos
{
    public class WorkDayRepo : RepoBase<WorkDay>, IWorkDayRepo
    {
        private readonly ApplicationDbContext _context;

        private UserManager<ApplicationUser> _userManager;

        public WorkDayRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<WorkDay>> GetApprovedWorkDaysAsync()
        {
            var days = await _context.WorkDays
                 .Where(x => x.IsApproved == true && x.IsVacationDay == false)
                 .ToArrayAsync();

            return days;
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            var departments = await _context.Departments
                   .ToArrayAsync();
            return departments;
        }

        /*
         * @TODO Fix this
         */
        internal GroupPayViewModel GetRecord(WorkDay work, Department department)
        {
           
            decimal amount = 0;
            foreach(ApplicationUser user in department.AssignedEmployees)
            {
                if(user.Id == work.EmployeeId)
                {
                    amount += work.EstimatedPay;
                }
            }
            return new GroupPayViewModel { Date = work.Date, DepartmentName = department.Name,DepartmentId=department.Id, MoneyPaidToDepartment =  amount};
        }

        /*
         * @TODO Fix this
         */
        public async Task<GroupPayOverallViewModel> GetGroupPaymentsAsync()
        {

            var departments = await _context.Departments.ToArrayAsync();
            var workDays = await _context.WorkDays.Where(x => x.IsVacationDay == false && x.IsApproved == true).ToArrayAsync();

           List<GroupPayViewModel> groupPay = new List<GroupPayViewModel>();

            IEnumerable<decimal> Amount = _context.WorkDays.Include(x => x.Employee).Where(x => x.IsVacationDay == false && x.IsApproved == true).Select(items => GetRecord(items,items.Employee.Department)).Select(x => x.MoneyPaidToDepartment); // Overall pay
            
            int workdayCount = 0;

            WorkDay day = workDays.LastOrDefault();
            //WorkDay day = null;
            if(Amount != null)
            {
                for(int i = 0; i < departments.Length && i < Amount.Count() ; ++i)
                {
                    // if (workdayCount < workDays.Length && workDays[i] != null)
                    //{
                        workdayCount++;
                        // day = workDays[i];
                        groupPay.Add(new GroupPayViewModel { DepartmentId = departments[i].Id , DepartmentName = departments[i].Name, Date = day.Date,  MoneyPaidToDepartment = Amount.ToArray()[i] });  
                    //}
                    // var appUsers = departments[i].AssignedEmployees.Where();
                }
            }

            var groupPayOverall = new GroupPayOverallViewModel { GroupPayments = groupPay };


            return groupPayOverall;

        }

        public async Task<IEnumerable<WorkDay>> GetAllUnapprovedWorkDaysAsyncForDepartment(Guid departmentId)
        {

            var days = await _context.WorkDays
                .Where(x => x.IsApproved == false).Include(x => x.Employee).Where(x => x.Employee.DepartmentId.Equals(departmentId))
                .ToArrayAsync();
            return days;
        }

        public async Task<IEnumerable<WorkDay>> GetAllWorkDaysAsyncForDepartment(Guid departmentId)
        {

            var days = await _context.WorkDays
                .Include(x => x.Employee).Where(x => x.Employee.DepartmentId.Equals(departmentId))
                .ToArrayAsync();
            return days;
        }

        public async Task<IEnumerable<WorkDay>> GetUnapprovedWorkDaysAsync()
        {
            var days = await _context.WorkDays
                .Where(x => x.IsApproved == false).Include(x => x.Employee)
                .ToArrayAsync();
            return days;
        }

        public async Task<IEnumerable<WorkDay>> GetAllWorkDaysAsync(string userId)
        {
            var days = await _context.WorkDays.Where(x => x.EmployeeId == userId).Include(x => x.Employee)
               .ToArrayAsync();
            return days;
        }

        /*
         * From  https://stackoverflow.com/questions/794663/net-convert-number-to-string-representation-1-to-one-2-to-two-etc
         */
        public static string NumberToText(int n)
        {
            if (n < 0)
                return "Minus " + NumberToText(-n);
            else if (n == 0)
                return "Zero";
            else if (n <= 19)
                return new string[] {"One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight",
         "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
         "Seventeen", "Eighteen", "Nineteen"}[n - 1] + " ";
            else if (n <= 99)
                return new string[] {"Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy",
         "Eighty", "Ninety"}[n / 10 - 2] + " " + NumberToText(n % 10);
            else if (n <= 199)
                return "One Hundred " + NumberToText(n % 100);
            else if (n <= 999)
                return NumberToText(n / 100) + "Hundreds " + NumberToText(n % 100);
            else if (n <= 1999)
                return "One Thousand " + NumberToText(n % 1000);
            else if (n <= 999999)
                return NumberToText(n / 1000) + "Thousands " + NumberToText(n % 1000);
            else if (n <= 1999999)
                return "One Million " + NumberToText(n % 1000000);
            else if (n <= 999999999)
                return NumberToText(n / 1000000) + "Millions " + NumberToText(n % 1000000);
            else if (n <= 1999999999)
                return "One Billion " + NumberToText(n % 1000000000);
            else
                return NumberToText(n / 1000000000) + "Billions " + NumberToText(n % 1000000000);
        }

        public async Task<IEnumerable<CheckViewModel>> GetChecks()
        {
            var users =  _userManager.Users.ToArray(); 
            var workDays = await _context.WorkDays.ToArrayAsync();

            List<CheckViewModel> checkViews = new List<CheckViewModel>();
            
            for(int i = 0; i < users.Count(); i++)
            {
                
                checkViews.Add(new CheckViewModel
                {
                    EmployeeFirstName = users[i].FirstName,
                    EmployeeLastName = users[i].LastName,
                    Payment = workDays.Where(x => x.EmployeeId == users[i].Id && x.IsApproved == true).Sum(x => x.EstimatedPay),
                    Date = workDays.LastOrDefault().Date,
                    PaymentInMoney = NumberToText((int)workDays.Where(x => x.EmployeeId == users[i].Id && x.IsApproved == true).Sum(x => x.EstimatedPay))
                });
            }

            return checkViews; 
        }

        public async Task<IEnumerable<CheckViewModel>> GetChecksForDepartment(Guid departmentId)
        {
            var users = _userManager.Users.Where(x => x.DepartmentId == departmentId).ToArray();
            var workDays = await _context.WorkDays.ToArrayAsync();

            List<CheckViewModel> checkViews = new List<CheckViewModel>();

            for (int i = 0; i < users.Count(); i++)
            {

                checkViews.Add(new CheckViewModel
                {
                    EmployeeFirstName = users[i].FirstName,
                    EmployeeLastName = users[i].LastName,
                    Payment = workDays.Where(x => x.EmployeeId == users[i].Id).Sum(x => x.EstimatedPay),

                    PaymentInMoney = NumberToText((int)workDays.Where(x => x.EmployeeId == users[i].Id && x.IsApproved == true).Sum(x => x.EstimatedPay))
                });
            }

            return checkViews;
        }

        public IEnumerable<WorkDay> GetAllWithEmployees()
        {
            var days = _context.WorkDays.Include(x => x.Employee).ToArray();

            return days;
        }
    }
}
