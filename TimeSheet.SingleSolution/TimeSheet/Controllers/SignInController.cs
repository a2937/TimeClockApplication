using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Models;
using TimeSheet.Models.Repos.Interfaces;
using TimeSheet.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeSheet.Controllers
{
    [Authorize]
    public class SignInController : Controller
    {
        private readonly IWorkDayRepo _timeSheetService;
        private UserManager<ApplicationUser> _userManager;

    
        public SignInController(IWorkDayRepo timeSheetService, UserManager<ApplicationUser> userManager)
        {
            _timeSheetService = timeSheetService;
            _userManager = userManager;
        }

        public async Task<ViewResult> Create()
        {
            var userName = User.Identity.Name;
            ApplicationUser applicationUser = await _userManager.FindByNameAsync(userName);
            ViewBag.Id = applicationUser.Id;
            return View();
        }

        [HttpPost]
        public IActionResult Create(WorkDay day)
        {
            if (ModelState.IsValid)
            {
                int result = _timeSheetService.Add(day);
                if (result != 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(day);
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

        public async Task<ViewResult> Index()
        {
            var userName = User.Identity.Name;
            ApplicationUser applicationUser = await _userManager.FindByNameAsync(userName);
             IEnumerable<WorkDay> workDays = _timeSheetService.GetAllWorkDaysAsync(applicationUser.Id).Result;
           
            return View(workDays);
        }


        public IActionResult Edit(Guid id)
        {
            WorkDay day = _timeSheetService.Find(id);
            return View(day);
        }

        [HttpPost]
        public IActionResult Edit(string Id, string EmployeeId, decimal EstimatedPay, int HoursWorked,string Date, bool IsVacationDay, string SignIn, string SignOut)
       // public IActionResult Edit(WorkDay model)
        {
            
            WorkDay model = _timeSheetService.Find(Guid.Parse(Id));
            if(model != null)
            {
                model.EmployeeId = EmployeeId;
                model.Date = DateTime.Parse(Date);
                model.IsVacationDay = IsVacationDay;
                model.SignIn = DateTime.Parse(SignIn);
                model.SignOut = DateTime.Parse(SignOut);
                
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
            
            
            /*
            if (ModelState.IsValid)
            {
    
                int result = _timeSheetService.SaveChanges();

                if (result != 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Edit(model.Id);
                }

            }
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Edit(model.Id);
            }
            */
            
        }

    }
}
