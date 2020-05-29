using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VPM.Data;
using VPM.Services;

namespace VPM.Pages.Roles
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly RoleService _roleService;

        public IndexModel(RoleService roleService)
        {
            _roleService = roleService;

            Roles = roleService.GetRoles();

        }

        //Do validation
        [BindProperty]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }
        public List<ApplicationRole> Roles { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _roleService.CreateRoleAsync(RoleName);

            return RedirectToPage("./Index");
        }
    }
}