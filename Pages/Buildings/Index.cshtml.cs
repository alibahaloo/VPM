using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPM.Areas.Identity;
using VPM.Data.Entities;
using VPM.Data.Queries;
using VPM.Services;

namespace VPM.Pages.Buildings
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly BuildingService _buildingService;

        [BindProperty] //Data has to be binded for the inputs to hold their state (this is only required if using Filter toolbar)
        public BuildingQuery Query { get; set; } = new BuildingQuery { };
        public IList<Building> Buildings { get; set; }
        public IndexModel(BuildingService buildingService)
        {
            _buildingService = buildingService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.IsInRole("Admin")) //non-admins must redirect to their own building page
                RedirectToPage("./Details?Id={0}", User.Identity.GetBuildingId());

            Buildings = await _buildingService.GetBuildingsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //Filtering buildings based on the passed query
            Buildings = await _buildingService.GetBuildingsAsync(Query);
            return Page();
        }
    }
}
