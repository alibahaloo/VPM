using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPM.Data.Entities;
using VPM.Data.Queries;
using VPM.Services;
using VPM.Data;

namespace VPM.ViewComponents
{
    public class UserFilterInput : FilterInput
    {
        public bool ByFullName { get; set; } = false;
        public bool ByEmail { get; set; } = false;
        public bool ByRole { get; set; } = false;
        public bool ByUnit { get; set; } = false;
        public bool ByBuilding { get; set; } = false;
    }

    [ViewComponent(Name = "UserList")]
    public class UserListViewComponent : ViewComponent
    {

        public IList<ApplicationUser> Users { get; set; }
        public UserQuery Query { get; set; }
        public UserFilterInput Input { get; set; }
        public SelectList RoleOptions { get; set; } //Used to populate Role drop-down-menu
        //public ApplicationRole Role { get; set; } //TODO: Shouldn't this be ApplicationRole?
        public SelectList BuildingOptions { get; set; } //Used to populate Building drop-down-list

        public UserListViewComponent(RoleService roleService, BuildingService buildingService)
        {
            //Getting list of roles, to populate the drop-down-menu
            RoleOptions = new SelectList(roleService.GetRoles(), nameof(ApplicationRole.Name), nameof(ApplicationRole.Name));
            //Getting list of buildings, to populate the drop-down-menu
            BuildingOptions = new SelectList(buildingService.GetBuildings(), nameof(Building.Id), nameof(Building.Name));
        }

        public async Task<IViewComponentResult> InvokeAsync(IList<ApplicationUser> users, UserQuery query, UserFilterInput input)
        {
            Query = query ?? throw new NotImplementedException("Query cannot be null"); //Query is used to set the state of the tool-bar according to the shown query
            Users = users ?? throw new NotImplementedException("Users cannot be null"); //To generate the list
            Input = input ?? throw new NotImplementedException("Input cannot be null"); //Input has to be give as parameter

            return View(this);
        }
    }
}
