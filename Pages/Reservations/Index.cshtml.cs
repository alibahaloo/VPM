using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [BindProperty] //Data has to be binded for the inputs to hold their state (this is only required if using Filter toolbar)
        public ReservationQuery Query { get; set; } = new ReservationQuery { };

        public IndexModel(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public IList<Reservation> Reservations { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.IsInRole("Manager"))
                Query.BuildingId = User.Identity.GetBuildingId(); //So that manager can only query his own building

            if (User.IsInRole("Resident"))
                Query.ApplicationUserEmail = User.Identity.Name; //So that resident can only query his own reservations

            Reservations = await _reservationService.GetReservationsAsync(Query);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User.IsInRole("Manager"))
                Query.BuildingId = User.Identity.GetBuildingId(); //So that manager can only query his own building

            if (User.IsInRole("Resident"))
                Query.ApplicationUserEmail = User.Identity.Name; //So that resident can only query his own reservations

            Reservations = await _reservationService.GetReservationsAsync(Query);
            return Page();
        }
    }
}
