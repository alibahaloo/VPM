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
    public class DeleteModel : PageModel
    {
        private readonly ReservationService _reservationService;
        private readonly UserService _ApplicationUserService;

        public DeleteModel(ReservationService reservationService, UserService ApplicationUserService)
        {
            _reservationService = reservationService;
            _ApplicationUserService = ApplicationUserService;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation = await _reservationService.GetReservationAsync(id);
            bool isAuth = _reservationService.IsUserOwner(User.Identity.GetId(), Reservation);

            if (!isAuth)
            {
                return Forbid();
            }

            await _reservationService.DeleteReservationAsync(id);

            return RedirectToPage("./Index");
        }
    }
}
