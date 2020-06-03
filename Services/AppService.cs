using Microsoft.CodeAnalysis;
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
        private readonly UserService _userService;
        private readonly BuildingService _buildingService;
        private readonly ApplicationDbContext _context;

        private readonly List<string> _errors = new List<string> { };

        public AppService(UserService userService, BuildingService buildingService, ApplicationDbContext context)
        {
            _userService = userService;
            _buildingService = buildingService;

            _context = context;
        }

        public ServiceResult IsReadyToInitialize()
        {
            try
            {
                var userResult = _userService.CheckNeedForDefaultUser();
                var buildingResult = _buildingService.CheckNeedForDefaultBuilding();

                if ((userResult.Errors != null) && (buildingResult.Errors != null))
                {
                    foreach (var error in userResult.Errors)
                    {
                        _errors.Add(error);
                    }

                    foreach (var error in buildingResult.Errors)
                    {
                        _errors.Add(error);
                    }

                }

                if ((!userResult.Success) || (!buildingResult.Success))
                    return new ServiceResult { Success = false, Errors = _errors };

                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                _errors.Add(ex.Message);
                //Probably the migrations haven't happend yet
                return new ServiceResult { Success = false, Errors = _errors };
            }

        }

        public async Task<ServiceResult> AddDefaultData(string adminEmail, string adminPassword, string masterPassword)
        {
            //TODO: store master password somwhere secure
            if (masterPassword != "master")
            {
                _errors.Add("incorrect master password");
                return new ServiceResult { Success = false, Errors = _errors };
            }

            //Add Default Building
                await _buildingService.CreateBuildingAsync(new Building {
                Name = "Default",
                Address = "Planet Earth",
                AllowedReservationLength = 2,
                AllowedReservationPerUnit = 1,
                VisitorParkingCount = 5,
                ReservationInterval = AllowedReservationIntervals.FH,
                IsActive = true,
            });

            Building defaultBuilding = await _buildingService.GetDefaultBuildingAsync();

            await _userService.CreateUserAsync(new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Super Admin",
                Unit = "69",
                BuildingId = defaultBuilding.Id,
                Password = adminPassword,
            });

            //Add Default User to Role admin
            ApplicationUser defaultUser = await _userService.GetDefaultUserAsync();
            await _userService.AddUserToRoleAsync(defaultUser, "Admin");

            return new ServiceResult { Success = true };
        }
    }
}
