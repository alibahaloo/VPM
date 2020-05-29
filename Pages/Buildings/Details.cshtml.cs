using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Services;

namespace VPM.Pages.Buildings
{
    [Authorize(Roles = "Admin,Manager,Resident")]
    public class DetailsModel : PageModel
    {
        private readonly BuildingService _buildingService;
        private readonly ReservationService _reservationService;

        public IList<Reservation> Reservations { get; set; }

        public DetailsModel(BuildingService buildingService, ReservationService reservationService)
        {
            _buildingService = buildingService;
            _reservationService = reservationService;
        }

        public Building Building { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Building = await _buildingService.GetBuildingAsync(id);
            Reservations = await _reservationService.GetReservationsByBuildingAsync(id);

            if (Building == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
