using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VPM.Services;

namespace VPM.Pages.Init
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [DisplayName("Email")]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required]
            [DisplayName("Full Name")]
            public string FullName { get; set; }

            [Required]
            [DisplayName("Password")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
        private readonly AppService _appService;

        public IndexModel(AppService appService)
        {
            _appService = appService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _appService.AddInitData();
            return RedirectToPage();
        }
    }
}