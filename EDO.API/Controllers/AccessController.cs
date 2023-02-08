using EDO.Access;
using EDO.API.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EDO.API.Controllers;

public class AccessController : Controller
{
    private IOptions<AccessConfiguration> _siteSettings;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccessController(IOptions<AccessConfiguration> siteSettings, UserManager<ApplicationUser> userManager)
    {
        this._siteSettings = siteSettings;
        this._userManager = userManager;
    }
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LogInDTO model)
    {
        ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            IEnumerable<string> roles = await _userManager.GetRolesAsync(user);
            List<Claim> authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Vars.TheSecretKey));
            AddRolesToClaims(authClaims, roles);
            var token = new JwtSecurityToken(
                issuer: _siteSettings.Value.Issuer,
                audience: _siteSettings.Value.Audience,
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                userKey = user.Id,
                email = user.Email,
                userName = user.UserName,
                userFullName = String.Format("{0} {1}", user.LastName, user.FirstName),
                access = roles,
            });
        }
        return Unauthorized();
    }

    private void AddRolesToClaims(List<Claim> claims, IEnumerable<string> roles)
    {
        foreach (var role in roles)
        {
            var roleClaim = new Claim(ClaimTypes.Role, role);
            claims.Add(roleClaim);
        }
    }
}
