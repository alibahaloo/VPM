using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Data.Queries;

namespace VPM.ViewComponents
{
    [ViewComponent(Name = "BuildingList")]
    public class BuildingListViewComponent : ViewComponent
    {
        public IList<Building> Buildings { get; set; }
        public BuildingQuery Query { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(IList<Building> buildings, BuildingQuery query)
        {
            Query = query; //Query is used to set the state of the toolbar according to the shown query
            Buildings = buildings; //To generat ethe list

            return View(this);
        }
    }
}
