using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace VPM.ViewComponents
{
    public class FilterInput
    {
        public bool ByCreated { get; set; } = false;
    }

    public class ReservationFilterInput : FilterInput
    {
        public bool ByValidity { get; set; } = false;
        public bool ByUnit { get; set; } = false;
        public bool ByVehiclePlateNumber { get; set; } = false; 
        public bool ByBuilding { get; set; } = false; 
        public bool ByDate { get; set; } = false; 
        public bool ByUser { get; set; } = false; 
    }

    public class BuildingFilterInput : FilterInput
    {
        public bool ByName { get; set; } = false;
        public bool ByAddress { get; set; } = false;
        public bool ByStatus { get; set; } = false;
        public bool ByIntervalMode { get; set; } = false;
    }

    
}
