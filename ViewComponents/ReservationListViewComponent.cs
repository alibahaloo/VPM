using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Data.Queries;
using VPM.Services;

namespace VPM.ViewComponents
{
    public class AvailableInputs
    {
        public bool ByValidity { get; set; } = false;
        public bool ByUnit { get; set; } = false;
        public bool ByVehiclePlateNumber { get; set; } = false;
        public bool ByBuilding { get; set; } = false;
        public bool ByDate { get; set; } = false;
        public bool ByResident { get; set; } = false;
    }

    [ViewComponent(Name = "ReservationList")]
    public class ReservationListViewComponent : ViewComponent
    {
        private readonly BuildingService _buildingService;
        public ReservationListViewComponent(BuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        public IList<Reservation> Reservations { get; set; }
        public ReservationQuery Query { get; set; }
        public AvailableInputs AvailableInputs { get; set; }
        public SelectList BuildingOptions { get; set; } //Used to populate Building drop-down-list
        public async Task<IViewComponentResult> InvokeAsync(IList<Reservation> reservations, ReservationQuery query)
        {
            BuildingOptions = new SelectList(_buildingService.GetBuildings(), nameof(Building.Id), nameof(Building.Name));
            
            Query = query; //Query is used to set the state of the tool-bar according to the shown query
            Reservations = reservations; //To generate the list

            if (User.IsInRole("Admin"))
            {
                AvailableInputs = new AvailableInputs
                {
                    ByValidity = true,
                    ByUnit = true,
                    ByBuilding = true,
                    ByVehiclePlateNumber = true,
                    ByDate = true,
                    ByResident = true
                };
            }
            else if (User.IsInRole("Manager"))
            {
                AvailableInputs = new AvailableInputs
                {
                    ByValidity = true,
                    ByUnit = true,
                    ByBuilding = false,
                    ByVehiclePlateNumber = true,
                    ByDate = true,
                    ByResident = true
                };
            }
            else
            {
                AvailableInputs = new AvailableInputs
                {
                    ByValidity = true,
                    ByUnit = true,
                    ByBuilding = false,
                    ByVehiclePlateNumber = true,
                    ByDate = true,
                    ByResident = false
                };
            }

            return View(this);
        }
    }
}
