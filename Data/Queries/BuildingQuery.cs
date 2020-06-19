using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VPM.Data.Entities;

namespace VPM.Data.Queries
{
    public class BuildingQuery
    {
        [Display(Name = "Name")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Address")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Address { get; set; }

        [DisplayName("Show Active Only")]
        public bool ShowActiveOnly { get; set; } = true;

        [DisplayName("All Interval Modes")]
        public bool ShowAllInterval { get; set; } = true;

        [DisplayName("Reservation Interval")]
        public AllowedReservationIntervals ReservationInterval { get; set; } = AllowedReservationIntervals.FH;
    }
}
