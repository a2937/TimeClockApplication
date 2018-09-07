using TimeSheet.Models;
using TimeSheet.Models.Repos.Base;
using TimeSheet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheet.Models.Repos.Interfaces
{
    public interface IWorkDayRepo : IRepo<WorkDay>
    {
        Task<IEnumerable<WorkDay>> GetUnapprovedWorkDaysAsync();

        Task<IEnumerable<WorkDay>> GetAllWorkDaysAsyncForDepartment(Guid departmentId);

        Task<IEnumerable<WorkDay>> GetAllWorkDaysAsync(string userId);

        Task<IEnumerable<WorkDay>> GetApprovedWorkDaysAsync();

        Task<IEnumerable<Department>> GetDepartmentsAsync();

        Task<GroupPayOverallViewModel> GetGroupPaymentsAsync();

        Task<IEnumerable<CheckViewModel>> GetChecks();

        Task<IEnumerable<CheckViewModel>> GetChecksForDepartment(Guid departmentId);
        IEnumerable<WorkDay> GetAllWithEmployees();
        Task<IEnumerable<WorkDay>> GetAllUnapprovedWorkDaysAsyncForDepartment(Guid departmentId);
    }
}
