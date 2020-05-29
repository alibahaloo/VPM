using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Services;

namespace VPM.Interfaces
{
    public interface IApplicationUserService
    {
        //get building
        //get reservations

        public Task<ServiceResult> RegisterApplicationUser(ApplicationUser ApplicationUser);
        public Task<ServiceResult> DeleteApplicationUser(ApplicationUser ApplicationUser);
        public Task<ServiceResult> UpdateApplicationUser(ApplicationUser ApplicationUser);
        public Task<ServiceResult> GetApplicationUser(string ApplicationUserId);
    }
}
