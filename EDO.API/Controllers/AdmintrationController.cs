using EDO.Access.DTO;
using EDO.Access.Mapper ;
using EDO.Access.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace EDO.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdmintrationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AdmintrationController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        this._userManager = userManager;
        this._roleManager = roleManager;
    }

    [Authorize(Roles = RoleConst.STUFF)]
    [HttpGet]
    [Route("User/GetAll")]
    public async Task<IActionResult> GetAll() => Ok((await _userManager.Users.ToListAsync()).ConvertToDTO());

    [Authorize(Roles = RoleConst.STUFF)]
    [HttpGet]
    [Route("User/GetRolesByUserId{Id:Guid}")]
    public async Task<IActionResult> GetUserRolesById(Guid Id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(Convert.ToString(Id));
        IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

        return (roles == null) ? NotFound() : Ok(new
        {
            userId = user.Id,
            access = roles,
        });

    }

    [Authorize(Roles = RoleConst.ADMIN)]
    [HttpPost]
    [Route("User/CreateUser")]
    public async Task<IActionResult> CreateAccount([FromBody] RegistrationDTO userDTO)
    {
        try
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                ThirdName = userDTO.ThirdName,
                PhoneNumber = userDTO.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (!result.Succeeded)
                throw new BadHttpRequestException(result.Errors.Select(x => x.Description).ToString());

            if (result.Succeeded)
                await _userManager.AddToRolesAsync(user, new string[] { RoleConst.NEWUSER });

            return Ok(userDTO);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = RoleConst.ADMIN)]
    [HttpPost]
    [Route("User/AddRoleToUser")]
    public async Task<IActionResult> AddRoleToUser([FromBody] UserRolesDTO userRolesDTO)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userRolesDTO.UserId);
            IList<string> addingRoles = new List<string>();
            foreach (var role in userRolesDTO.Roles)
                addingRoles.Add(role.Name);

            var result = await _userManager.AddToRolesAsync(user, addingRoles);

            if (!result.Succeeded)
                throw new BadHttpRequestException(result.Errors.Select(x => x.Description).ToString());
            var userRoles = await _userManager.GetRolesAsync(user);

            IList<RoleDTO> rolesDTOs = new List<RoleDTO>();
            foreach (var role in userRoles)
            {
                var userRole = await _roleManager.FindByNameAsync(role);
                rolesDTOs.Add(userRole.ConvertToDTO());
            }

            userRolesDTO.Roles = rolesDTOs;
            return Ok(userRolesDTO);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = RoleConst.ADMIN)]
    [HttpPost]
    [Route("User/RemoveRoleFromUser")]
    public async Task<IActionResult> RemoveRoleFromUser(UserRolesDTO userRolesDTO)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userRolesDTO.UserId);

            IList<string> removingRoles = new List<string>();

            foreach (var role in userRolesDTO.Roles)
                removingRoles.Add(role.Name);

            var result = await _userManager.RemoveFromRolesAsync(user, removingRoles);

            if (!result.Succeeded)
                throw new BadHttpRequestException(result.Errors.Select(x => x.Description).ToString());

            var userRoles = await _userManager.GetRolesAsync(user);

            IList<RoleDTO> rolesDTO = new List<RoleDTO>();
            foreach (var role in userRoles)
            {
                var userRole = await _roleManager.FindByNameAsync(role);
                rolesDTO.Add(userRole.ConvertToDTO());
            }

            userRolesDTO.Roles = rolesDTO;
            return Ok(userRolesDTO);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = RoleConst.STUFF)]
    [HttpPost]
    [Route("User/ChangeUser")]
    public async Task<IActionResult> ChangeUser([FromBody]UpdateProfileDTO userDTO)
    {
        var userName = _userManager.GetUserId(User);
        var _user = await _userManager.FindByNameAsync(userName);
        if (_user.Id == null || (!User.IsInRole(RoleConst.ADMIN) && _user.Id != userDTO.Id))
            return Forbid();

        ApplicationUser user = await _userManager.FindByIdAsync(userDTO.Id);
        if (user == null)
            return NotFound();
        user.LastName = userDTO.LastName;
        user.FirstName = userDTO.FirstName;
        user.ThirdName = userDTO.ThirdName;
        user.PhoneNumber = userDTO.PhoneNumber;
        user.Email = userDTO.Email;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return BadRequest(new { Successful = false, Errors = result.Errors.Select(x => x.Description) });

        if (userDTO.Password == userDTO.ConfirmPassword && user != null)
        {
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, userDTO.Password);
        }

        return Ok(user.ConvertToDTO());
    }
}

