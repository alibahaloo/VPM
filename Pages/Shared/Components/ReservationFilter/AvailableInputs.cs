using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPM.Pages.Shared.Components.ReservationFilter
{
    public class AvailableInputs
    {
        public bool ByValidity { get; set; }
        public bool ByUnit { get; set; }
        public bool ByVehiclePlateNumber { get; set; } = false;
        public bool ByBuilding { get; set; }
    }
}
