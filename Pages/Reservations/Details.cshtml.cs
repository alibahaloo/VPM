using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Services;

namespace VPM.Pages.Reservations
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ReservationService _reservationService;

        public DetailsModel(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public Reservation Reservation { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation = await _reservationService.GetReservationAsync(id);

            if (Reservation == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
