using EDO.Access.DTO;
using EDO.Access.Models;
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
        if(!ModelState.IsValid)
        {
            return BadRequest("Model state isn't valid");
        }
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
                firstName = user.FirstName,
                lastName = user.LastName,
                thirdName = user.ThirdName,
                phoneNumber = user.PhoneNumber,
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


    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> RegistrationAsync([FromBody] RegistrationDTO registrationDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model State isn't valid");

        ApplicationUser user = await _userManager.FindByNameAsync(registrationDTO.UserName);

        if (user != null)
            return BadRequest("This user alrady Created.");

        ApplicationUser applicationUser = new ApplicationUser()
        {
            UserName = registrationDTO.UserName,
            FirstName = registrationDTO.FirstName,
            LastName = registrationDTO.LastName,
            ThirdName = registrationDTO.ThirdName,
            Email = registrationDTO.Email,
            PhoneNumber = registrationDTO.PhoneNumber
        };
        var result = await _userManager.CreateAsync(applicationUser, registrationDTO.Password);
        if (result.Succeeded)
            await _userManager.AddToRolesAsync(applicationUser, new string[] { RoleConst.NEWUSER });

        return Ok(result);
    }
}
