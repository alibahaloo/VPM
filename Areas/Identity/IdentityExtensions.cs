using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using System.Net.NetworkInformation;

namespace VPM.Areas.Identity
{
    public static class IdentityExtensions
    {
        public static string GetFullName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FullName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static Guid GetBuildingId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("BuildingId");
            // Test for null to avoid issues during local testing
            return (claim != null) ? Guid.Parse(claim.Value) : Guid.Parse(string.Empty);
            //return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Id");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
           // return (claim != null) ? Guid.Parse(claim.Value) : Guid.Parse(string.Empty);
        }
    }
}
