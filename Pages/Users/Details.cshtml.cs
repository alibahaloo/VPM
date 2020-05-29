using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VPM.Data.Entities;
using VPM.Services;

namespace VPM.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly UserService _userService;

        public DetailsModel(UserService userService)
        {
            _userService = userService;
        }

        public ApplicationUser ApplicationUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser = await _userService.GetUserAsync(id);

            if(ApplicationUser == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}