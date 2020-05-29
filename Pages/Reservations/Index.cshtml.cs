using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPM.Areas.Identity;
using VPM.Data.Entities;
using VPM.Services;

namespace VPM.Pages.Reservations
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ReservationService _reservationService;

        public IndexModel(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public IList<Reservation> Reservations { get;set; }

        public async Task OnGetAsync()
        {

            if (User.IsInRole("Admin"))
            {
                Reservations = await _reservationService.GetReservationsAsync();
            } 
            else if (User.IsInRole("Manager"))
            {
                Reservations = await _reservationService.GetReservationsByBuildingAsync(User.Identity.GetBuildingId());
            }
            else if (User.IsInRole("Resident"))
            {
                Reservations = await _reservationService.GetReservationsByUserAsync(User.Identity.GetId());
            }

        }
    }
}
