using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Models;

namespace TimeSheet.Controllers
{
    public class HomeController : Controller
    {

        private UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        private Task<ApplicationUser> CurrentUser => _userManager.FindByNameAsync(HttpContext.User.Identity.Name);


        public ViewResult About() => View();

        [Authorize]
        public IActionResult Index() => View(GetData(nameof(Index)));

        [Authorize(Roles = "Users")]
        public IActionResult OtherAction() => View("Index", GetData(nameof(OtherAction)));

        private Dictionary<string, object> GetData(string actionName) => new Dictionary<string, object>
        {
            ["Action"] = actionName,
            ["User"] = HttpContext.User.Identity.Name,
            ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
            ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
            ["Is in PayrollSupervisorsRole"] = HttpContext.User.IsInRole("PayrollSupervisors"),
            ["FirstName"] = CurrentUser.Result.FirstName,
            ["LastName"] = CurrentUser.Result.LastName,
            ["HourlyWage"] = CurrentUser.Result.HourlyWage,
            ["ExceptFromOverTime"] = CurrentUser.Result.ExceptFromOvertime,
            ["DepartmentId"] = CurrentUser.Result.DepartmentId
        };

        [Authorize]
        public async Task<IActionResult> UserProps()
        {
            return View(await CurrentUser);
        }

     
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserProps([Required]decimal HourlyWage  )
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = await CurrentUser;
                user.HourlyWage = HourlyWage;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            return View(await CurrentUser);
        }
       
    }
}
