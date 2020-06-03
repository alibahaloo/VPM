using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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
            [DisplayName("Password")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DisplayName("Master Password")]
            [DataType(DataType.Password)]
            public string MasterPassword { get; set; }
        }
        private readonly AppService _appService;

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
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _appService.AddDefaultData(Input.Email,Input.Password, Input.MasterPassword);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return Page();
            }

            //Refreshes the page, which redirects to the error .. may be better to 
            return RedirectToPage("../Init/Success");
        }
    }
}