using EDO.Access;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace EDO.API;

public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
{
    public MyUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor) { }
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName));
        identity.AddClaim(new Claim("UserName", user.UserName));
        identity.AddClaim(new Claim("Email", user.Email));
        return identity;
    }
}