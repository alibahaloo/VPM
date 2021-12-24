using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Vereyon.Web;
using VPM.Data.Entities;
using VPM.Services;

namespace VPM.Pages.Buildings
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly BuildingService _buildingService;
        private readonly IFlashMessage _flashMessage;
        public CreateModel(BuildingService buildingService, IFlashMessage flashMessage)
        {
            _buildingService = buildingService;
            _flashMessage = flashMessage;
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

            // Validate Inputs; AllowedReservationPerUnit Vs VisitorParkingCount
            // AllowedReservationPerUnit cannot be more than VisitorParkingCount
            if (Building.AllowedReservationPerUnit > Building.VisitorParkingCount)
            {
                _flashMessage.Danger("Invalid Input; Allowed Reservations Per Unit cannot be more than the Total Number of Available Parkings");
                return Page();
            }

            await _buildingService.CreateBuildingAsync(Building);

            return RedirectToPage("./Index");
        }
    }
}
