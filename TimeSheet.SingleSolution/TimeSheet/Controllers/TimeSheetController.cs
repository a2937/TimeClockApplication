using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Models;
using TimeSheet.Models.Repos.Interfaces;
using TimeSheet.Models.ViewModels;
using TimeSheet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Controllers
{
    [Authorize(Roles = "PayrollSupervisors,Managers")]
    public class TimeSheetController : Controller
    {
        private readonly IWorkDayRepo _timeSheetService;
        private readonly IDepartmentRepo _departmentRepo;
        private readonly ISupervisorRepo _supervisorRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TimeSheetController(IWorkDayRepo timeSheetService, IDepartmentRepo departmentRepo, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ISupervisorRepo supervisorRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _supervisorRepo = supervisorRepo;
            _timeSheetService = timeSheetService;
            _departmentRepo = departmentRepo;
        }

        public async Task<IActionResult> Index()
        {


            IEnumerable<WorkDay> days = new List<WorkDay>();

            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            Supervisor supervisor = _supervisorRepo.GetAll().Where(x => x.EmployeeId == user.Id).FirstOrDefault();
            IList<string> role = await _userManager.GetRolesAsync(user);

            if (role.Contains("Managers"))
            {
                Department department = _departmentRepo.GetAllWithUsers().Where(x => x.SupervisorId == supervisor.Id).FirstOrDefault();
                days = await _timeSheetService.GetAllUnapprovedWorkDaysAsyncForDepartment(department.Id);
            }
            else
            {
                days = await _timeSheetService.GetUnapprovedWorkDaysAsync();
            }
            WorkDayViewModel viewModel = new WorkDayViewModel
            {
                workDays = days
            };
            return View(viewModel);
        }

        public async Task<IActionResult> ReviewAll()
        {
            IEnumerable<WorkDay> days = new List<WorkDay>();

            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            Supervisor supervisor = _supervisorRepo.GetAll().Where(x => x.EmployeeId == user.Id).FirstOrDefault();
            IList<string> role = await _userManager.GetRolesAsync(user);

            if (role.Contains("Managers"))
            {
                Department department = _departmentRepo.GetAllWithUsers().Where(x => x.SupervisorId == supervisor.Id).FirstOrDefault();
                days = await _timeSheetService.GetAllWorkDaysAsyncForDepartment(department.Id);
            }
            else
            {
                days = _timeSheetService.GetAll();
            }
            WorkDayViewModel viewModel = new WorkDayViewModel
            {
                workDays = days
            };
            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            WorkDay day = _timeSheetService.Find(id);
            if (day != null)
            {
                int result = _timeSheetService.Delete(day);
                if (result != 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    //AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "No work day found");
            }
            return View("Index", _timeSheetService.GetAll());
        }

        [Authorize(Roles = "PayrollSupervisors")]
        public async Task<IActionResult> GroupPay()
        {
            var model = await _timeSheetService.GetGroupPaymentsAsync();


            return View(model);
        }

        [Authorize(Roles = "PayrollSupervisors")]
        public async Task<IActionResult> Check()
        {
            var model = await _timeSheetService.GetChecks();
            return View(model);
        }

        public IActionResult Edit(Guid id)
        {
            WorkDay day = _timeSheetService.Find(id);
            return View(day);
        }

        [HttpPost]
        public IActionResult Edit(string Id, string EmployeeId, decimal EstimatedPay, int HoursWorked, string Date,bool IsVacationDay, string SignIn, string SignOut, bool IsApproved, string Reason)
        // public IActionResult Edit(WorkDay model)
        {

            WorkDay model = _timeSheetService.Find(Guid.Parse(Id));
            if (model != null)
            {
                model.EmployeeId = EmployeeId;

                model.Date = DateTime.Parse(Date);
                //model.Date = Date;

                model.IsVacationDay = IsVacationDay;

                model.SignIn = DateTime.Parse(SignIn);
                //model.SignIn = SignIn;

                model.SignOut = DateTime.Parse(SignOut);
               // model.SignOut = SignOut;

                model.IsApproved = IsApproved;
                model.Reason = Reason;
                int result = _timeSheetService.SaveChanges();

                if (result != 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Console.Out.WriteLine("Not saving.");
                    return Edit(model.Id);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
    }
}
