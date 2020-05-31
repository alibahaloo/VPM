using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPM.Data;
using VPM.Data.Entities;

namespace VPM.Services
{
    public class AppService
    {
        private readonly RoleService _roleService;
        private readonly UserService _userService;
        private readonly BuildingService _buildingService;

        public AppService(RoleService roleService, UserService userService, BuildingService buildingService)
        {
            _roleService = roleService;
            _userService = userService;
            _buildingService = buildingService;
        }

        public async Task AddInitData()
        {
            //Add Admin Role
            //await _roleService.CreateRoleAsync("Admin");

            //Add Default Building
            await _buildingService.CreateBuildingAsync(new Building {
                Name = "Default",
                Address = "Planet Earth",
                AllowedReservationLength = 2,
                AllowedReservationPerUnit = 1,
                VisitorParkingCount = 5,
                ReservationInterval = AllowedReservationIntervals.FH
            });

            Building defaultBuilding = await _buildingService.GetDefaultBuildingAsync();

            var result = await _userService.CreateUserAsync(new ApplicationUser
            {
                UserName = "admin@vpm-app.com",
                Email = "admin@vpm-app.com",
                FullName = "Super Admin",
                Unit = "69",
                BuildingId = defaultBuilding.Id,
                Password = "P@ssw0rd",
            });

            //Add Default User to Role admin
            ApplicationUser defaultUser = await _userService.GetDefaultUserAsync();
            await _userService.AddUserToRoleAsync(defaultUser, "Admin");
        }
    }
}
