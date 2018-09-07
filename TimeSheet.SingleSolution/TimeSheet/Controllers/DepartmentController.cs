using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Models;
using TimeSheet.Models.Repos.Interfaces;
using TimeSheet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Controllers
{
    [Authorize(Roles ="Supervisors,PayrollSupervisors,Admins")]
    public class DepartmentController : Controller
    {
        private IDepartmentRepo _departmentRepo { get; set; }
        private UserManager<ApplicationUser> _userManager;
        private ISupervisorRepo _supervisorRepo;

        public DepartmentController(IDepartmentRepo departmentRepo, UserManager<ApplicationUser> userManager, ISupervisorRepo supervisorRepo)
        {
            _departmentRepo = departmentRepo;
            _userManager = userManager;
            _supervisorRepo = supervisorRepo;
        }

        public ViewResult Create() => View();


        [HttpPost]
        public IActionResult Create(Department depo)
        {
            if (ModelState.IsValid)
            {
                int result = _departmentRepo.Add(depo);
                if (result != 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(depo);
        }


        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            Department depo = _departmentRepo.Find(id);
            if (depo != null)
            {
                int result = _departmentRepo.Delete(depo);
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
                ModelState.AddModelError("", "No department found");
            }
            return View("Index", _departmentRepo.GetAll());
        }

        public IActionResult Edit(Guid id)
        {
            Department depo = _departmentRepo.Find(id);

            List<ApplicationUser> members = new List<ApplicationUser>();

            List<ApplicationUser> nonMembers = new List<ApplicationUser>();

            foreach (ApplicationUser user in _userManager.Users)
            {
                if (user.DepartmentId == depo.Id)
                {
                    members.Add(user);
                }
                else
                {
                    nonMembers.Add(user);
                }
                //var list = (user,depo.Name) ? members : nonMembers;
                //list.Add(user);
            }
            return View(new DepartmentAndUserViewModel
            {
                Department = depo,
                DepartmentId = depo.Id,
                SupervisorId = depo.SupervisorId,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentEditViewModel model)
        {
            
            if (ModelState.IsValid)
            {

                Supervisor supervisor = _supervisorRepo.GetAll().Where(x => x.EmployeeId == model.NewSuperVisorEmployeeId).FirstOrDefault();
                Department department = _departmentRepo.GetAllWithUsers().Where(x => x.Id == model.DepartmentId).FirstOrDefault();
                
                if (supervisor == null && model.NewSuperVisorEmployeeId != null && !model.NewSuperVisorEmployeeId.Equals(""))
                {
                    List<Department> departments = new List<Department>
                    {
                        department
                    };
                    if(_userManager.FindByIdAsync(model.NewSuperVisorEmployeeId) == null)
                    {
                        throw new Exception("Department Controller could not find user with id of " + model.NewSuperVisorEmployeeId);
                    }
                   
                    _supervisorRepo.Add(new Supervisor { EmployeeId = model.NewSuperVisorEmployeeId });
                    _supervisorRepo.SaveChanges();
                    
                    Supervisor SavedSupervisor = _supervisorRepo.GetAll().Where(x => x.EmployeeId == model.NewSuperVisorEmployeeId).FirstOrDefault();

                    department.SupervisorId = SavedSupervisor.Id;
                    _departmentRepo.SaveChanges();
                }
                else if(supervisor != null)
                {
                    // supervisor.Departments.Add(department);
                    department.SupervisorId = supervisor.Id;
                    _supervisorRepo.SaveChanges();
                }
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        //department.AssignedEmployees.Add(user);
                        user.DepartmentId = department.Id;
                        IdentityResult result = await _userManager.UpdateAsync(user);
                        //result = _departmentRepo.SaveChanges();
                        
                       // result = _departmentRepo.AddToDepartment(user, model.DepartmentId);//_.AddToRoleAsync(user, model.RoleName);
                        
                       
                        if (!result.Succeeded)
                         {
                        AddErrorsFromResult(result);
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        //_departmentRepo.Add(department);
                        _departmentRepo.SaveChanges();
                        //return RedirectToAction("Index");
                    }
                }

                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        //department.AssignedEmployees.Remove(user);
                        user.DepartmentId = null;
                        if(supervisor != null)
                        {
                            if(user.Id == supervisor.EmployeeId)
                            {
                                Supervisor newSupervisor = new Supervisor { EmployeeId = department.AssignedEmployees.FirstOrDefault().Id };
                                _supervisorRepo.SaveChanges();
                                department.SupervisorId = newSupervisor.Id;
                            }
                            IdentityResult result = await _userManager.UpdateAsync(user);
                            //result = _departmentRepo.RemoveFromDepartment(user, model.DepartmentId);//_.RemoveFromRoleAsync(user, model.RoleName);
                            if (!result.Succeeded)
                            {
                                AddErrorsFromResult(result);
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Edit(model.DepartmentId);
            }
        }




        public ViewResult Index() => View(_departmentRepo.GetAllWithUsers());

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        /*
         * Service Level Methods
         * This is for an API controller
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_departmentRepo.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var item = _departmentRepo.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Json(item);
        }

        [HttpGet("{departmentId}/users")]
        public IEnumerable<DepartmentAndUserViewModel> GetUsersForDepartment(Guid departmentId)
         => _departmentRepo.GetUsersWithDepartment(departmentId).ToList();

        */

    }
}
