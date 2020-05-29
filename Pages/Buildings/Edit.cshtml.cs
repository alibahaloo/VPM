using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Services;

namespace VPM.Pages.Buildings
{
    [Authorize(Roles = "Admin,Manager")]
    public class EditModel : PageModel
    {
        private readonly BuildingService _buildingService;

        public EditModel(BuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [BindProperty]
        public Building Building { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Building = await _buildingService.GetBuildingAsync(id);

            if (Building == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Building = await _buildingService.GetBuildingAsync(Building.Id);

            if (Building == null)
            {
                return NotFound();
            }

            await _buildingService.UpdateBuildingAsync(Building);

            return RedirectToPage("./Index");
        }
    }
}
