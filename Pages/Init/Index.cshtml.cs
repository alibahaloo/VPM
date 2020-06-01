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

            [Required]
            [DisplayName("Master Password")]
            [DataType(DataType.Password)]
            public string MasterPassword { get; set; }
        }
        private readonly AppService _appService;

        public bool Error { get; set; }

        public IndexModel(AppService appService)
        {
            _appService = appService;
        }

        public IActionResult OnGet()
        {
            //Check if there's a default user, then go to home page
            var result = _appService.IsReadyToInitialize();

            if (!result.Success)
            {
                Error = true;
                ModelState.AddModelError(string.Empty, result.Data.ToString());
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Input.MasterPassword != "master")
            {
                ModelState.AddModelError(string.Empty, "incorrect master password");
                return Page();
            }


            await _appService.AddInitData();
            return RedirectToPage();
        }
    }
}