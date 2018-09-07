using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TimeSheet.Models;
using TimeSheet.Models.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Infrastructure
{
    [HtmlTargetElement("td",Attributes ="user-department")]
    public class DepartmentEmployeesTagHelper : TagHelper
    {
        private UserManager<ApplicationUser> _userManager;

        private IDepartmentRepo _departmentRepo;

        public DepartmentEmployeesTagHelper(UserManager<ApplicationUser> userManager,IDepartmentRepo departmentRepo)
        {
            _userManager = userManager;
            _departmentRepo = departmentRepo;
        }

        [HtmlAttributeName("user-department")]
        public Guid Department { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();

            Department department = await _departmentRepo.FindById(Department);

            if(department != null)
            {
                foreach(var user in _userManager.Users)
                {
                    if(user != null &&  _departmentRepo.UserIsInDepartment(user,department.Id))
                    {
                        names.Add(user.UserName);
                    }
                }
            }
            output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(" , ", names));
        }
    }
}
