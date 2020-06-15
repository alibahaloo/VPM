using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

        [DisplayName("Resident @")]
        [DataType(DataType.EmailAddress)]
        public string ApplicationUserEmail { get; set; }
        public string Unit { get; set; }

        [DisplayName("Plate #")]
        public string VehiclePlateNumber { get; set; }

        [DisplayName("Show Expired Reservations")]
        public bool ShowExpired { get; set; } = true;

        [DisplayName("From")]
        [DataType(DataType.Date)]
        public DateTime From { get; set; } = DateTime.Today.AddMonths(-1); //Default should show all!

        [DisplayName("To")]
        [DataType(DataType.Date)]
        public DateTime To { get; set; } = DateTime.Today.AddMonths(+1); //Default should show all!
    }
}
