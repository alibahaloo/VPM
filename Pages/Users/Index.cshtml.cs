using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Data.Queries;
using VPM.Services;
using VPM.ViewComponents;

namespace VPM.Pages.Users
{
    [Authorize(Roles = "Admin, Manager")]
    public class IndexModel : PageModel
    {
        private readonly UserService _userService;

        public List<ApplicationUser> Users { get; set; } //Used to populate the list
        public SelectList BuildingOptions { get; set; } //Used to populate Building dropdownlist
        public Building Building { get; set; }
        public SelectList RoleOptions { get; set; } //Used to populate Role dropdownlist
        public IdentityRole Role { get; set; } //TODO: Shouldn't this be ApplicationRole?

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DisplayName("Email")]
            [DataType(DataType.EmailAddress)]
            public string UserEmail { get; set; }

            [Required]
            [DisplayName("Full Name")]
            public string UserFullName { get; set; }

            [Required]
            [DisplayName("Password")]
            [DataType(DataType.Password)]
            public string UserPassword { get; set; }

            [Required]
            [DisplayName("Unit")]
            public string Unit { get; set; }
            public Guid SelectedBuildingId { get; set; }
            public string SelectedRoleName { get; set; }
        }

        [BindProperty] //Data has to be binded for the inputs to hold their state (this is only required if using Filter toolbar)
        public UserQuery Query { get; set; } = new UserQuery { };
        public UserFilterInput FilterInput { get; set; }

        public IndexModel(UserService userService, RoleService roleService, BuildingService buildingService)
        {
            _userService = userService;

            BuildingOptions = new SelectList(buildingService.GetBuildings(), nameof(Building.Id), nameof(Building.Name));
            RoleOptions = new SelectList(roleService.GetRoles(), nameof(Role.Name), nameof(Role.Name));

            //THIS IS WIERD!
            //Users = _userService.GetApplicationUsersAsync().Result;

            FilterInput = new UserFilterInput
            {
                ByBuilding = true,
                ByFullName = true,
                ByEmail = true,
                ByRole = true,
                ByUnit = true
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Users = await _userService.GetUsersAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Query == new UserQuery { })
            {
                var msg = 0;
            }

            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser { 
                    UserName = Input.UserEmail, 
                    Email = Input.UserEmail, 
                    FullName = Input.UserFullName, 
                    Unit = Input.Unit, 
                    BuildingId = Input.SelectedBuildingId,
                    Password = Input.UserPassword,
                };

                var result = await _userService.CreateUserAsync(applicationUser);

                if (result.Succeeded)
                {
                    await _userService.AddUserToRoleAsync(applicationUser, Input.SelectedRoleName);

                } else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            //Used to populate the list before loading the page
            Users = await _userService.GetUsersAsync(Query);
            return RedirectToPage();
        }
    }
}