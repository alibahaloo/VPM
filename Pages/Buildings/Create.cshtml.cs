using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Services;

namespace VPM.Pages.Buildings
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly BuildingService _buildingService;
        public CreateModel(BuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Building Building { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _buildingService.CreateBuildingAsync(Building);

            return RedirectToPage("./Index");
        }
    }
}
