using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPM.Data;

namespace VPM.Services
{
    public class RoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly List<string> _errors = new List<string> { };


        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public List<ApplicationRole> GetRoles()
        {
            return _roleManager.Roles?.ToList();
        }

        public async Task CreateRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                var role = new ApplicationRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = roleName
                };

                await _roleManager.CreateAsync(role);
            }
        }

        public async Task<ServiceResult> DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                if (role.Name == "Admin")
                {
                    _errors.Add("Cannot delete Admin role.");
                    return new ServiceResult { Success = false, Errors = _errors };
                }

                try
                {
                    await _roleManager.DeleteAsync(role);
                    return new ServiceResult { Success = true };
                }
                catch (Exception)
                {
                    throw;
                }
               
            }

            _errors.Add("Role not found");
            return new ServiceResult { Success = false, Errors = _errors };
        }

        public async Task<ApplicationRole> GetRoleAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }
    }
}
