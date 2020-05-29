using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using VPM.Data.Entities;

namespace VPM.Areas.Identity
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public ApplicationUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
                : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("FullName", user.FullName));
            identity.AddClaim(new Claim("BuildingId", user.BuildingId.ToString()));
            identity.AddClaim(new Claim("Id", user.Id.ToString()));

            //Adding roles to claims so it can be detected by User.IsInRole()
            foreach (var role in user.UserRoles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Role.Name));
            }

            return identity;
        }
    }
    
}
