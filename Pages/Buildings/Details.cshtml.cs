using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Data.Queries;
using VPM.Services;

namespace VPM.Pages.Buildings
{
    [Authorize(Roles = "Admin,Manager,Resident")]
    public class DetailsModel : PageModel
    {
        private readonly BuildingService _buildingService;
        private readonly ReservationService _reservationService;

        [BindProperty]
        public Building Building { get; set; }
        public IList<Reservation> Reservations { get; set; }

        [BindProperty] //Data has to be binded for the inputs to hold their state (this is only required if using Filter toolbar)
        public ReservationQuery Query { get; set; } = new ReservationQuery { };

        public DetailsModel(BuildingService buildingService, ReservationService reservationService)
        {
            _buildingService = buildingService;
            _reservationService = reservationService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Building = await _buildingService.GetBuildingAsync(id);
            //Setting the building Id in the query since we're looking at a specific building page
            Query.BuildingId = id;
            Reservations = await _reservationService.GetReservationsAsync(Query);

            if (Building == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //Building is binded, so find the ID, then full get object
            Building = await _buildingService.GetBuildingAsync(Building.Id);

            Query.BuildingId = Building.Id;

            Reservations = await _reservationService.GetReservationsAsync(Query);
            return Page();
        }
    }
}
