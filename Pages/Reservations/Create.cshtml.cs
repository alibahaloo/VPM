using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using VPM.Areas.Identity;
using VPM.Data.Entities;
using VPM.Services;

namespace VPM.Pages.Reservations
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ReservationService _reservationService;
        private readonly BuildingService _buildingService;

        [BindProperty]
        public Reservation Reservation { get; set; }
        public Building Building { get; set; }

        public CreateModel(ReservationService reservationService, BuildingService buildingService)
        {
            _reservationService = reservationService;
            _buildingService = buildingService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Building = await _buildingService.GetBuildingAsync(User.Identity.GetBuildingId());

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            //Used for inputs
            Building = await _buildingService.GetBuildingAsync(User.Identity.GetBuildingId());

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Preparing reservation object
            Reservation.ApplicationUserId = User.Identity.GetId();
            Reservation.BuildingId = User.Identity.GetBuildingId();

            //Use the service to save reservation (pass prepared object)
            ServiceResult result = await _reservationService.CreateReservationAsync(Reservation);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return Page();

            }

            return RedirectToPage("./Index");
        }
    }
}
