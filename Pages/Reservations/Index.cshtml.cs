using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPM.Areas.Identity;
using VPM.Data.Entities;
using VPM.Data.Queries;
using VPM.Services;

namespace VPM.Pages.Reservations
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ReservationService _reservationService;
        public ReservationQuery Query { get; set; } = new ReservationQuery { };

        public IndexModel(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public IList<Reservation> Reservations { get;set; }

        public async Task OnGetAsync()
        {
            if (User.IsInRole("Manager"))
                Query.BuildingId = User.Identity.GetBuildingId();

            if (User.IsInRole("Resident"))
                Query.ApplicationUserId = User.Identity.GetId();

            Reservations = await _reservationService.GetReservationsAsync(Query);

        }
    }
}
