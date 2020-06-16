using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Data.Queries;
using VPM.Services;

namespace VPM.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly UserService _userService;
        private readonly ReservationService _reservationService;
        public IList<Reservation> Reservations { get; set; }
        [BindProperty] //Data has to be binded for the inputs to hold their state (this is only required if using Filter toolbar)
        public ReservationQuery Query { get; set; } = new ReservationQuery { };
        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }
        
        public DetailsModel(UserService userService, ReservationService reservationService)
        {
            _userService = userService;
            _reservationService = reservationService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // TODO: Admins can see every user's details
            //Managers can see details of users within the same building as them


            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser = await _userService.GetUserAsync(id);

            Reservations = ApplicationUser.Reservations; //Setting the default list, user's reservation
            Query.ApplicationUserEmail = ApplicationUser.Email; //Preparing Query for filter

            if (ApplicationUser == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //Application is binded, but reletions (e.g. building) are not, so load!
            ApplicationUser = await _userService.GetUserAsync(ApplicationUser.Id);
            Query.ApplicationUserEmail = ApplicationUser.Email;
            //Set the reservations as per the query
            Reservations = await _reservationService.GetReservationsAsync(Query);
            return Page();
        }
    }
}