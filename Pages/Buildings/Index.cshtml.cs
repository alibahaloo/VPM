using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Services;

namespace VPM.Pages.Buildings
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly BuildingService _buildingService;

        public IndexModel(BuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        public IList<Building> Buildings { get;set; }

        public async Task OnGetAsync()
        {
            Buildings = await _buildingService.GetBuildingsAsync();
        }
    }
}
