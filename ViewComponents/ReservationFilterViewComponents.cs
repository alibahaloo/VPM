using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Data.Queries;
using VPM.Pages.Shared.Components.ReservationFilter;
using VPM.Services;

namespace VPM.ViewComponents
{
    [ViewComponent(Name = "ReservationFilter")]
    public class ReservationFilterViewComponents : ViewComponent
    {
        private readonly BuildingService _buildingService;

        public ReservationFilterViewComponents(BuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        public ReservationQuery Query { get; set; }
        public AvailableInputs AvailableInputs { get; set; }
        public SelectList BuildingOptions { get; set; } //Used to populate Building dropdownlist
        public Guid SelectedBuildingId { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(ReservationQuery query, AvailableInputs availableInputs)
        {
            BuildingOptions = new SelectList(_buildingService.GetBuildings(), nameof(Building.Id), nameof(Building.Name));

            AvailableInputs = availableInputs;
            Query = query;
            return View(this);
        }
    }
}
