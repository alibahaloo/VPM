using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public Guid BuildingId { get; set; } = Guid.Empty;
        public string ApplicationUserId { get; set; }
        public string Unit { get; set; }

        [DisplayName("Plate Number")]
        public string VehiclePlateNumber { get; set; }

        [DisplayName("Show Expired Reservations")]
        public bool ShowExpired { get; set; } = true;
    }
}
