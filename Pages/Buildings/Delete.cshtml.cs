using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Services;

namespace VPM.Pages.Buildings
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly BuildingService _buildingService;

        public DeleteModel(BuildingService buildingService)
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

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Building = await _buildingService.GetBuildingAsync(id);

            if (Building != null)
            {
                await _buildingService.DeleteBuildingAsync(id);
            }

            return RedirectToPage("./Index");
        }
    }
}
