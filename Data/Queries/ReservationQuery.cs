using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VPM.Data.Queries
{
    /*
     * These are the items that Resevation lists can be quered against
     * For ==Getting== reservation lists
     */
    public class ReservationQuery
    {
        [DisplayName("Building")]
        public Guid BuildingId { get; set; } = Guid.Empty;

        [DisplayName("All Buildings")]
        public bool ShowAllBuildings { get; set; } = true;

        [DisplayName("Resident @")]
        [DataType(DataType.EmailAddress)]
        public string ApplicationUserEmail { get; set; }
        public string Unit { get; set; }

        [DisplayName("Plate #")]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string VehiclePlateNumber { get; set; }

        [DisplayName("Show Expired Reservations")]
        public bool ShowExpired { get; set; } = true;

        [DisplayName("Begin")]
        [DataType(DataType.Date)]
        public DateTime From { get; set; } = DateTime.Today.AddMonths(-1); //Default should show all!

        [DisplayName("End")]
        [DataType(DataType.Date)]
        public DateTime To { get; set; } = DateTime.Today.AddMonths(+1); //Default should show all!
    }
}
