using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VPM.Areas.Identity;
using VPM.Data;
using VPM.Data.Entities;
using VPM.Services;

namespace VPM.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly UserService _userService;

        public SelectList BuildingOptions { get; set; } //Used to populate Building dropdownlist
        public Building Building { get; set; }
        public SelectList RoleOptions { get; set; } //Used to populate Role dropdownlist
        public ApplicationRole Role { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [DisplayName("Unit")]
            public string Unit { get; set; }
            public Guid SelectedBuildingId { get; set; }
            public string SelectedRoleName { get; set; }
        }
        public EditModel(UserService userService, RoleService roleService, BuildingService buildingService)
        {
            _userService = userService;

            BuildingOptions = new SelectList(buildingService.GetBuildings(), nameof(Building.Id), nameof(Building.Name));
            RoleOptions = new SelectList(roleService.GetRoles(), nameof(Role.Name), nameof(Role.Name));
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser = await _userService.GetUserAsync(id);

            if (ApplicationUser == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApplicationUser = await _userService.GetUserAsync(id);

            ApplicationUser.BuildingId = Input.SelectedBuildingId;
            ApplicationUser.Unit = Input.Unit;

            await _userService.UpdateUserAsync(ApplicationUser);

            //ONLY POSSIBLE TO CHANGE USER'S ROLE
            await _userService.UpdateUserRoleAsync(ApplicationUser, Input.SelectedRoleName);

            return RedirectToPage("../Users/Index");

        }
    }
}