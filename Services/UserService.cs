using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPM.Data;
using VPM.Data.Entities;

namespace VPM.Services
{
    public class UserService /*: IApplicationUserService*/
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleService _roleService;
        public UserService(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            RoleService roleService)
        {
            _context = context;
            _userManager = userManager;
            _roleService = roleService;
        }

        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            return await _userManager.Users?.Include(u => u.Building).ToListAsync();
        }

        public List<ApplicationUser> GetUsers()
        {
            return _userManager.Users?.Include(u => u.Building).ToList();
        }

        public Task<ServiceResult> DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetDefaultUserAsync()
        {
            return await _context.Users.FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> GetUserAsync(string userId)
        {
            try
            {
                return await _context.Users
                    .Include(b => b.Building)
                    .Include(b => b.Reservations)
                    .FirstOrDefaultAsync(r => r.Id == userId);                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user)
        {
            return await _userManager.CreateAsync(user, user.Password);
        }

        public async Task AddUserToRoleAsync(ApplicationUser user, string rolename)
        {
            await _roleService.CreateRoleAsync(rolename);

            await _userManager.AddToRoleAsync(user, rolename);
        }

        public async Task UpdateUserAsync(ApplicationUser applicationUser)
        {
            try
            {
                _context.Attach(applicationUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task UpdateUserRoleAsync(ApplicationUser user, string rolename)
        {

            var roles = await _userManager.GetRolesAsync(user);
            //Removing user from current roles
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
            //Adding user to the new role
            await AddUserToRoleAsync(user, rolename);
        }
    }
}
