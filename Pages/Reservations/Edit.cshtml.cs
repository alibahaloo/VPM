using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using VPM.Areas.Identity;
using VPM.Data.Entities;
using VPM.Services;

namespace VPM.Pages.Reservations
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ReservationService _reservationService;
        private readonly BuildingService _buildingService;
        [BindProperty]
        public Reservation Reservation { get; set; }
        public Building Building { get; set; }

        public EditModel(ReservationService reservationService, BuildingService buildingService)
        {
            _reservationService = reservationService;
            _buildingService = buildingService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {

            Reservation = await _reservationService.GetReservationAsync(id);

            //TODO: Move to model
            //Reservation.Date = Reservation.StartTime;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            /*
             * Reservation.Building is not lazyloaded! so we have to manually add it here
             * making it ready for the page ti be used (for input settings; min, max, etc)
             */
            Reservation.Building = await _buildingService.GetBuildingAsync(Reservation.BuildingId);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Reservation == null)
            {
                return NotFound();
            }

            bool isAuth = _reservationService.IsUserOwner(User.Identity.GetId(), Reservation);

            // Checking if the current user is the owner of the reservation
            if (isAuth || User.IsInRole("Admin"))
            {
                ServiceResult result = await _reservationService.UpdateReservationAsync(Reservation);

                if (!result.Success)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }
            else
            {
                return Forbid();
            }
            //return Page();
            return RedirectToPage("./Index");
        }
    }
}
