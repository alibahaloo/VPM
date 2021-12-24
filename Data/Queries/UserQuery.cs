using System;
using System.ComponentModel.DataAnnotations;

namespace VPM.Data.Queries
{
    public class UserQuery
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Unit")]
        public string Unit { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "All Buildings")]
        public bool ShowAllBuildings { get; set; } = true;
        public Guid BuildingId { get; set; } = Guid.Empty;

        [Display(Name = "All Roles")]
        public bool ShowAllRoles { get; set; } = true;
        public string Role { get; set; } = string.Empty;
    }
}
